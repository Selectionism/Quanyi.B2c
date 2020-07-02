using Quanyi.B2c.Yaofangwang.Business.Order;
using Quanyi.B2c.Yaofangwang.ViewModel.Order;
using Quanyi.Entity.DBEntity.Medicine;
using Quanyi.Entity.DBEntity.Order;
using Quanyi.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanyi.B2c.Yaofangwang
{
    public partial class FormMain : Form
    {
        private string storage = ConfigurationManager.AppSettings["Storage"];
        //订单集合提醒
        private const int PROGRESS_BILL = -1;
        //业务报错提醒
        private const int PROGRESS_ERROR = -9;
        //新的订单声音提醒
        private const int PROGRESS_NEWBILL = -2;
        private const int PROGRESS_MEDICINE = -5;
        private const int PROGRESS_STOCK = -3;
        private const int PROGRESS_STOCKSUCCESS = -4;
        private OrderService _orderService;
        private B2cPuller _puller;
        private B2cMedicine _medicineService;
        private SapWebApi _sapWebApi;
        //订单dgv数据集合
        private BindingList<DataGridViewBillInfo> billBindingList;
        //订单明细数据集合
        private List<BillProInfo> billProInfoList;
        //判断是否是第一次打开程序
        private bool IsFirstOpened = false;
        //状态列排序规则
        private bool statusAsc = false;
        //指示当前backgroundwork1是否正在工作
        private bool bgw1Working = false;
        //private bool bgw2Working = false;

        public FormMain()
        {
            InitializeComponent();
            string storage = ConfigurationManager.AppSettings["Storage"];
            _orderService = new OrderService();
            _puller = new B2cPuller();
            _medicineService = new B2cMedicine();
            _sapWebApi = new SapWebApi();
            dgvBillInfo.AllowUserToAddRows = false;
            dgvBillInfo.RowHeadersVisible = false;
            dgvBillProInfo.AllowUserToAddRows = false;
            dgvBillProInfo.RowHeadersVisible = false;
            dgvBillInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBillInfo.MultiSelect = false;
            dgvBillInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvBillInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvBillProInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvBillProInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            billBindingList = new BindingList<DataGridViewBillInfo>();
            billProInfoList = new List<BillProInfo>();
            IsFirstOpened = true;
            InitComboBillStatus();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void InitComboBillStatus()
        {
            var items = new List<BillStatusListItem>();
            items.Add(new BillStatusListItem()
            {
                Text = "请选择",
                Value = "-1"
            });
            items.Add(new BillStatusListItem()
            {
                Text = "暂未付款",
                Value = "10"
            });
            items.Add(new BillStatusListItem()
            {
                Text = "等待发货",
                Value = "11"
            });
            items.Add(new BillStatusListItem()
            {
                Text = "申请退款",
                Value = "111"
            });
            items.Add(new BillStatusListItem()
            {
                Text = "取消申请退款",
                Value = "112"
            });
            items.Add(new BillStatusListItem()
            {
                Text = "拒绝退款",
                Value = "113"
            });
            items.Add(new BillStatusListItem()
            {
                Text = "暂未收货",
                Value = "13"
            });
            items.Add(new BillStatusListItem()
            {
                Text = "交易完成",
                Value = "14"
            });
            items.Add(new BillStatusListItem()
            {
                Text = "交易失败",
                Value = "15"
            });
            items.Add(new BillStatusListItem()
            {
                Text = "正在退款",
                Value = "16"
            });
            items.Add(new BillStatusListItem()
            {
                Text = "交易取消",
                Value = "17"
            });
            cboBillStatus.ComboBox.DisplayMember = "Text";//显示
            cboBillStatus.ComboBox.ValueMember = "Value";//值
            cboBillStatus.ComboBox.DataSource = items;
            cboBillStatus.ComboBox.SelectedValue = "-1";//设定选择项
        }

        private void ShowForm()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //已禁用最大化，因此单击直接最大化，而不是恢复正常
                WindowState = FormWindowState.Maximized;
            }
            Show();
            BringToFront();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            //如果是天猫的，暂时订单拉取只支持手动导入，同时不支持对库存和商品的操作
            if(storage.Equals("TMall"))
            {
                btnImportOrder.Visible = true;
                btnImport.Visible = false;
                btnModifyMedicine.Visible = false;
                cbStockUpdate.Visible = false;
                cbPeriodUpdate.Visible = false;
                toolStripLabel1.Visible = false;
                cboBillStatus.Visible = false;
            }
            else
            {
                backgroundWorker2.RunWorkerAsync();
                backgroundWorker3.RunWorkerAsync();
                backgroundWorker4.RunWorkerAsync();
                backgroundWorker1.RunWorkerAsync();
            }
            backgroundWorker1.RunWorkerAsync();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                e.Cancel = backgroundWorker1.IsBusy;
            }
            else if (!backgroundWorker1.CancellationPending)
            {
                notifyIcon1.Visible = false;
                backgroundWorker1.CancelAsync();
            }
            else if (!backgroundWorker2.CancellationPending)
            {
                backgroundWorker2.CancelAsync();
            }
            else if (!backgroundWorker3.CancellationPending)
            {
                backgroundWorker3.CancelAsync();
            }
            else if (!backgroundWorker4.CancellationPending)
            {
                backgroundWorker4.CancelAsync();
            }
        }

        public void RefreshOrder()
        {
            try
            {
                bool isNew;
                string msg;
                _orderService.GetNewOrder(out msg, IsFirstOpened, billBindingList, billProInfoList, out isNew, storage);
                dgvBillInfo.DataSource = billBindingList;
                //最后一列，即状态名称列，支持自定义排序
                dgvBillInfo.Columns[dgvBillInfo.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.Programmatic;
                //调用药房网或者插入数据库有可能有错误返回消息，是否对消息文本框做进一步处理？
                tslMsg.Text = $"共有{billBindingList.Count}单";
                //行切换事件
                dgvBillInfo.RowEnter += Dgv_RowEnter;
                //列表头单击事件
                dgvBillInfo.ColumnHeaderMouseClick += Dgv_ColumnHeaderMouseClick;
                ////单元格点击事件
                dgvBillInfo.CellClick += Dgv_CellClick;
                if(isNew)
                {
                    Speech("有新的订单");
                }
            }
            catch (Exception ex)
            {
                //显示错误信息
                tslMsg.Text = ex?.Message;
            }
        }

        /// <summary>
        /// 用于承载异步操作-定时拉取订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    //正在执行
                    bgw1Working = true;
                    bool isNew;
                    string msg;
                    _orderService.GetNewOrder(out msg, IsFirstOpened, billBindingList, billProInfoList, out isNew, storage);
                    //第一次报告返回订单集合
                    backgroundWorker1.ReportProgress(PROGRESS_BILL, billBindingList);
                    //紧接着，看是否有新的订单进来bindinglist，有则声音提醒
                    if (isNew)
                    {
                        backgroundWorker1.ReportProgress(PROGRESS_NEWBILL, "new bills coming");
                    }
                    //如果还有错误，就顺便报告一下错误
                    if (msg != "")
                    {
                        backgroundWorker1.ReportProgress(PROGRESS_ERROR, msg);
                    }
                }
                catch (Exception ex)
                {
                    backgroundWorker1.ReportProgress(PROGRESS_ERROR, ex);
                }
                finally
                {
                    //一次执行完成
                    bgw1Working = false;
                    //第一次打开之后，设置是否第一次打开值为false
                    IsFirstOpened = false;
                    Thread.Sleep(120 * 1000);
                }
            }
        }

        /// <summary>
        /// 报告进度事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //订单集合
            if (e.ProgressPercentage == PROGRESS_BILL)
            {
                dgvBillInfo.DataSource = billBindingList;
                //最后一列，即状态名称列，支持自定义排序
                dgvBillInfo.Columns[dgvBillInfo.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.Programmatic;
                //调用药房网或者插入数据库有可能有错误返回消息，是否对消息文本框做进一步处理？
                tslMsg.Text = $"共有{billBindingList.Count}单";
                //行切换事件
                dgvBillInfo.RowEnter += Dgv_RowEnter;
                //列表头单击事件
                dgvBillInfo.ColumnHeaderMouseClick += Dgv_ColumnHeaderMouseClick;
                ////单元格点击事件
                dgvBillInfo.CellClick += Dgv_CellClick;
                //dgvBillInfo.ClearSelection();
            }
            else if (e.ProgressPercentage == PROGRESS_ERROR)//异常错误报告
            {
                Type ex = typeof(Exception);
                Type txt = typeof(String);
                Type userStat = e.UserState.GetType();
                if(ex.Equals(userStat))
                {
                    //显示错误信息
                    tslMsg.Text = (e.UserState as Exception)?.Message;
                }
                else
                {
                    tslMsg.Text = e.UserState as String;
                }
            }
            else//新的订单声音提醒
            {
                Speech("有新的订单");
            }
        }

        /// <summary>
        /// 异步操作完成或取消时执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                tslMsg.Text = e.Error.ToString();
                return;
            }
            if (!e.Cancelled)
                tslMsg.Text = "处理完毕!";
            else
                tslMsg.Text = "处理终止!";
        }

        /// <summary>
        /// 用于承载异步操作-定时拉取订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            //while(true)
            //{
            //    try
            //    {
            //        //正在执行
            //        bool isNew;
            //        string msg;
            //        _orderService.GetNewOrder(out msg, IsFirstOpened, billBindingList, billProInfoList, out isNew, storage);
            //        //第一次报告返回订单集合
            //        backgroundWorker5.ReportProgress(PROGRESS_BILL, billBindingList);
            //        //紧接着，看是否有新的订单进来bindinglist，有则声音提醒
            //        if (isNew)
            //        {
            //            backgroundWorker5.ReportProgress(PROGRESS_NEWBILL, "new bills coming");
            //        }
            //        //如果还有错误，就顺便报告一下错误
            //        if (msg != "")
            //        {
            //            backgroundWorker5.ReportProgress(PROGRESS_ERROR, msg);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        backgroundWorker5.ReportProgress(PROGRESS_ERROR, ex);
            //    }
            //    finally
            //    {
            //        //第一次打开之后，设置是否第一次打开值为false
            //        IsFirstOpened = false;
            //        Thread.Sleep(240 * 1000);
            //    }
            //}
        }

        /// <summary>
        /// 报告进度事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker5_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //订单集合
            //if (e.ProgressPercentage == PROGRESS_BILL)
            //{
            //    dgvBillInfo.DataSource = billBindingList;
            //    //最后一列，即状态名称列，支持自定义排序
            //    dgvBillInfo.Columns[dgvBillInfo.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.Programmatic;
            //    //调用药房网或者插入数据库有可能有错误返回消息，是否对消息文本框做进一步处理？
            //    tslMsg.Text = $"共有{billBindingList.Count}单";
            //    //行切换事件
            //    dgvBillInfo.RowEnter += Dgv_RowEnter;
            //    //列表头单击事件
            //    dgvBillInfo.ColumnHeaderMouseClick += Dgv_ColumnHeaderMouseClick;
            //    ////单元格点击事件
            //    dgvBillInfo.CellClick += Dgv_CellClick;
            //    //dgvBillInfo.ClearSelection();
            //}
            //else if (e.ProgressPercentage == PROGRESS_ERROR)//异常错误报告
            //{
            //    Type ex = typeof(Exception);
            //    Type txt = typeof(String);
            //    Type userStat = e.UserState.GetType();
            //    if (ex.Equals(userStat))
            //    {
            //        //显示错误信息
            //        tslMsg.Text = (e.UserState as Exception)?.Message;
            //    }
            //    else
            //    {
            //        tslMsg.Text = e.UserState as String;
            //    }
            //}
            //else//新的订单声音提醒
            //{
            //    Speech("有新的订单");
            //}
        }

        /// <summary>
        /// 异步操作完成或取消时执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                tslMsg.Text = e.Error.ToString();
                return;
            }
            if (!e.Cancelled)
                tslMsg.Text = "处理完毕!";
            else
                tslMsg.Text = "处理终止!";
        }

        /// <summary>
        /// 定时更新药房网库存等信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            //建议定时获取SAP大仓库存等信息的时候，同步药房网只更新库存，调用批量更新库存信息接口
            while (true)
            {
                if(!cbStockUpdate.Checked)
                {
                    //自动更新库存开关未打开，则不执行以下代码
                    continue;
                }
                try
                {
                    backgroundWorker2.ReportProgress(PROGRESS_STOCK, "正在同步新的库存信息");
                    var medicineList = _medicineService.GetMedicineListForStockUpdate();
                    //每次都同步商品表全部记录，随着记录增加，是否要优化逻辑？
                    if (medicineList != null && medicineList.Count > 0)
                    {
                        //每次只同步20条
                        int cycle = 20;
                        //向下取整，总共循环多少次
                        int multiple = medicineList.Count / cycle;
                        int reminder = medicineList.Count % cycle;
                        //string storage = ConfigurationManager.AppSettings["Storage"];
                        for (int i = 0; i < multiple; i++)
                        {
                            var currList = medicineList.Skip(i * cycle).Take(cycle).ToList();
                            MedicineStockBatchUpdate(currList, storage);
                        }
                        if (reminder != 0)
                        {
                            var currList = medicineList.Skip(multiple * cycle).Take(reminder).ToList();
                            MedicineStockBatchUpdate(currList, storage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    backgroundWorker2.ReportProgress(PROGRESS_ERROR, ex);
                }
                finally
                {
                    //bgw2Working = false;
                    //缩短时间，先改成20分钟同步一次
                    Thread.Sleep(60 * 20 * 1000);
                }
            }
        }

        /// <summary>
        /// 报告进度事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage == PROGRESS_STOCK)
            {
                Speech("正在同步新的库存信息");
            }
            //if (e.ProgressPercentage == PROGRESS_STOCKSUCCESS)
            //{
            //    Speech("库存同步成功");
            //}
            if (e.ProgressPercentage == PROGRESS_ERROR)
            {
                var ex = typeof(Exception);
                var str = typeof(String);
                if(e.UserState.GetType()== ex)
                {
                    Speech((e.UserState as Exception)?.Message);
                }
                else
                {
                    Speech(e.UserState as string);
                }
            }
            //if (e.ProgressPercentage == PROGRESS_BILL)
            //{
            //    Speech("正在同步新的库存信息");
            //    return;
            //}
            ////显示错误信息
            //tslMsg.Text = (e.UserState as Exception)?.Message;
        }

        /// <summary>
        /// 异步操作完成或取消时执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                tslMsg.Text = e.Error.ToString();
                return;
            }
            if (!e.Cancelled)
                tslMsg.Text = "定时同步库存信息程序处理完毕!";
            else
                tslMsg.Text = "定时同步库存信息程序处理终止!";
        }

        /// <summary>
        /// 定时调用药房网订单详情接口，刷新dgv订单的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                //如果bgw1正在执行异步操作，不能执行3
                if (!bgw1Working && billBindingList.Count > 0)
                {
                    //一条条传(由于foreach在集合索引已变得情况下不被允许，就改成for循环吧)
                    for (int i = 0; i < billBindingList.Count; i++)
                    {
                        try
                        {
                            string msg;
                            var bill = _puller.RegularUpdateBillStatus(out msg, billBindingList[i].OrderNo);
                            if (msg != "")
                            {
                                backgroundWorker3.ReportProgress(PROGRESS_ERROR, msg);
                                continue;
                            }
                            if (!bill.status_id.Equals(billBindingList[i].StatusId) && !bill.status_name.Equals(billBindingList[i].StatusName))
                            {
                                //随着bindinglist项目的值发生变化，向progresschanged报告，重置dgv的数据源
                                billBindingList[i].StatusId = bill.status_id;
                                billBindingList[i].StatusName = bill.status_name;
                            }
                        }
                        catch (Exception ex)
                        {
                            backgroundWorker3.ReportProgress(PROGRESS_ERROR, ex);
                        }
                    }
                    //一起处理完bindinglist，给changed事件报告
                    backgroundWorker3.ReportProgress(PROGRESS_BILL, billBindingList);
                }
                //隔个5秒刷新一次
                Thread.Sleep(5 * 1000);
            }
        }

        /// <summary>
        /// 报告进度事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == PROGRESS_BILL)
            {
                dgvBillInfo.DataSource = billBindingList;
                dgvBillInfo.Refresh();
            }
            else if (e.ProgressPercentage == PROGRESS_ERROR)
            {
                Type ex = typeof(Exception);
                Type txt = typeof(String);
                Type userStat = e.UserState.GetType();
                if (ex.Equals(userStat))
                {
                    //显示错误信息
                    tslMsg.Text = (e.UserState as Exception)?.Message;
                }
                else
                {
                    tslMsg.Text = e.UserState as String;
                }
            }
        }

        /// <summary>
        /// 异步操作完成或取消时执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                tslMsg.Text = e.Error.ToString();
                return;
            }
            if (!e.Cancelled)
                tslMsg.Text = "定时更新订单状态处理完毕!";
            else
                tslMsg.Text = "定时更新订单状态处理终止!";
        }

        /// <summary>
        /// 定时更新药房网效期等信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                if(!cbPeriodUpdate.Checked)
                {
                    continue;
                }
                try
                {
                    backgroundWorker4.ReportProgress(PROGRESS_STOCK, "正在同步新的SAP效期等信息");
                    var medicineList = _medicineService.GetAllMedicineList();
                    if (medicineList != null && medicineList.Count > 0)
                    {
                        //关于批量更新库存，一次可以传20条；关于批量更新药品信息，由于传入字段比较多，建议一次只传10条
                        int cycle = 10;
                        int multiple = medicineList.Count / cycle;
                        int reminder = medicineList.Count % cycle;
                        //string storage = ConfigurationManager.AppSettings["Storage"];
                        for (int i = 0; i < multiple; i++)
                        {
                            var currList = medicineList.Skip(i * cycle).Take(cycle).ToList();
                            MedicinePeriodBatchUpdate(currList, storage);
                        }
                        if (reminder != 0)
                        {
                            var currList = medicineList.Skip(multiple * cycle).Take(reminder).ToList();
                            MedicinePeriodBatchUpdate(currList, storage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    backgroundWorker4.ReportProgress(PROGRESS_ERROR, ex);
                }
                finally
                {
                    //缩短时间，先改成20分钟同步一次
                    Thread.Sleep(60 * 20 * 1000);
                }
            }
        }

        /// <summary>
        /// 报告进度事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //哪些成功，哪些失败？失败是否要记录失败原因，方便下次继续回传？
            if (e.ProgressPercentage == PROGRESS_MEDICINE)
            {
                //是否显示进度条？
            }
            else
            {
                //失败
                tslMsg.Text = e.UserState.GetType() == typeof(Exception) ? (e.UserState as Exception)?.Message : e.UserState as String;
            }
        }

        /// <summary>
        /// 异步操作完成或取消时执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                tslMsg.Text = e.Error.ToString();
                return;
            }
            if (!e.Cancelled)
                tslMsg.Text = "定时同步SAP效期等信息程序处理完毕!";
            else
                tslMsg.Text = "定时同步SAP效期等信息程序处理终止!";
        }

        private void MedicinePeriodBatchUpdate(List<MedicineInfo> currList, string storage)
        {
            DateTime beginTime = DateTime.Now;
            var sapStockList = storage.Equals("JY02") ? _medicineService.GetPosAvailStockInfo(currList) : _sapWebApi.GetAvailableStockInfo(currList);
            DateTime endTime = DateTime.Now;
            TimeSpan ts = endTime.Subtract(beginTime);
            int sec = (int)ts.TotalSeconds;
            if (sapStockList.Code.Equals("S") && sapStockList.Items != null && sapStockList.Items.Count > 0)
            {
                string msg;
                bool result = _medicineService.MedicinePeriodBatchUpdate(currList, sapStockList, out msg);
                if (result)
                {
                    //backgroundWorker4.ReportProgress(PROGRESS_STOCKSUCCESS, "库存同步成功");
                }
                else
                {
                    backgroundWorker4.ReportProgress(PROGRESS_ERROR, $"SAP效期等信息同步失败：{msg}");
                }
            }
        }

        private void MedicineStockBatchUpdate(List<MedicineInfo> currList, string storage)
        {
            DateTime beginTime = DateTime.Now;
            var sapStockList = storage.Equals("JY02") ? _medicineService.GetPosAvailStockInfo(currList) : _sapWebApi.GetAvailableStockInfo(currList);
            DateTime endTime = DateTime.Now;
            TimeSpan ts = endTime.Subtract(beginTime);
            int sec = (int)ts.TotalSeconds;
            if (sapStockList.Code.Equals("S") && sapStockList.Items != null && sapStockList.Items.Count > 0)
            {
                //有一些品种，SAP可用库存和数据库可用库存相等，则不再次同步药房网
                string msg;
                bool result = _medicineService.MedicineStockBatchUpdate(currList, sapStockList, out msg);
                if (result)
                {
                    //backgroundWorker2.ReportProgress(PROGRESS_STOCKSUCCESS, "库存同步成功");
                }
                else
                {
                    backgroundWorker2.ReportProgress(PROGRESS_ERROR, $"库存同步失败：{msg}");
                }
            }
        }

        /// <summary>
        /// 点击列头部支持排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //单击列头
            if (e.ColumnIndex == dgvBillInfo.Columns.Count - 1)
            {
                //并且单击的是最后一列 
                if (billBindingList.Count > 0)
                {
                    var orderByList = !statusAsc ? billBindingList.OrderBy(n => n.StatusName).ToList()
                        : billBindingList.OrderByDescending(n => n.StatusName).ToList();
                    var orderByBillBindingList = new BindingList<DataGridViewBillInfo>();
                    orderByList.ForEach(n =>
                    {
                        orderByBillBindingList.Add(new DataGridViewBillInfo()
                        {
                            OrderTime = n.OrderTime,
                            OrderNo = n.OrderNo,
                            BuyerName = n.BuyerName,
                            BuyerMobile = n.BuyerMobile,
                            ReceiverName = n.ReceiverName,
                            ReceiverMobile = n.ReceiverMobile,
                            StatusId = n.StatusId,
                            StatusName = n.StatusName
                        });
                    });
                    billBindingList = orderByBillBindingList;
                    statusAsc = !statusAsc;
                    dgvBillInfo.DataSource = billBindingList;
                }
            }
        }

        /// <summary>
        /// dgv行切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dgv_RowEnter(object sender,DataGridViewCellEventArgs e)
        {
            if(dgvBillInfo.CurrentRow==null)
            {
                return;
            }
            //表头行索引是-1
            if (e.RowIndex > -1)
            {
                string orderNo = dgvBillInfo.Rows[e.RowIndex].Cells["OrderNo"].Value.ToString();
                //获取该订单号对应的明细
                var dgvBillProInfoList = new List<DataGridViewBillProInfo>();
                foreach (var item in billProInfoList.Where(n => n.OrderNo.Equals(orderNo)).ToList())
                {
                    dgvBillProInfoList.Add(new DataGridViewBillProInfo()
                    {
                        PrimaryKey = item.PrimaryKey,
                        //ProductNumber = item.ProductNumber,
                        Aliascn = item.Aliascn,
                        Standard = item.Standard,
                        //TrocheType = item.TrocheType,
                        ProduceNo = item.ProduceNo,
                        //UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                        //Total = item.Total,
                        //ReturnQuantity = item.ReturnQuantity
                    });
                }
                //也就是说，一次只展示一个单号的明细信息
                dgvBillProInfo.DataSource = dgvBillProInfoList;
            }
        }

        /// <summary>
        /// dgv单元格单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ////列表头
            //if(e.RowIndex==-1)
            //{
            //    //又是点击第一列选择列
            //    if (dgvBillInfo.Columns[e.ColumnIndex].HeaderText.Equals("选择"))
            //    {
            //        var dt = new BindingList<DataGridViewBillInfo>();
            //        foreach (var item in billBindingList)
            //        {
            //            item.IsChecked = item.IsChecked ? false : true;
            //        }
            //    }
            //}
            //dgvBillInfo.Refresh();
            if (dgvBillInfo.Columns[e.ColumnIndex].HeaderText.Equals("选择"))
            {
                billBindingList[e.RowIndex].IsChecked = !billBindingList[e.RowIndex].IsChecked;
            }
            dgvBillInfo.Refresh();
        }

        /// <summary>
        /// 声音提醒事件
        /// </summary>
        /// <param name="text"></param>
        private void Speech(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            try
            {
                var voice = new SpeechSynthesizer
                {
                    Volume = 100
                };
                voice.SetOutputToDefaultAudioDevice();
                voice.SpeakAsync(text);
            }
            catch 
            {

            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //确认订单是否请货
            if (e.ClickedItem == btnCommit)
            {
                dgvBillInfo.EndEdit();
                try
                {
                    dgvBillInfo.SuspendLayout();
                    var currList = dgvBillInfo.DataSource as BindingList<DataGridViewBillInfo>;
                    var selectedDgvBindingList = currList.Where(n => n.IsChecked).ToList();
                    if (selectedDgvBindingList.Count == 0)
                    {
                        MessageBox.Show("请选择订单！");
                        return;
                    }
                    var isOk = MessageBox.Show("确认请货吗？", "提示", MessageBoxButtons.OKCancel);
                    if (isOk == DialogResult.OK)
                    {
                        var orderNoList = selectedDgvBindingList.Select(n => n.OrderNo).Distinct().ToList();
                        _puller.StoreNeed(selectedDgvBindingList, billProInfoList.Where(n => orderNoList.Contains(n.OrderNo)).ToList());
                        //成功没有成功提示
                        foreach (var item in selectedDgvBindingList)
                        {
                            billBindingList.Remove(item);
                        }
                        tslMsg.Text = $"共有{billBindingList.Count}单";
                        //删完重置数据集合
                        dgvBillInfo.DataSource = billBindingList;
                        dgvBillProInfo.DataSource = null;
                        dgvBillProInfo.Refresh();
                        MessageBox.Show("订单请货成功！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("订单请货失败！");
                    backgroundWorker1.ReportProgress(PROGRESS_ERROR, ex);
                }
                finally
                {
                    dgvBillInfo.ResumeLayout();
                }
            }
            //删除
            if(e.ClickedItem==btnDelete)
            {
                dgvBillInfo.EndEdit();
                try
                {
                    dgvBillInfo.SuspendLayout();
                    var currList = dgvBillInfo.DataSource as BindingList<DataGridViewBillInfo>;
                    var selectedDgvBindingList = currList.Where(n => n.IsChecked).ToList();
                    if (selectedDgvBindingList.Count == 0)
                    {
                        MessageBox.Show("请选择订单！");
                        return;
                    }
                    var isOk = MessageBox.Show("确认删除订单吗？", "警告", MessageBoxButtons.OKCancel);
                    if(isOk==DialogResult.OK)
                    {
                        var orderNoList = selectedDgvBindingList.Select(n => n.OrderNo).Distinct().ToList();
                        _puller.DeleteOrder(selectedDgvBindingList);
                        //成功没有成功提示
                        foreach (var item in selectedDgvBindingList)
                        {
                            billBindingList.Remove(item);
                        }
                        tslMsg.Text = $"共有{billBindingList.Count}单";
                        //删完重置数据集合
                        dgvBillInfo.DataSource = billBindingList;
                        dgvBillProInfo.DataSource = null;
                        dgvBillProInfo.Refresh();
                        MessageBox.Show("订单删除成功！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("订单删除失败！");
                    backgroundWorker1.ReportProgress(PROGRESS_ERROR, ex);
                }
                finally
                {
                    dgvBillInfo.ResumeLayout();
                }
            }
            //导入
            if (e.ClickedItem == btnImport)
            {
                //if(bgw2Working)
                //{
                //    MessageBox.Show("当前后台正在执行自动同步SAP库存工作，请过20分钟再导入商品");
                //    return;
                //}
                var frmImport = new FormImport();
                frmImport.StartPosition = FormStartPosition.CenterScreen;
                frmImport.Show();
            }
            //全选
            if (e.ClickedItem == btnSelectAll)
            {
                if (billBindingList.Count>0)
                {
                    dgvBillInfo.EndEdit();
                    dgvBillInfo.SuspendLayout();
                    foreach (var item in billBindingList)
                    {
                        item.IsChecked = true;
                    }
                    ////切换一行
                    dgvBillInfo.CurrentCell = dgvBillInfo.Rows[1].Cells["OrderNo"];
                    dgvBillInfo.Refresh();
                    dgvBillInfo.ResumeLayout();
                }
            }
            //修改商品信息
            if(e.ClickedItem==btnModifyMedicine)
            {
                var frmModify = new FormModifyMedicine();
                frmModify.StartPosition = FormStartPosition.CenterScreen;
                frmModify.Show();
            }
            if(e.ClickedItem==btnImportOrder)
            {
                var frmImportOrder = new FrmImportOrder(this);
                frmImportOrder.StartPosition = FormStartPosition.CenterScreen;
                frmImportOrder.Show();
            }
        }

        /// <summary>
        /// 判断是否最小化，然后显示到托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if(WindowState==FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                ShowInTaskbar = false;
                //图标显示在托盘区
                notifyIcon1.Visible = true;
            }
        }

        /// <summary>
        /// 确认是否退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("是否确认退出程序？","退出",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.OK)
            {
                //关闭所有的线程
                Dispose();
                Close();
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 托盘右键显示主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDisplay_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// 托盘右键退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiExit_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("是否确认退出程序？","退出",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.OK)
            {
                //关闭所有的线程
                Dispose();
                Close();
            }
        }

        private void notifyIcon1_MouseClick(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void cboBillStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BillStatusListItem selectedItem = (BillStatusListItem)cboBillStatus.SelectedItem;
            string value = selectedItem.Value;//值
            string text = selectedItem.Text;//显示的文字
            //第一次打开，默认不执行
            if(IsFirstOpened && value.Equals("-1"))
            {
                return;
            }
            //MessageBox.Show(text);
            var selectedList = billBindingList.Where(n => n.StatusId.Equals(value)).ToList();
            var selectedBillBindingList = new BindingList<DataGridViewBillInfo>();
            selectedList.ForEach(n =>
            {
                selectedBillBindingList.Add(new DataGridViewBillInfo()
                {
                    OrderTime = n.OrderTime,
                    OrderNo = n.OrderNo,
                    BuyerName = n.BuyerName,
                    BuyerMobile = n.BuyerMobile,
                    ReceiverName = n.ReceiverName,
                    ReceiverMobile = n.ReceiverMobile,
                    StatusId = n.StatusId,
                    StatusName = n.StatusName
                });
            });
            //billBindingList = selectedBillBindingList;
            if(value.Equals("-1"))
            {
                dgvBillInfo.DataSource = billBindingList;
            }
            else
            {
                dgvBillInfo.DataSource = selectedBillBindingList;
            }
        }

        private void dgvBillInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dgvBillProInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}

using Quanyi.B2c.Yaofangwang.ViewModel.Medicine;
using Quanyi.Entity.DBEntity.Medicine;
using Quanyi.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanyi.B2c.Yaofangwang
{
    public partial class FormModifyMedicine : Form
    {
        private const int PROGRESS_MEDICINE = -1;
        private const int PROGRESS_ERROR = -9;
        private B2cMedicine _medicineService;
        private List<DataGridViewMedicineInfo> _medicineList;
        //状态列排序规则
        private bool statusAsc = false;
        private Dictionary<string, string> medicineStatusDic;
        private SapWebApi _sapWebApi;

        public FormModifyMedicine()
        {
            InitializeComponent();
            dgvMedicine.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _medicineService = new B2cMedicine();
            //_medicineList = new List<DataGridViewMedicineInfo>();
            medicineStatusDic = new Dictionary<string, string>() 
            {
                {"1","发布" },
                {"2","热销" },
                {"3","促销" },
                {"4","新品" },
                {"5","推荐" },
                {"-999","主动下架" },
            };
            _sapWebApi = new SapWebApi();
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

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            //展示即调用一次bgw，里面执行的是加载一次当前商品资料表记录
            backgroundWorker1.RunWorkerAsync();
        }

        /// <summary>
        /// 回车键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrimaryKey_KeyDown(object sender,KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                //第一次只是预加载数据集合到dgv，再次点击搜索按钮，还是要查询一次数据库
                var primaryKey = string.IsNullOrEmpty(txtPrimaryKey.Text.Trim()) ? "" : txtPrimaryKey.Text.Trim();
                //RunWorkerAsync方法支持传参形式的重载
                backgroundWorker1.RunWorkerAsync(primaryKey);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //上线之后，商品资料表导入的记录增多，为了避免一次加载，影响用户界面体验，特引入bgw
            try
            {
                //每次刷新之前都重置_medicineList
                _medicineList = new List<DataGridViewMedicineInfo>();
                List<MedicineInfo> medicineList = new List<MedicineInfo>();
                //判断是否传了参数
                if(e.Argument==null)
                {
                    medicineList = _medicineService.GetAllMedicineList();
                }
                else
                {
                    medicineList = _medicineService.GetMedicineList(e.Argument.ToString());
                }
                medicineList.ForEach(md =>
                {
                    if(!_medicineList.Exists(n=>n.PrimaryKey.Equals(md.PrimaryKey)))
                    {
                        _medicineList.Add(new DataGridViewMedicineInfo()
                        {
                            StatusId = md.StatusId,
                            StatusName = medicineStatusDic[md.StatusId],
                            PrimaryKey = md.PrimaryKey,
                            Aliascn = md.Aliascn,
                            Standard = md.Standard,
                            TrocheType = md.TrocheType,
                            Milltitle = md.Milltitle,
                            Stock = md.Stock,
                            ProduceDate = md.ProduceDate,
                            PeriodTo = md.PeriodTo,
                            ProductBatchNo = md.ProductBatchNo,
                            AuthorizedCode = md.AuthorizedCode,
                            Namecn = md.Namecn,
                            ProductNumber = md.ProductNumber,
                            Weight = md.Weight,
                            ProductBarcode = md.ProductBarcode,
                            Price = md.Price,
                            MaxBuyQuantity = md.MaxBuyQuantity,
                            SendDay = md.SendDay
                        });
                    }
                });
                backgroundWorker1.ReportProgress(PROGRESS_MEDICINE, _medicineList);
            }
            catch (Exception ex)
            {
                backgroundWorker1.ReportProgress(PROGRESS_ERROR, ex);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == PROGRESS_MEDICINE)
            {
                dgvMedicine.DataSource = _medicineList;
                //最后一列，即状态名称列，支持自定义排序
                dgvMedicine.Columns[dgvMedicine.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.Programmatic;
                //调用药房网或者插入数据库有可能有错误返回消息，是否对消息文本框做进一步处理？
                lblMsg.Text = $"共有{_medicineList.Count}个商品";
                //列表头单击事件
                dgvMedicine.ColumnHeaderMouseClick += Dgv_ColumnHeaderMouseClick;
            }
            else if (e.ProgressPercentage == PROGRESS_ERROR)
            {
                //显示错误信息
                lblMsg.Text = (e.UserState as Exception)?.Message;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                lblMsg.Text = e.Error.ToString();
                return;
            }
            if (!e.Cancelled)
                lblMsg.Text = "获取药品信息程序处理完毕!";
            else
                lblMsg.Text = "获取药品信息程序处理终止!";
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if(e.Argument!=null)
            {
                var selectedMedicineList = e.Argument as List<DataGridViewMedicineInfo>;
                //分批调用后台接口上传药房网
                foreach (var item in selectedMedicineList)
                {
                    try
                    {
                        var entity = new MedicineInfo() 
                        {
                            //+这种特殊字符在webapi传输的时候不被识别，因此需要转换成对应的16进制值(md5)
                            PrimaryKey = item.PrimaryKey,
                            AuthorizedCode = item.AuthorizedCode,
                            Namecn = item.Namecn,
                            Aliascn = item.Aliascn,
                            TrocheType = item.TrocheType,
                            Standard = item.Standard,
                            Milltitle = item.Milltitle,
                            ProductNumber = item.ProductNumber,
                            Weight = item.Weight,
                            ProductBarcode = item.ProductBarcode,
                            Price = item.Price,
                            Stock = item.Stock,
                            MaxBuyQuantity = item.MaxBuyQuantity,
                            SendDay = item.SendDay,
                            //该操作执行上架工作
                            StatusId = "1",
                            //有效期和生产日期从SAP传来，有可能为空
                            PeriodTo = item.PeriodTo,
                            ProduceDate = item.ProduceDate,
                            ProductBatchNo = item.ProductBatchNo
                        };
                        //就报告成功，处理完，再加载一遍数据，刷新dgv
                        string msg;
                        _medicineService.MedicineUpdate(0, entity, out msg);
                        if (msg.Equals("ok"))
                        {
                            backgroundWorker2.ReportProgress(PROGRESS_MEDICINE, "success");
                        }
                        else
                        {
                            backgroundWorker2.ReportProgress(PROGRESS_ERROR, msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        backgroundWorker2.ReportProgress(PROGRESS_ERROR, ex);
                    }
                }
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //哪些成功，哪些失败？失败是否要记录失败原因，方便下次继续回传？
            if(e.ProgressPercentage== PROGRESS_MEDICINE)
            {
                //是否显示进度条？
            }
            else
            {
                //失败
                lblMsg.Text = e.UserState.GetType() == typeof(Exception) ? (e.UserState as Exception)?.Message : e.UserState as String;
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //第一次只是预加载数据集合到dgv，再次点击搜索按钮，还是要查询一次数据库
            var primaryKey = string.IsNullOrEmpty(txtPrimaryKey.Text.Trim()) ? "" : txtPrimaryKey.Text.Trim();
            //RunWorkerAsync方法支持传参形式的重载
            backgroundWorker1.RunWorkerAsync(primaryKey);
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument != null)
            {
                var selectedMedicineList = e.Argument as List<DataGridViewMedicineInfo>;
                //分批调用后台接口上传药房网
                foreach (var item in selectedMedicineList)
                {
                    try
                    {
                        var entity = new MedicineInfo()
                        {
                            //+这种特殊字符在webapi传输的时候不被识别，因此需要转换成对应的16进制值(md5)
                            PrimaryKey = item.PrimaryKey,
                            AuthorizedCode = item.AuthorizedCode,
                            Namecn = item.Namecn,
                            Aliascn = item.Aliascn,
                            TrocheType = item.TrocheType,
                            Standard = item.Standard,
                            Milltitle = item.Milltitle,
                            ProductNumber = item.ProductNumber,
                            Weight = item.Weight,
                            ProductBarcode = item.ProductBarcode,
                            Price = item.Price,
                            Stock = item.Stock,
                            MaxBuyQuantity = item.MaxBuyQuantity,
                            SendDay = item.SendDay,
                            //该操作执行下架工作
                            StatusId = "-999",
                            //有效期和生产日期从SAP传来，有可能为空
                            PeriodTo = item.PeriodTo,
                            ProduceDate = item.ProduceDate,
                            ProductBatchNo = item.ProductBatchNo
                        };
                        //就报告成功，处理完，再加载一遍数据，刷新dgv
                        string msg;
                        _medicineService.MedicineUpdate(1, entity, out msg);
                        if (msg.Equals("ok"))
                        {
                            backgroundWorker3.ReportProgress(PROGRESS_MEDICINE, "success");
                        }
                        else
                        {
                            backgroundWorker3.ReportProgress(PROGRESS_ERROR, msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        backgroundWorker3.ReportProgress(PROGRESS_ERROR, ex);
                    }
                }
            }
        }

        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //哪些成功，哪些失败？失败是否要记录失败原因，方便下次继续回传？
            if (e.ProgressPercentage == PROGRESS_MEDICINE)
            {
                //是否显示进度条？
            }
            else
            {
                //失败
                lblMsg.Text = e.UserState.GetType() == typeof(Exception) ? (e.UserState as Exception)?.Message : e.UserState as String;
            }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //第一次只是预加载数据集合到dgv，再次点击搜索按钮，还是要查询一次数据库
            var primaryKey = string.IsNullOrEmpty(txtPrimaryKey.Text.Trim()) ? "" : txtPrimaryKey.Text.Trim();
            //RunWorkerAsync方法支持传参形式的重载
            backgroundWorker1.RunWorkerAsync(primaryKey);
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument != null)
            {
                var selectedMedicineList = e.Argument as List<DataGridViewMedicineInfo>;
                //分批调用后台接口上传药房网
                foreach (var item in selectedMedicineList)
                {
                    try
                    {
                        var entity = new MedicineInfo()
                        {
                            //+这种特殊字符在webapi传输的时候不被识别，因此需要转换成对应的16进制值(md5)
                            PrimaryKey = item.PrimaryKey
                        };
                        //就报告成功，处理完，再加载一遍数据，刷新dgv
                        string msg;
                        _medicineService.MedicineUpdate(2, entity, out msg);
                        if (msg.Equals("ok"))
                        {
                            backgroundWorker4.ReportProgress(PROGRESS_MEDICINE, "success");
                        }
                        else
                        {
                            backgroundWorker4.ReportProgress(PROGRESS_ERROR, msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        backgroundWorker4.ReportProgress(PROGRESS_ERROR, ex);
                    }
                }
            }
        }

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
                lblMsg.Text = e.UserState.GetType() == typeof(Exception) ? (e.UserState as Exception)?.Message : e.UserState as String;
            }
        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //第一次只是预加载数据集合到dgv，再次点击搜索按钮，还是要查询一次数据库
            var primaryKey = string.IsNullOrEmpty(txtPrimaryKey.Text.Trim()) ? "" : txtPrimaryKey.Text.Trim();
            //RunWorkerAsync方法支持传参形式的重载
            backgroundWorker1.RunWorkerAsync(primaryKey);
        }

        /// <summary>
        /// 点击列头部支持排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //选择列
            if(e.ColumnIndex==0)
            {
                if(_medicineList.Count>0)
                {
                    foreach (var item in _medicineList)
                    {
                        item.IsChecked = item.IsChecked ? false : true;
                    }
                    dgvMedicine.Refresh();
                    //切换一行
                    dgvMedicine.CurrentCell = dgvMedicine.Rows[dgvMedicine.CurrentRow.Index == _medicineList.Count - 1 ? 0 : dgvMedicine.CurrentRow.Index + 1].Cells["PrimaryKey"];
                }
            }
            //状态列
            if (e.ColumnIndex == 1)
            {
                //并且单击的是最后一列 
                if (_medicineList.Count > 0)
                {
                    var orderByList = !statusAsc ? _medicineList.OrderBy(n => n.StatusId).ToList()
                        : _medicineList.OrderByDescending(n => n.StatusId).ToList();
                    _medicineList = orderByList;
                    statusAsc = !statusAsc;
                    dgvMedicine.DataSource = _medicineList;
                }
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //第一次只是预加载数据集合到dgv，再次点击搜索按钮，还是要查询一次数据库
            var primaryKey = string.IsNullOrEmpty(txtPrimaryKey.Text.Trim()) ? "" : txtPrimaryKey.Text.Trim();
            //RunWorkerAsync方法支持传参形式的重载
            backgroundWorker1.RunWorkerAsync(primaryKey);
        }

        /// <summary>
        /// 商品上架
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            //支持单个上架和批量上架
            var selectedMedicineList = _medicineList.Where(n => n.IsChecked).ToList();
            if (selectedMedicineList.Count == 0)
            {
                MessageBox.Show("请选择商品！");
                return;
            }
            //已上架的不可被勾选
            if (selectedMedicineList.Exists(n => n.StatusId.Equals("1")))
            {
                MessageBox.Show($"商品{selectedMedicineList.Where(n => n.StatusId.Equals("1")).FirstOrDefault().PrimaryKey}已上架，不可再次选择上架！");
                return;
            }
            if(MessageBox.Show("是否确认上架？", "提示", MessageBoxButtons.OKCancel)==DialogResult.OK)
            {
                //后台业务通过backgroundwork来处理
                backgroundWorker2.RunWorkerAsync(selectedMedicineList);
            }
        }

        /// <summary>
        /// 商品下架
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            var selectedMedicineList = _medicineList.Where(n => n.IsChecked).ToList();
            if (selectedMedicineList.Count == 0)
            {
                MessageBox.Show("请选择商品！");
                return;
            }
            //已下架的不可被勾选
            if (selectedMedicineList.Exists(n => n.StatusId.Equals("-999")))
            {
                MessageBox.Show($"商品{selectedMedicineList.Where(n => n.StatusId.Equals("-999")).FirstOrDefault().PrimaryKey}已下架，不可再次选择下架！");
                return;
            }
            if (MessageBox.Show("是否确认下架？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //后台业务通过backgroundwork来处理
                backgroundWorker3.RunWorkerAsync(selectedMedicineList);
            }
        }

        /// <summary>
        /// 商品删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selectedMedicineList = _medicineList.Where(n => n.IsChecked).ToList();
            if (selectedMedicineList.Count == 0)
            {
                MessageBox.Show("请选择商品！");
                return;
            }
            if (MessageBox.Show("是否确认删除？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //后台业务通过backgroundwork来处理
                backgroundWorker4.RunWorkerAsync(selectedMedicineList);
            }
        }
        
        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyPrice_Click(object sender, EventArgs e)
        {
            var selectedMedicine = _medicineList.Where(n => n.IsChecked).ToList();
            if (selectedMedicine.Count == 0)
            {
                MessageBox.Show("请选择商品！");
                return;
            }
            if (selectedMedicine.Count > 1)
            {
                MessageBox.Show("至多选择一个商品！");
                return;
            }
            FormModifyPrice frmModifyPrice = new FormModifyPrice(selectedMedicine.SingleOrDefault().PrimaryKey, selectedMedicine.SingleOrDefault().Price);
            frmModifyPrice.StartPosition = FormStartPosition.CenterScreen;
            frmModifyPrice.Show();
        }
    }
}

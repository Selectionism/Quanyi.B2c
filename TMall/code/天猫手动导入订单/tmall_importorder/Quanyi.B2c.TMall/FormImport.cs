using Quanyi.Entity.DBEntity.Medicine;
using Quanyi.Excel;
using Quanyi.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanyi.B2c.Yaofangwang
{
    public partial class FormImport : Form
    {
        private const int PROGRESS_StockProcessing = -1;

        private const int PROGRESS_StockEnd = -2;

        private const int PROGRESS_BEGINWORKING = -4;

        private const int PROGRESS_Import = -3;

        private const int PROGRESS_Export = -5;

        private const int PROGRESS_ERROR = -9;

        private ExcelUtils _excelUtils;

        //private int iiRowCount = 0;

        //private int iiColCount;

        private List<int> success_rows = new List<int>();

        private List<int> error_rows = new List<int>();

        private int rows;

        private B2cMedicine _medicineService;

        private SapWebApi _sapWebApi;

        private DataTable dt;

        public FormImport()
        {
            InitializeComponent();
            //不允许该dgv自动增加一行
            dgvUploadExcel.AllowUserToAddRows = false;
            _excelUtils = new ExcelUtils();
            _medicineService = new B2cMedicine();
            _sapWebApi = new SapWebApi();
        }

        //private void ShowForm()
        //{
        //    if (WindowState == FormWindowState.Minimized)
        //    {
        //        WindowState = FormWindowState.Normal;
        //    }
        //    Show();
        //    BringToFront();
        //}

        //protected override void OnShown(EventArgs e)
        //{
        //    base.OnShown(e);
        //    backgroundWorker1.RunWorkerAsync();
        //}

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
        }

        /// <summary>
        /// 用于承载异步操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //提示正在导入Excel
                backgroundWorker1.ReportProgress(PROGRESS_BEGINWORKING, "正在导入Excel数据");
                dt = _excelUtils.ExcelToTable(txtFileName.Text);
                backgroundWorker1.ReportProgress(PROGRESS_Import, dt);
            }
            catch (Exception ex)
            {
                backgroundWorker1.ReportProgress(PROGRESS_ERROR, ex);
            }
            ////openFileDialog打开的Excel文件，数据量有可能很大，不影响UI体验的话，放到后台线程DoWork中处理
            //string msg;
            //int iRowCount;
            //int iColCount;
            ////为了显示化地能够显示导入进度，我们应该实时获取当前已经操作到Excel文件的多少了
            //dt = _excelUtils.CreateDataTable(txtFileName.Text, 1, 
            //    out msg, out iRowCount, out iColCount, out iiColCount);
            //if(msg.Equals("OK"))
            //{
            //    //第一次报告进度条最大值
            //    backgroundWorker1.ReportProgress(iRowCount, "working");
            //    for (int iRow = 2; iRow <= iRowCount; iRow++)
            //    {
            //        _excelUtils.AddNewRow(ref msg, iRow, iColCount, ref iiRowCount, iRowCount, dt);
            //        //有一行读取失败，就退出循环
            //        if(!msg.Equals("OK"))
            //        {
            //            break;
            //        }
            //        backgroundWorker1.ReportProgress(iRow, dt);
            //    }
            //}
        }

        /// <summary>
        /// 报告进度事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblProgress.Visible = true;
            //开始导入
            if (e.ProgressPercentage == PROGRESS_BEGINWORKING)
            {
                lblProgress.Text = "正在导入Excel数据...";
            }
            else if (e.ProgressPercentage== PROGRESS_Import)
            {
                //导入完成
            }
            else
            {
                lblProgress.Text = "导入失败：" + (e.UserState as Exception)?.Message;
            }
            //progressBar1.Visible = true;
            //Type working = typeof(String);
            //Type list = typeof(DataTable);
            //Type userState = e.UserState.GetType();
            //if (working.Equals(userState))
            //{
            //    progressBar1.Maximum = e.ProgressPercentage;
            //}
            //if (list.Equals(userState))
            //{
            //    if (e.ProgressPercentage == progressBar1.Maximum)
            //    {
            //        progressBar1.Value = e.ProgressPercentage;
            //    }
            //    else
            //    {
            //        progressBar1.Value = e.ProgressPercentage - 2;
            //    }
            //    lblProgress.Text = string.Concat("处理进度：", 
            //        progressBar1.Value.ToString(), "/", progressBar1.Maximum.ToString());
            //}
        }

        /// <summary>
        /// 异步操作完成或取消时执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(dt == null)
            {
                return;
            }
            //do work完成，默认只加载Excel文件前10条
            var previewDt = dt.AsEnumerable().Take(10).CopyToDataTable<DataRow>();
            dgvUploadExcel.DataSource = previewDt;
            btnUpload.Enabled = true;
            btnSelectFile.Enabled = true;
            lblProgress.Visible = false;
            //progressBar1.Visible = false;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString());
                return;
            }
            if (!e.Cancelled)
            {
                //MessageBox.Show($"处理完毕！总共过滤掉{iiRowCount}空行，{iiColCount}空列");
                MessageBox.Show($"处理完毕！");
            }
            else
            {
                //MessageBox.Show($"处理终止！总共过滤掉{iiRowCount}空行，{iiColCount}空列");
                MessageBox.Show($"处理终止！");
            }
            //iiRowCount = 0;
            //iiColCount = 0;
        }

        /// <summary>
        /// 用于承载异步操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //SAP读库存有可能读取很长时间，后面考虑loading效果
                backgroundWorker2.ReportProgress(PROGRESS_StockProcessing, DateTime.Now);
                string storage = ConfigurationManager.AppSettings["Storage"];
                var currList = new DbTableConvertor<MedicineInfo>().ConvertToList(dt, 1);
                var newStockList = storage.Equals("JY02") ? _medicineService.GetPosAvailStockInfo(currList) : _sapWebApi.GetAvailableStockInfo(currList);
                backgroundWorker2.ReportProgress(PROGRESS_StockEnd, DateTime.Now);
                if (newStockList.Code.Equals("S") && newStockList.Items != null && newStockList.Items.Count > 0)
                {
                    //商品资料表已有记录集合
                    var medicineList = _medicineService.GetMyMedicineList(dt);
                    rows = dt.Rows.Count;
                    backgroundWorker2.ReportProgress(rows, "working");
                    for (int i = 0; i <= rows; i++)
                    {
                        if (backgroundWorker2.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        //调用后台接口，逐个上传，即采用单个上传。删除datatable表也在后台
                        if (i == rows)
                        {
                            //是否应该提示progressbar，成功处理多少个，其中回传失败多少个？
                            backgroundWorker2.ReportProgress(i, "foreach");
                            break;
                        }
                        //不要一条条删，等遍历完再做处理
                        bool result = _medicineService.MedicineImport(dt.Rows[i], medicineList, newStockList);
                        if (result)
                        {
                            //哪行成功的做标注
                            success_rows.Add(i);
                        }
                        else
                        {
                            //哪行失败的也做标注
                            error_rows.Add(i);
                        }
                        //是否应该提示progressbar，成功处理多少个，其中回传失败多少个？
                        backgroundWorker2.ReportProgress(i, "foreach");
                        Thread.Sleep(2 * 1000);
                    }
                }
                else
                {
                    //后台报错直接throw exception
                    backgroundWorker2.ReportProgress(PROGRESS_ERROR, newStockList.Message);
                }
            }
            catch (Exception ex)
            {
                //后台报错直接throw exception
                backgroundWorker2.ReportProgress(PROGRESS_ERROR, ex);
            }
        }

        /// <summary>
        /// 报告进度事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int code = e.ProgressPercentage;
            DateTime beginTime;
            DateTime endTime;
            //第一次读取SAP库存
            if (code == PROGRESS_StockProcessing)
            {
                lblProgress.Visible = true;
                beginTime = DateTime.Parse(e.UserState.ToString());
                lblProgress.Text = string.Concat("正在读取库存，请等待。开始时间：", beginTime);
            }
            if (code == PROGRESS_StockEnd)
            {
                endTime = DateTime.Parse(e.UserState.ToString());
                lblProgress.Text = string.Concat("库存读取结束。结束时间：", endTime);
            }
            if (code >= 0)
            {
                string txt = e.UserState as String;
                //第一次开启工作
                if (txt.Equals("working"))
                {
                    progressBar1.Visible = true;
                    progressBar1.Maximum = e.ProgressPercentage;
                }
                else
                {
                    progressBar1.Value = e.ProgressPercentage;
                    lblProgress.Text = string.Concat("处理进度：",
    progressBar1.Value.ToString(), "/", progressBar1.Maximum.ToString());
                }
            }
            //报错
            if(code==-9)
            {
                MessageBox.Show(e.UserState.ToString());
            }
        }

        /// <summary>
        /// 异步操作完成或取消时执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //必须刷新dgv
            dgvUploadExcel.DataSource = dt;
            dgvUploadExcel.Refresh();
            //如果都上传成功，则上传按钮不可编辑，取消也不可编辑
            if(success_rows.Count==dt.Rows.Count)
            {
                //可以重新选择文件
                btnSelectFile.Enabled = true;
                btnUpload.Enabled = false;
                btnCancel.Enabled = false;
                //清空文件内容框
                txtFileName.Text = "";
                //重置dt
                dt = null;
                //手动刷新
                dgvUploadExcel.DataSource = dt;
                dgvUploadExcel.Refresh();
            }
            //无论如何，完成上传进度条都消失
            lblProgress.Visible = false;
            progressBar1.Visible = false;
            //有失败，重新传
            if (error_rows.Count > 0)
            {
                btnUpload.Enabled = true;
                //有失败，导出失败商品按钮，就可编辑
                btnExportFail.Enabled = true;
            }
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString());
                return;
            }
            if (!e.Cancelled)
            {
                MessageBox.Show($"处理完毕！总共{rows}行，成功{success_rows.Count}行,失败{error_rows.Count}行。");
            }
            else
            {
                MessageBox.Show($"处理终止！总共{rows}行，成功{success_rows.Count}行,失败{error_rows.Count}行。");
            }
            rows = 0;
            success_rows = new List<int>();
            error_rows = new List<int>();
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument != null)
            {
                try
                {
                    //提示正在导入Excel
                    backgroundWorker3.ReportProgress(PROGRESS_BEGINWORKING, "正在导出上传药房网失败的商品数据");
                    //应该对dt进行过滤，只取那些失败的单品
                    var drFail = dt.Select("操作结果<>'成功'");
                    var dtFail = dt.Clone();
                    if (drFail.Length > 0)
                    {
                        foreach (var item in drFail)
                        {
                            dtFail.ImportRow(item);
                        }
                    }
                    ////提供给用户选择保存文件的位置吧
                    //if(string.IsNullOrEmpty(ShowSaveFileDialog()))
                    //{
                    //    MessageBox.Show("请选择文件保存的位置");
                    //    return;
                    //}
                    _excelUtils.TableToExcel(dtFail, e.Argument as String);
                    backgroundWorker3.ReportProgress(PROGRESS_Export, dt);
                }
                catch (Exception ex)
                {
                    backgroundWorker1.ReportProgress(PROGRESS_ERROR, ex);
                }
            }
        }

        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblProgress.Visible = true;
            //开始导入
            if (e.ProgressPercentage == PROGRESS_BEGINWORKING)
            {
                lblProgress.Text = "正在导出上传药房网失败的商品数据...";
            }
            else if (e.ProgressPercentage == PROGRESS_Export)
            {
                //导入完成
            }
            else
            {
                lblProgress.Text = "导出商品数据失败：" + (e.UserState as Exception)?.Message;
            }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnExportFail.Enabled = true;
            lblProgress.Visible = false;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString());
                return;
            }
            if (!e.Cancelled)
            {
                MessageBox.Show($"处理完毕！");
            }
            else
            {
                MessageBox.Show($"处理终止！");
            }
        }

        private string ShowSaveFileDialog()
        {
            var sfd = new SaveFileDialog();
            //设置文件类型
            sfd.Filter = "上传失败单品数据（*.xls）|*.xls|上传失败单品数据（*.xlsx）|*.xlsx";
            //设置默认文件类型显示顺序
            sfd.FilterIndex = 1;
            //保存对话框是否记忆上次打开的目录
            sfd.RestoreDirectory = true;
            sfd.FileName = "YourFileName";
            if(sfd.ShowDialog()==DialogResult.OK)
            {
                //获得文件路径
                string localFilePath = sfd.FileName.ToString();
                //string fileNameExt= localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1); //获取文件名，不带路径
                return localFilePath;
            }
            return string.Empty;
        }

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

        /// <summary>
        /// 选择文件并解析Excel数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            //预处理：只支持Excel这种文件格式
            openFileDialog1.Filter = "(*.xls,*.xlsx)|*.xls;*.xlsx";
            openFileDialog1.Title = "请选择要上传的商品资料文件";
            openFileDialog1.Multiselect = false;
            openFileDialog1.FilterIndex = 0;
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //btnSelectFile.Enabled = false;
                if (string.IsNullOrEmpty(openFileDialog1.FileName))
                {
                    MessageBox.Show("请选择一个Excel文件！");
                    return;
                }
                //文本框显示文件名
                txtFileName.Text = openFileDialog1.FileName;
                //从Excel获取文件，是一个大量的工作，放到backgroundworker里面处理
                backgroundWorker1.RunWorkerAsync();
            }
        }

        /// <summary>
        /// 点击上传药房网按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            //点击完必须屏蔽掉该按钮，不可再次点击，必须等到当前这批品种全部传完，方可再次点击
            btnSelectFile.Enabled = false;
            btnUpload.Enabled = false;
            //点击上传按钮，取消上传开始工作
            btnCancel.Enabled = true;
            //上传药房网，对dgv已加载数据通过backgroundworker，一个个上传药房网和数据库
            backgroundWorker2.RunWorkerAsync();
        }

        /// <summary>
        /// 点击取消上传，就是终止当前backgroundworker2工作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //只有当dt还有数据，取消才有意义
            if(dt != null && dt.Rows.Count > 0)
            {
                backgroundWorker2.CancelAsync();
            }
        }

        /// <summary>
        /// 导出所有上传药房网失败的单品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportFail_Click(object sender, EventArgs e)
        {
            var fileName = ShowSaveFileDialog();
            //提供给用户选择保存文件的位置吧
            if (string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show("请选择文件保存的位置");
                return;
            }
            //点击完必须屏蔽掉该按钮，不可再次点击，必须等到当前失败品种全部导出，方可再次点击
            btnExportFail.Enabled = true;
            //上传药房网，对dgv已加载数据通过backgroundworker，一个个上传药房网和数据库
            backgroundWorker3.RunWorkerAsync(fileName);
        }

        /// <summary>
        /// 鼠标单击该超链接标签控件之后激发该事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            string fileTemplate = Application.StartupPath + "\\Resources\\MedicineTemplates\\商品资料导入模板.xlsx";
            Process.Start(fileTemplate);
        }
    }
}

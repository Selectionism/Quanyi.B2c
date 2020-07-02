using Quanyi.Excel;
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
    public partial class FrmImportOrder : Form
    {
        private const int PROGRESS_ORDERIMPORT = 1;
        private const int PROGRESS_ORDERCOMPLETED = 2;
        private const int PROGRESS_ORDERDETAILIMPORT = 3;
        private const int PROGRESS_ORDERDETAILCOMPLETED = 4;
        private const int PROGRESS_ORDERINFOUPLOAD = 5;
        private const int PROGRESS_ORDERINFOCOMPLETED = 6;
        private const int PROGRESS_ERROR = -1;
        private DataTable dtOrder;
        private DataTable dtOrderDetail;
        private ExcelUtils _excelUtils;
        private B2cPuller _puller;

        public FrmImportOrder()
        {
            InitializeComponent();
            _excelUtils = new ExcelUtils();
            _puller = new B2cPuller();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (!backgroundWorker1.CancellationPending)
            {
                backgroundWorker1.CancelAsync();
            }
            if (!backgroundWorker2.CancellationPending)
            {
                backgroundWorker2.CancelAsync();
            }
            if (!backgroundWorker3.CancellationPending)
            {
                backgroundWorker3.CancelAsync();
            }
        }

        /// <summary>
        /// 选择订单文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectOrder_Click(object sender, EventArgs e)
        {
            openFileDialogOrder.Filter = "(*.xls,*.xlsx)|*.xls;*.xlsx";
            openFileDialogOrder.Title = "请选择要上传的订单信息文件";
            openFileDialogOrder.Multiselect = false;
            openFileDialogOrder.FilterIndex = 0;
            if (openFileDialogOrder.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(openFileDialogOrder.FileName))
                {
                    MessageBox.Show("请选择一个Excel文件！");
                    return;
                }
                txtOrderFileName.Text = openFileDialogOrder.FileName;
                backgroundWorker1.RunWorkerAsync(openFileDialogOrder.FileName);
                //btnSelectOrder.Enabled = false;
            }
        }

        /// <summary>
        /// 选择订单明细文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectOrderDetail_Click(object sender, EventArgs e)
        {
            openFileDialogOrderDetail.Filter = "(*.csv)|*.csv";
            openFileDialogOrderDetail.Title = "请选择要上传的订单明细信息文件";
            openFileDialogOrderDetail.Multiselect = false;
            openFileDialogOrderDetail.FilterIndex = 0;
            if (openFileDialogOrderDetail.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(openFileDialogOrderDetail.FileName))
                {
                    MessageBox.Show("请选择一个CSV文件！");
                    return;
                }
                txtOrderDetailFileName.Text = openFileDialogOrderDetail.FileName;
                backgroundWorker2.RunWorkerAsync(openFileDialogOrderDetail.FileName);
                //btnSelectOrderDetail.Enabled = false;
            }
        }

        /// <summary>
        /// 上传订单和订单明细，到数据库订单表和订单明细表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            //btnUpload.Enabled = false;
            btnSelectOrder.Enabled = false;
            btnSelectOrderDetail.Enabled = false;
            backgroundWorker3.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument != null)
            {
                try
                {
                    //提示正在导入Excel
                    backgroundWorker1.ReportProgress(PROGRESS_ORDERIMPORT, "开始导入订单Excel数据");
                    dtOrder = _excelUtils.ExcelToTable(e.Argument as String);
                    backgroundWorker1.ReportProgress(PROGRESS_ORDERCOMPLETED, dtOrder);
                }
                catch (Exception ex)
                {
                    backgroundWorker1.ReportProgress(PROGRESS_ERROR, ex);
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblOrder.Visible = true;
            //开始导入
            if (e.ProgressPercentage == PROGRESS_ORDERIMPORT)
            {
                lblOrder.Text = e.UserState as String;
            }
            else if (e.ProgressPercentage == PROGRESS_ORDERCOMPLETED)
            {
                //导入完成
            }
            else
            {
                lblOrder.Text = "订单Excel数据导入失败：" + (e.UserState as Exception)?.Message;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (dtOrder == null)
            {
                return;
            }
            //do work完成，默认只加载Excel文件前10条
            var previewDt = dtOrder.AsEnumerable().Take(10).CopyToDataTable<DataRow>();
            dgvOrder.DataSource = previewDt;
            //btnSelectOrder.Enabled = true;
            btnSelectOrderDetail.Enabled = true;
            //btnUpload.Enabled = true;
            lblOrder.Visible = false;
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

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument != null)
            {
                try
                {
                    //提示正在导入Excel
                    backgroundWorker2.ReportProgress(PROGRESS_ORDERDETAILIMPORT, "开始导入订单明细CSV数据");
                    dtOrderDetail = _excelUtils.ReadCsv(e.Argument as String);
                    backgroundWorker2.ReportProgress(PROGRESS_ORDERDETAILCOMPLETED, dtOrderDetail);
                }
                catch (Exception ex)
                {
                    backgroundWorker2.ReportProgress(PROGRESS_ERROR, ex);
                }
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblOrderDetail.Visible = true;
            //开始导入
            if (e.ProgressPercentage == PROGRESS_ORDERDETAILIMPORT)
            {
                lblOrderDetail.Text = e.UserState as String;
            }
            else if (e.ProgressPercentage == PROGRESS_ORDERDETAILCOMPLETED)
            {
                //导入完成
            }
            else
            {
                lblOrderDetail.Text = "订单明细CSV数据导入失败：" + (e.UserState as Exception)?.Message;
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (dtOrderDetail == null)
            {
                return;
            }
            //do work完成，默认只加载Excel文件前10条
            var previewDt = dtOrderDetail.AsEnumerable().Take(10).CopyToDataTable<DataRow>();
            dgvOrderDetail.DataSource = previewDt;
            //btnSelectOrder.Enabled = true;
            //btnSelectOrderDetail.Enabled = true;
            btnUpload.Enabled = true;
            lblOrderDetail.Visible = false;
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

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                backgroundWorker3.ReportProgress(PROGRESS_ORDERINFOUPLOAD, "开始上传订单信息");
                //调用后台接口，将订单表、订单明细表全部整合到一起，经过筛选、归纳，一起写入数据库
                _puller.UploadBillExcelData(dtOrder, dtOrderDetail);
                backgroundWorker3.ReportProgress(PROGRESS_ORDERINFOCOMPLETED, "订单信息上传成功");
            }
            catch (Exception ex)
            {
                backgroundWorker3.ReportProgress(PROGRESS_ERROR, ex);
            }
        }

        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblOrderInfoUpload.Visible = true;
            //第一次读取SAP库存
            if (e.ProgressPercentage == PROGRESS_ORDERINFOUPLOAD)
            {
                lblOrderInfoUpload.Text = e.UserState as String;
            }
            if (e.ProgressPercentage == PROGRESS_ORDERINFOCOMPLETED)
            {
                lblOrderInfoUpload.Text = e.UserState as String;
            }
            //报错
            if (e.ProgressPercentage == PROGRESS_ERROR)
            {
                MessageBox.Show(e.UserState.ToString());
            }

        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dtOrder = null;
            dtOrderDetail = null;
            //必须刷新dgv
            dgvOrder.DataSource = dtOrder;
            dgvOrder.Refresh();
            dgvOrderDetail.DataSource = dtOrderDetail;
            dgvOrderDetail.Refresh();
            lblOrderInfoUpload.Visible = false;
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
            btnUpload.Enabled = false;
            btnSelectOrder.Enabled = true;
        }
    }
}

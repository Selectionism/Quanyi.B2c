namespace Quanyi.B2c.Yaofangwang
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cboBillStatus = new System.Windows.Forms.ToolStripComboBox();
            this.btnImportOrder = new System.Windows.Forms.ToolStripButton();
            this.btnSelectAll = new System.Windows.Forms.ToolStripButton();
            this.btnCommit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.tslMsg = new System.Windows.Forms.ToolStripLabel();
            this.btnModifyMedicine = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvBillInfo = new System.Windows.Forms.DataGridView();
            this.dgvBillProInfo = new System.Windows.Forms.DataGridView();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDisplay = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.cbStockUpdate = new System.Windows.Forms.CheckBox();
            this.cbPeriodUpdate = new System.Windows.Forms.CheckBox();
            this.backgroundWorker4 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker5 = new System.ComponentModel.BackgroundWorker();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillProInfo)).BeginInit();
            this.notifyMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cboBillStatus,
            this.btnImportOrder,
            this.btnSelectAll,
            this.btnCommit,
            this.btnDelete,
            this.btnImport,
            this.toolStripSeparator3,
            this.toolStripSeparator1,
            this.toolStripButton4,
            this.toolStripSeparator2,
            this.toolStripButton3,
            this.tslMsg,
            this.btnModifyMedicine});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1580, 39);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(84, 36);
            this.toolStripLabel1.Text = "订单状态：";
            // 
            // cboBillStatus
            // 
            this.cboBillStatus.Name = "cboBillStatus";
            this.cboBillStatus.Size = new System.Drawing.Size(121, 39);
            this.cboBillStatus.SelectedIndexChanged += new System.EventHandler(this.cboBillStatus_SelectedIndexChanged);
            // 
            // btnImportOrder
            // 
            this.btnImportOrder.Image = ((System.Drawing.Image)(resources.GetObject("btnImportOrder.Image")));
            this.btnImportOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImportOrder.Name = "btnImportOrder";
            this.btnImportOrder.Size = new System.Drawing.Size(124, 36);
            this.btnImportOrder.Text = "导入订单&IM";
            this.btnImportOrder.Visible = false;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectAll.Image")));
            this.btnSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(84, 36);
            this.btnSelectAll.Text = "全选&S";
            // 
            // btnCommit
            // 
            this.btnCommit.Image = ((System.Drawing.Image)(resources.GetObject("btnCommit.Image")));
            this.btnCommit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(85, 36);
            this.btnCommit.Text = "确认&C";
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(86, 36);
            this.btnDelete.Text = "删除&D";
            // 
            // btnImport
            // 
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(109, 36);
            this.btnImport.Text = "导入商品&I";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(75, 36);
            this.toolStripButton4.Text = "日志";
            this.toolStripButton4.Visible = false;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(83, 36);
            this.toolStripButton3.Text = "关闭&E";
            this.toolStripButton3.Visible = false;
            // 
            // tslMsg
            // 
            this.tslMsg.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tslMsg.Name = "tslMsg";
            this.tslMsg.Size = new System.Drawing.Size(63, 36);
            this.tslMsg.Text = "共有0单";
            // 
            // btnModifyMedicine
            // 
            this.btnModifyMedicine.Image = ((System.Drawing.Image)(resources.GetObject("btnModifyMedicine.Image")));
            this.btnModifyMedicine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnModifyMedicine.Name = "btnModifyMedicine";
            this.btnModifyMedicine.Size = new System.Drawing.Size(150, 36);
            this.btnModifyMedicine.Text = "修改商品信息&M";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 39);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvBillInfo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvBillProInfo);
            this.splitContainer1.Size = new System.Drawing.Size(1580, 523);
            this.splitContainer1.SplitterDistance = 933;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // dgvBillInfo
            // 
            this.dgvBillInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBillInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBillInfo.Location = new System.Drawing.Point(0, 0);
            this.dgvBillInfo.Margin = new System.Windows.Forms.Padding(4);
            this.dgvBillInfo.Name = "dgvBillInfo";
            this.dgvBillInfo.RowHeadersWidth = 51;
            this.dgvBillInfo.RowTemplate.Height = 23;
            this.dgvBillInfo.Size = new System.Drawing.Size(933, 523);
            this.dgvBillInfo.TabIndex = 0;
            this.dgvBillInfo.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvBillInfo_DataError);
            // 
            // dgvBillProInfo
            // 
            this.dgvBillProInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBillProInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBillProInfo.Location = new System.Drawing.Point(0, 0);
            this.dgvBillProInfo.Margin = new System.Windows.Forms.Padding(4);
            this.dgvBillProInfo.Name = "dgvBillProInfo";
            this.dgvBillProInfo.RowHeadersWidth = 51;
            this.dgvBillProInfo.Size = new System.Drawing.Size(642, 523);
            this.dgvBillProInfo.TabIndex = 0;
            this.dgvBillProInfo.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvBillProInfo_DataError);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.notifyMenu;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "天猫手动导入订单正在运行...";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // notifyMenu
            // 
            this.notifyMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.notifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDisplay,
            this.tsmiExit});
            this.notifyMenu.Name = "notifyMenu";
            this.notifyMenu.Size = new System.Drawing.Size(109, 52);
            // 
            // tsmiDisplay
            // 
            this.tsmiDisplay.Name = "tsmiDisplay";
            this.tsmiDisplay.Size = new System.Drawing.Size(108, 24);
            this.tsmiDisplay.Text = "显示";
            this.tsmiDisplay.Click += new System.EventHandler(this.tsmiDisplay_Click);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(108, 24);
            this.tsmiExit.Text = "退出";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.WorkerReportsProgress = true;
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // backgroundWorker3
            // 
            this.backgroundWorker3.WorkerReportsProgress = true;
            this.backgroundWorker3.WorkerSupportsCancellation = true;
            this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker3_DoWork);
            this.backgroundWorker3.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker3_ProgressChanged);
            this.backgroundWorker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker3_RunWorkerCompleted);
            // 
            // cbStockUpdate
            // 
            this.cbStockUpdate.AutoSize = true;
            this.cbStockUpdate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbStockUpdate.Location = new System.Drawing.Point(1097, 11);
            this.cbStockUpdate.Name = "cbStockUpdate";
            this.cbStockUpdate.Size = new System.Drawing.Size(125, 19);
            this.cbStockUpdate.TabIndex = 3;
            this.cbStockUpdate.Text = "自动更新库存";
            this.cbStockUpdate.UseVisualStyleBackColor = true;
            // 
            // cbPeriodUpdate
            // 
            this.cbPeriodUpdate.AutoSize = true;
            this.cbPeriodUpdate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbPeriodUpdate.Location = new System.Drawing.Point(1242, 11);
            this.cbPeriodUpdate.Name = "cbPeriodUpdate";
            this.cbPeriodUpdate.Size = new System.Drawing.Size(152, 19);
            this.cbPeriodUpdate.TabIndex = 3;
            this.cbPeriodUpdate.Text = "自动更新SAP效期";
            this.cbPeriodUpdate.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker4
            // 
            this.backgroundWorker4.WorkerReportsProgress = true;
            this.backgroundWorker4.WorkerSupportsCancellation = true;
            this.backgroundWorker4.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker4_DoWork);
            this.backgroundWorker4.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker4_ProgressChanged);
            this.backgroundWorker4.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker4_RunWorkerCompleted);
            // 
            // backgroundWorker5
            // 
            this.backgroundWorker5.WorkerReportsProgress = true;
            this.backgroundWorker5.WorkerSupportsCancellation = true;
            this.backgroundWorker5.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker5_DoWork);
            this.backgroundWorker5.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker5_ProgressChanged);
            this.backgroundWorker5.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker5_RunWorkerCompleted);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1580, 562);
            this.Controls.Add(this.cbPeriodUpdate);
            this.Controls.Add(this.cbStockUpdate);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "天猫手动导入订单程序";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillProInfo)).EndInit();
            this.notifyMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnCommit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvBillInfo;
        private System.Windows.Forms.DataGridView dgvBillProInfo;
        private System.Windows.Forms.ToolStripLabel tslMsg;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.ToolStripButton btnSelectAll;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.Windows.Forms.ToolStripButton btnModifyMedicine;
        private System.Windows.Forms.CheckBox cbStockUpdate;
        private System.Windows.Forms.CheckBox cbPeriodUpdate;
        private System.ComponentModel.BackgroundWorker backgroundWorker4;
        private System.Windows.Forms.ContextMenuStrip notifyMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplay;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripButton btnImport;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cboBillStatus;
        private System.Windows.Forms.ToolStripButton btnImportOrder;
        private System.ComponentModel.BackgroundWorker backgroundWorker5;
    }
}


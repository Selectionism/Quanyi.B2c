namespace Quanyi.B2c.Yaofangwang
{
    partial class FormImport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImport));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvUploadExcel = new System.Windows.Forms.DataGridView();
            this.btnExportFail = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUploadExcel)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.btnSelectFile);
            this.panel1.Controls.Add(this.txtFileName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(790, 45);
            this.panel1.TabIndex = 0;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(525, 9);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 25);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "选择";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(134, 9);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(378, 25);
            this.txtFileName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Excel数据文件：";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvUploadExcel);
            this.groupBox1.Controls.Add(this.btnExportFail);
            this.groupBox1.Location = new System.Drawing.Point(12, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(790, 335);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Excel数据预览(第一次加载数据默认只显示前10条)";
            // 
            // dgvUploadExcel
            // 
            this.dgvUploadExcel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUploadExcel.Location = new System.Drawing.Point(6, 63);
            this.dgvUploadExcel.Name = "dgvUploadExcel";
            this.dgvUploadExcel.RowHeadersWidth = 51;
            this.dgvUploadExcel.RowTemplate.Height = 27;
            this.dgvUploadExcel.Size = new System.Drawing.Size(859, 263);
            this.dgvUploadExcel.TabIndex = 0;
            // 
            // btnExportFail
            // 
            this.btnExportFail.Enabled = false;
            this.btnExportFail.Location = new System.Drawing.Point(636, 23);
            this.btnExportFail.Name = "btnExportFail";
            this.btnExportFail.Size = new System.Drawing.Size(134, 30);
            this.btnExportFail.TabIndex = 2;
            this.btnExportFail.Text = "导出失败单品";
            this.btnExportFail.UseVisualStyleBackColor = true;
            this.btnExportFail.Click += new System.EventHandler(this.btnExportFail_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(120, 460);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(82, 15);
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "处理进度：";
            this.lblProgress.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(118, 492);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(579, 23);
            this.progressBar1.TabIndex = 1;
            this.progressBar1.Visible = false;
            // 
            // btnUpload
            // 
            this.btnUpload.Enabled = false;
            this.btnUpload.Location = new System.Drawing.Point(255, 423);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(110, 30);
            this.btnUpload.TabIndex = 2;
            this.btnUpload.Text = "上传药房网";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(453, 423);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消上传";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.WorkerReportsProgress = true;
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // backgroundWorker3
            // 
            this.backgroundWorker3.WorkerReportsProgress = true;
            this.backgroundWorker3.WorkerSupportsCancellation = true;
            this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker3_DoWork);
            this.backgroundWorker3.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker3_ProgressChanged);
            this.backgroundWorker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker3_RunWorkerCompleted);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(647, 14);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(127, 15);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "商品资料导入模板";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // FormImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 541);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImport";
            this.Text = "商品资料的导入";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUploadExcel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox txtFileName;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvUploadExcel;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblProgress;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnExportFail;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}
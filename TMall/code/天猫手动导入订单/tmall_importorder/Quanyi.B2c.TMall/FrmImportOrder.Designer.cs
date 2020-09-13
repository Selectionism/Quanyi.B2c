namespace Quanyi.B2c.Yaofangwang
{
    partial class FrmImportOrder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmImportOrder));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialogOrder = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogOrderDetail = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.btnSelectOrder = new System.Windows.Forms.Button();
            this.txtOrderFileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblOrder = new System.Windows.Forms.Label();
            this.dgvOrder = new System.Windows.Forms.DataGridView();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.btnSelectOrderDetail = new System.Windows.Forms.Button();
            this.txtOrderDetailFileName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblOrderDetail = new System.Windows.Forms.Label();
            this.dgvOrderDetail = new System.Windows.Forms.DataGridView();
            this.lblOrderInfoUpload = new System.Windows.Forms.Label();
            this.btnUpload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderDetail)).BeginInit();
            this.SuspendLayout();
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
            // openFileDialogOrder
            // 
            this.openFileDialogOrder.FileName = "openFileDialog1";
            // 
            // openFileDialogOrderDetail
            // 
            this.openFileDialogOrderDetail.FileName = "openFileDialog2";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblOrderInfoUpload);
            this.splitContainer1.Panel2.Controls.Add(this.btnUpload);
            this.splitContainer1.Size = new System.Drawing.Size(1277, 629);
            this.splitContainer1.SplitterDistance = 530;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Size = new System.Drawing.Size(1277, 530);
            this.splitContainer2.SplitterDistance = 652;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.btnSelectOrder);
            this.splitContainer3.Panel1.Controls.Add(this.txtOrderFileName);
            this.splitContainer3.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer3.Size = new System.Drawing.Size(652, 530);
            this.splitContainer3.SplitterDistance = 59;
            this.splitContainer3.TabIndex = 0;
            // 
            // btnSelectOrder
            // 
            this.btnSelectOrder.BackColor = System.Drawing.Color.PowderBlue;
            this.btnSelectOrder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSelectOrder.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectOrder.Image")));
            this.btnSelectOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSelectOrder.Location = new System.Drawing.Point(529, 14);
            this.btnSelectOrder.Name = "btnSelectOrder";
            this.btnSelectOrder.Size = new System.Drawing.Size(79, 33);
            this.btnSelectOrder.TabIndex = 5;
            this.btnSelectOrder.Text = "  选择";
            this.btnSelectOrder.UseVisualStyleBackColor = false;
            this.btnSelectOrder.Click += new System.EventHandler(this.btnSelectOrder_Click);
            // 
            // txtOrderFileName
            // 
            this.txtOrderFileName.Location = new System.Drawing.Point(188, 18);
            this.txtOrderFileName.Name = "txtOrderFileName";
            this.txtOrderFileName.Size = new System.Drawing.Size(332, 25);
            this.txtOrderFileName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "订单Excel数据文件：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblOrder);
            this.groupBox1.Controls.Add(this.dgvOrder);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(652, 467);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "订单数据预览（默认只显示前10条）";
            // 
            // lblOrder
            // 
            this.lblOrder.AutoSize = true;
            this.lblOrder.Location = new System.Drawing.Point(85, 246);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(112, 15);
            this.lblOrder.TabIndex = 5;
            this.lblOrder.Text = "订单处理结果：";
            this.lblOrder.Visible = false;
            // 
            // dgvOrder
            // 
            this.dgvOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrder.Location = new System.Drawing.Point(3, 21);
            this.dgvOrder.Name = "dgvOrder";
            this.dgvOrder.RowHeadersWidth = 51;
            this.dgvOrder.RowTemplate.Height = 27;
            this.dgvOrder.Size = new System.Drawing.Size(646, 443);
            this.dgvOrder.TabIndex = 1;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.btnSelectOrderDetail);
            this.splitContainer4.Panel1.Controls.Add(this.txtOrderDetailFileName);
            this.splitContainer4.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer4.Size = new System.Drawing.Size(621, 530);
            this.splitContainer4.SplitterDistance = 59;
            this.splitContainer4.TabIndex = 0;
            // 
            // btnSelectOrderDetail
            // 
            this.btnSelectOrderDetail.BackColor = System.Drawing.Color.LightGreen;
            this.btnSelectOrderDetail.Enabled = false;
            this.btnSelectOrderDetail.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSelectOrderDetail.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectOrderDetail.Image")));
            this.btnSelectOrderDetail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSelectOrderDetail.Location = new System.Drawing.Point(513, 13);
            this.btnSelectOrderDetail.Name = "btnSelectOrderDetail";
            this.btnSelectOrderDetail.Size = new System.Drawing.Size(88, 33);
            this.btnSelectOrderDetail.TabIndex = 5;
            this.btnSelectOrderDetail.Text = "选择";
            this.btnSelectOrderDetail.UseVisualStyleBackColor = false;
            this.btnSelectOrderDetail.Click += new System.EventHandler(this.btnSelectOrderDetail_Click);
            // 
            // txtOrderDetailFileName
            // 
            this.txtOrderDetailFileName.Location = new System.Drawing.Point(178, 17);
            this.txtOrderDetailFileName.Name = "txtOrderDetailFileName";
            this.txtOrderDetailFileName.Size = new System.Drawing.Size(329, 25);
            this.txtOrderDetailFileName.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "订单明细CSV数据文件：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblOrderDetail);
            this.groupBox2.Controls.Add(this.dgvOrderDetail);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(621, 467);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "订单明细数据预览（默认只显示前10条）";
            // 
            // lblOrderDetail
            // 
            this.lblOrderDetail.AutoSize = true;
            this.lblOrderDetail.Location = new System.Drawing.Point(68, 246);
            this.lblOrderDetail.Name = "lblOrderDetail";
            this.lblOrderDetail.Size = new System.Drawing.Size(142, 15);
            this.lblOrderDetail.TabIndex = 7;
            this.lblOrderDetail.Text = "订单详情处理结果：";
            this.lblOrderDetail.Visible = false;
            // 
            // dgvOrderDetail
            // 
            this.dgvOrderDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrderDetail.Location = new System.Drawing.Point(3, 21);
            this.dgvOrderDetail.Name = "dgvOrderDetail";
            this.dgvOrderDetail.RowHeadersWidth = 51;
            this.dgvOrderDetail.RowTemplate.Height = 27;
            this.dgvOrderDetail.Size = new System.Drawing.Size(615, 443);
            this.dgvOrderDetail.TabIndex = 1;
            // 
            // lblOrderInfoUpload
            // 
            this.lblOrderInfoUpload.AutoSize = true;
            this.lblOrderInfoUpload.Location = new System.Drawing.Point(400, 17);
            this.lblOrderInfoUpload.Name = "lblOrderInfoUpload";
            this.lblOrderInfoUpload.Size = new System.Drawing.Size(82, 15);
            this.lblOrderInfoUpload.TabIndex = 7;
            this.lblOrderInfoUpload.Text = "上传结果：";
            this.lblOrderInfoUpload.Visible = false;
            // 
            // btnUpload
            // 
            this.btnUpload.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnUpload.Enabled = false;
            this.btnUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnUpload.Image")));
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpload.Location = new System.Drawing.Point(550, 42);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(222, 42);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "上传订单和订单明细";
            this.btnUpload.UseVisualStyleBackColor = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // FrmImportOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1277, 629);
            this.Controls.Add(this.splitContainer1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmImportOrder";
            this.Text = "导入订单";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmImportOrder_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrder)).EndInit();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.Windows.Forms.OpenFileDialog openFileDialogOrder;
        private System.Windows.Forms.OpenFileDialog openFileDialogOrderDetail;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Button btnSelectOrder;
        private System.Windows.Forms.TextBox txtOrderFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectOrderDetail;
        private System.Windows.Forms.TextBox txtOrderDetailFileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvOrder;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvOrderDetail;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.Label lblOrderDetail;
        private System.Windows.Forms.Label lblOrderInfoUpload;
    }
}
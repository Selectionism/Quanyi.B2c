namespace Quanyi.B2c.Yaofangwang
{
    partial class FormModifyMedicine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModifyMedicine));
            this.label1 = new System.Windows.Forms.Label();
            this.txtPrimaryKey = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvMedicine = new System.Windows.Forms.DataGridView();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lblMsg = new System.Windows.Forms.Label();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker4 = new System.ComponentModel.BackgroundWorker();
            this.btnModifyPrice = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicine)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "商品编号：";
            // 
            // txtPrimaryKey
            // 
            this.txtPrimaryKey.BackColor = System.Drawing.SystemColors.Control;
            this.txtPrimaryKey.Location = new System.Drawing.Point(98, 19);
            this.txtPrimaryKey.Name = "txtPrimaryKey";
            this.txtPrimaryKey.Size = new System.Drawing.Size(162, 25);
            this.txtPrimaryKey.TabIndex = 1;
            this.txtPrimaryKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPrimaryKey_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(280, 17);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(92, 30);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvMedicine
            // 
            this.dgvMedicine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMedicine.Location = new System.Drawing.Point(13, 63);
            this.dgvMedicine.Name = "dgvMedicine";
            this.dgvMedicine.RowHeadersWidth = 51;
            this.dgvMedicine.RowTemplate.Height = 27;
            this.dgvMedicine.Size = new System.Drawing.Size(775, 323);
            this.dgvMedicine.TabIndex = 3;
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.Color.Azure;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.Location = new System.Drawing.Point(144, 406);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(88, 32);
            this.btnUp.TabIndex = 4;
            this.btnUp.Text = "上架";
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDown.Location = new System.Drawing.Point(281, 406);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(88, 32);
            this.btnDown.TabIndex = 4;
            this.btnDown.Text = "下架";
            this.btnDown.UseVisualStyleBackColor = false;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Salmon;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Location = new System.Drawing.Point(421, 406);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(88, 32);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(457, 24);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMsg.Size = new System.Drawing.Size(90, 15);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "共有0个商品";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // backgroundWorker4
            // 
            this.backgroundWorker4.WorkerReportsProgress = true;
            this.backgroundWorker4.WorkerSupportsCancellation = true;
            this.backgroundWorker4.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker4_DoWork);
            this.backgroundWorker4.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker4_ProgressChanged);
            this.backgroundWorker4.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker4_RunWorkerCompleted);
            // 
            // btnModifyPrice
            // 
            this.btnModifyPrice.BackColor = System.Drawing.Color.LightGreen;
            this.btnModifyPrice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModifyPrice.Location = new System.Drawing.Point(560, 406);
            this.btnModifyPrice.Name = "btnModifyPrice";
            this.btnModifyPrice.Size = new System.Drawing.Size(88, 32);
            this.btnModifyPrice.TabIndex = 4;
            this.btnModifyPrice.Text = "修改价格";
            this.btnModifyPrice.UseVisualStyleBackColor = false;
            this.btnModifyPrice.Click += new System.EventHandler(this.btnModifyPrice_Click);
            // 
            // FormModifyMedicine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnModifyPrice);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.dgvMedicine);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtPrimaryKey);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormModifyMedicine";
            this.Text = "修改商品信息";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicine)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPrimaryKey;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvMedicine;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnDelete;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lblMsg;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.ComponentModel.BackgroundWorker backgroundWorker4;
        private System.Windows.Forms.Button btnModifyPrice;
    }
}
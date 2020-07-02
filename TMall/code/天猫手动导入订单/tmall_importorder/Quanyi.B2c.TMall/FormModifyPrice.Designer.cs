namespace Quanyi.B2c.Yaofangwang
{
    partial class FormModifyPrice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModifyPrice));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCurrentPrice = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.btnCommit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPrimaryKey = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前价格：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "要修改的价格：";
            // 
            // lblCurrentPrice
            // 
            this.lblCurrentPrice.AutoSize = true;
            this.lblCurrentPrice.Location = new System.Drawing.Point(137, 61);
            this.lblCurrentPrice.Name = "lblCurrentPrice";
            this.lblCurrentPrice.Size = new System.Drawing.Size(46, 15);
            this.lblCurrentPrice.TabIndex = 0;
            this.lblCurrentPrice.Text = "XXX元";
            // 
            // txtPrice
            // 
            this.txtPrice.BackColor = System.Drawing.SystemColors.Control;
            this.txtPrice.Location = new System.Drawing.Point(140, 103);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(100, 25);
            this.txtPrice.TabIndex = 2;
            // 
            // btnCommit
            // 
            this.btnCommit.BackColor = System.Drawing.Color.Wheat;
            this.btnCommit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCommit.Location = new System.Drawing.Point(91, 149);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(92, 37);
            this.btnCommit.TabIndex = 3;
            this.btnCommit.Text = "提交";
            this.btnCommit.UseVisualStyleBackColor = false;
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "当前商品编号：";
            // 
            // lblPrimaryKey
            // 
            this.lblPrimaryKey.AutoSize = true;
            this.lblPrimaryKey.Location = new System.Drawing.Point(137, 19);
            this.lblPrimaryKey.Name = "lblPrimaryKey";
            this.lblPrimaryKey.Size = new System.Drawing.Size(31, 15);
            this.lblPrimaryKey.TabIndex = 0;
            this.lblPrimaryKey.Text = "XXX";
            // 
            // FormModifyPrice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 204);
            this.Controls.Add(this.btnCommit);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblPrimaryKey);
            this.Controls.Add(this.lblCurrentPrice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormModifyPrice";
            this.Text = "修改商品价格";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCurrentPrice;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Button btnCommit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPrimaryKey;
    }
}
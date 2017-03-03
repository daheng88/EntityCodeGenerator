namespace EntityCodeGenerator
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtConnectionStr = new System.Windows.Forms.TextBox();
            this.lblConnectionStr = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconect = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.lblNameSpace = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtConnectionStr
            // 
            this.txtConnectionStr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionStr.Location = new System.Drawing.Point(87, 11);
            this.txtConnectionStr.Name = "txtConnectionStr";
            this.txtConnectionStr.Size = new System.Drawing.Size(533, 21);
            this.txtConnectionStr.TabIndex = 0;
            this.txtConnectionStr.Text = "Server=121.40.206.55;Database=test;UID=sa;PWD=sa;";
            // 
            // lblConnectionStr
            // 
            this.lblConnectionStr.AutoSize = true;
            this.lblConnectionStr.Location = new System.Drawing.Point(13, 17);
            this.lblConnectionStr.Name = "lblConnectionStr";
            this.lblConnectionStr.Size = new System.Drawing.Size(65, 12);
            this.lblConnectionStr.TabIndex = 1;
            this.lblConnectionStr.Text = "连接字符串";
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(478, 45);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(67, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconect
            // 
            this.btnDisconect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisconect.Enabled = false;
            this.btnDisconect.Location = new System.Drawing.Point(551, 45);
            this.btnDisconect.Name = "btnDisconect";
            this.btnDisconect.Size = new System.Drawing.Size(67, 23);
            this.btnDisconect.TabIndex = 2;
            this.btnDisconect.Text = "断开";
            this.btnDisconect.UseVisualStyleBackColor = true;
            this.btnDisconect.Click += new System.EventHandler(this.btnDisconect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtNameSpace);
            this.groupBox1.Controls.Add(this.lblNameSpace);
            this.groupBox1.Controls.Add(this.btnGenerate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 74);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(632, 279);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(64, 42);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(200, 21);
            this.txtNameSpace.TabIndex = 5;
            this.txtNameSpace.Text = "Daheng88.Model";
            // 
            // lblNameSpace
            // 
            this.lblNameSpace.AutoSize = true;
            this.lblNameSpace.Location = new System.Drawing.Point(62, 17);
            this.lblNameSpace.Name = "lblNameSpace";
            this.lblNameSpace.Size = new System.Drawing.Size(53, 12);
            this.lblNameSpace.TabIndex = 4;
            this.lblNameSpace.Text = "命名空间";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(61, 71);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 3;
            this.btnGenerate.Text = "生成实体类";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 353);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDisconect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lblConnectionStr);
            this.Controls.Add(this.txtConnectionStr);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "实体类生成";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtConnectionStr;
        private System.Windows.Forms.Label lblConnectionStr;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconect;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.Label lblNameSpace;
    }
}


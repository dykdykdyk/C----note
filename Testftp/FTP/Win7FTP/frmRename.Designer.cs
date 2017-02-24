namespace Win7FTP
{
    partial class frmRename
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
            this.pnlNonTransparent = new System.Windows.Forms.Panel();
            this.cmdRename = new Microsoft.WindowsAPICodePack.Controls.WindowsForms.CommandLink();
            this.button1 = new System.Windows.Forms.Button();
            this.txtRenameTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlNonTransparent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlNonTransparent
            // 
            this.pnlNonTransparent.Controls.Add(this.cmdRename);
            this.pnlNonTransparent.Controls.Add(this.button1);
            this.pnlNonTransparent.Controls.Add(this.txtRenameTo);
            this.pnlNonTransparent.Controls.Add(this.label2);
            this.pnlNonTransparent.Controls.Add(this.lblFileName);
            this.pnlNonTransparent.Controls.Add(this.lblLocation);
            this.pnlNonTransparent.Controls.Add(this.label1);
            this.pnlNonTransparent.Location = new System.Drawing.Point(12, 11);
            this.pnlNonTransparent.Name = "pnlNonTransparent";
            this.pnlNonTransparent.Size = new System.Drawing.Size(358, 161);
            this.pnlNonTransparent.TabIndex = 0;
            this.pnlNonTransparent.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlNonTransparent_Paint);
            // 
            // cmdRename
            // 
            this.cmdRename.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdRename.Location = new System.Drawing.Point(224, 112);
            this.cmdRename.Name = "cmdRename";
            this.cmdRename.NoteText = "";
            this.cmdRename.Size = new System.Drawing.Size(114, 42);
            this.cmdRename.TabIndex = 8;
            this.cmdRename.Text = "确定";
            this.cmdRename.UseVisualStyleBackColor = true;
            this.cmdRename.Click += new System.EventHandler(this.cmdRename_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 132);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 21);
            this.button1.TabIndex = 6;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtRenameTo
            // 
            this.txtRenameTo.Location = new System.Drawing.Point(21, 88);
            this.txtRenameTo.Name = "txtRenameTo";
            this.txtRenameTo.Size = new System.Drawing.Size(317, 21);
            this.txtRenameTo.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "命名为";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(18, 50);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(53, 12);
            this.lblFileName.TabIndex = 2;
            this.lblFileName.Text = "文件名：";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(18, 35);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(41, 12);
            this.lblLocation.TabIndex = 1;
            this.lblLocation.Text = "地址：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label1.Location = new System.Drawing.Point(17, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件重命名";
            // 
            // frmRename
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 189);
            this.Controls.Add(this.pnlNonTransparent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmRename";
            this.Text = "重命名";
            this.Load += new System.EventHandler(this.frmRename_Load);
            this.pnlNonTransparent.ResumeLayout(false);
            this.pnlNonTransparent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNonTransparent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private Microsoft.WindowsAPICodePack.Controls.WindowsForms.CommandLink cmdRename;
        public System.Windows.Forms.TextBox txtRenameTo;
    }
}
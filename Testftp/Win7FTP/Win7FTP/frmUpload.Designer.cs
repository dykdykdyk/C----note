namespace Win7FTP
{
    partial class frmUpload
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDownloadStatus = new System.Windows.Forms.Label();
            this.lblUploadDirectory = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pnlNonTransparent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlNonTransparent
            // 
            this.pnlNonTransparent.Controls.Add(this.btnCancel);
            this.pnlNonTransparent.Controls.Add(this.lblDownloadStatus);
            this.pnlNonTransparent.Controls.Add(this.lblUploadDirectory);
            this.pnlNonTransparent.Controls.Add(this.lblFileName);
            this.pnlNonTransparent.Controls.Add(this.label2);
            this.pnlNonTransparent.Controls.Add(this.label1);
            this.pnlNonTransparent.Location = new System.Drawing.Point(17, 11);
            this.pnlNonTransparent.Name = "pnlNonTransparent";
            this.pnlNonTransparent.Size = new System.Drawing.Size(368, 126);
            this.pnlNonTransparent.TabIndex = 2;
            this.pnlNonTransparent.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlNonTransparent_Paint);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(290, 102);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 21);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDownloadStatus
            // 
            this.lblDownloadStatus.AutoSize = true;
            this.lblDownloadStatus.Location = new System.Drawing.Point(16, 87);
            this.lblDownloadStatus.Name = "上传状态:";
            this.lblDownloadStatus.Size = new System.Drawing.Size(59, 12);
            this.lblDownloadStatus.TabIndex = 4;
            this.lblDownloadStatus.Text = "上传状态:";
            // 
            // lblUploadDirectory
            // 
            this.lblUploadDirectory.AutoSize = true;
            this.lblUploadDirectory.Location = new System.Drawing.Point(16, 59);
            this.lblUploadDirectory.Name = "lblUploadDirectory";
            this.lblUploadDirectory.Size = new System.Drawing.Size(41, 12);
            this.lblUploadDirectory.TabIndex = 3;
            this.lblUploadDirectory.Text = "label4";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(16, 26);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(41, 12);
            this.lblFileName.TabIndex = 2;
            this.lblFileName.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "上传路径:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件名:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(46, 158);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(310, 21);
            this.progressBar1.TabIndex = 3;
            // 
            // frmUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 198);
            this.Controls.Add(this.pnlNonTransparent);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmUpload";
            this.Text = "上传文件";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpload_FormClosing);
            this.pnlNonTransparent.ResumeLayout(false);
            this.pnlNonTransparent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNonTransparent;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDownloadStatus;
        private System.Windows.Forms.Label lblUploadDirectory;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
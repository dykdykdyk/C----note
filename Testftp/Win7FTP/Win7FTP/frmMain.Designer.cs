using System.Windows.Forms;
namespace Win7FTP
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mMain = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.gbRemoteSite = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lstRemoteSiteFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.备注ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.btnNewDir = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRename = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpload = new System.Windows.Forms.ToolStripButton();
            this.btnDownload = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnRemoteBack = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.txtRemoteDirectory = new System.Windows.Forms.ToolStripTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbRemoteSite.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mMain
            // 
            this.mMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem3,
            this.menuItem4,
            this.menuItem5,
            this.menuItem6});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "File";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Edit";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "View";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 3;
            this.menuItem4.Text = "Server";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 4;
            this.menuItem5.Text = "Bookmarks";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 5;
            this.menuItem6.Text = "Help";
            // 
            // gbRemoteSite
            // 
            this.gbRemoteSite.Controls.Add(this.groupBox1);
            this.gbRemoteSite.Controls.Add(this.lstRemoteSiteFiles);
            this.gbRemoteSite.Controls.Add(this.toolStrip3);
            this.gbRemoteSite.Controls.Add(this.toolStrip2);
            this.gbRemoteSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRemoteSite.Location = new System.Drawing.Point(0, 0);
            this.gbRemoteSite.Name = "gbRemoteSite";
            this.gbRemoteSite.Size = new System.Drawing.Size(1017, 639);
            this.gbRemoteSite.TabIndex = 6;
            this.gbRemoteSite.TabStop = false;
            this.gbRemoteSite.Text = "服务器列表";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(0, 412);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(986, 227);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "打印信息：";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(7, 11);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(973, 208);
            this.textBox1.TabIndex = 0;
            // 
            // lstRemoteSiteFiles
            // 
            this.lstRemoteSiteFiles.AllowDrop = true;
            this.lstRemoteSiteFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstRemoteSiteFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader12,
            this.columnHeader13});
            this.lstRemoteSiteFiles.ContextMenuStrip = this.contextMenuStrip1;
            this.lstRemoteSiteFiles.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lstRemoteSiteFiles.GridLines = true;
            this.lstRemoteSiteFiles.Location = new System.Drawing.Point(3, 75);
            this.lstRemoteSiteFiles.MultiSelect = false;
            this.lstRemoteSiteFiles.Name = "lstRemoteSiteFiles";
            this.lstRemoteSiteFiles.Size = new System.Drawing.Size(983, 339);
            this.lstRemoteSiteFiles.SmallImageList = this.imageList1;
            this.lstRemoteSiteFiles.TabIndex = 4;
            this.lstRemoteSiteFiles.UseCompatibleStateImageBehavior = false;
            this.lstRemoteSiteFiles.View = System.Windows.Forms.View.Details;
            this.lstRemoteSiteFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstRemoteSiteFiles_MouseDoubleClick);
            this.lstRemoteSiteFiles.SelectedIndexChanged += new System.EventHandler(this.lstRemoteSiteFiles_SelectedValueChanged);
            this.lstRemoteSiteFiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstRemoteSiteFiles_Click);
            this.lstRemoteSiteFiles.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lstRemoteSiteFiles_ItemDrag);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名字";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "文件类型";
            this.columnHeader2.Width = 75;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "编号";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "日期";
            this.columnHeader5.Width = 120;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "大小";
            this.columnHeader6.Width = 85;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "状态";
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "备注";
            this.columnHeader13.Width = 405;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.备注ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // 备注ToolStripMenuItem
            // 
            this.备注ToolStripMenuItem.Name = "备注ToolStripMenuItem";
            this.备注ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.备注ToolStripMenuItem.Text = "备注";
            this.备注ToolStripMenuItem.Click += new System.EventHandler(this.备注ToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(1, 20);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStrip3
            // 
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewDir,
            this.toolStripSeparator3,
            this.btnRename,
            this.btnDelete,
            this.toolStripSeparator4,
            this.btnUpload,
            this.btnDownload,
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip3.Location = new System.Drawing.Point(3, 42);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(1011, 25);
            this.toolStrip3.TabIndex = 3;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // btnNewDir
            // 
            this.btnNewDir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnNewDir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNewDir.Name = "btnNewDir";
            this.btnNewDir.Size = new System.Drawing.Size(78, 22);
            this.btnNewDir.Text = "新建主机(N)";
            this.btnNewDir.Click += new System.EventHandler(this.btnNewDir_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRename
            // 
            this.btnRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRename.Image = global::Win7FTP.Properties.Resources.RenameFolderHS;
            this.btnRename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(68, 22);
            this.btnRename.Text = "重命名(M)";
            this.btnRename.ToolTipText = "Rename";
            this.btnRename.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDelete.Image = global::Win7FTP.Properties.Resources.DeleteHS;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(77, 22);
            this.btnDelete.Text = "删除文件(D)";
            this.btnDelete.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUpload
            // 
            this.btnUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUpload.Image = global::Win7FTP.Properties.Resources.FillUpHS;
            this.btnUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(77, 22);
            this.btnUpload.Text = "上传文件(U)";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDownload.Image = global::Win7FTP.Properties.Resources.FillDownHS;
            this.btnDownload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(77, 22);
            this.btnDownload.Text = "下载文件(D)";
            this.btnDownload.Click += new System.EventHandler(this.downloadToolStripMenuItem_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(109, 22);
            this.toolStripButton1.Text = "一键上传文件(UA)";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(109, 22);
            this.toolStripButton2.Text = "一键删除文件(DA)";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRemoteBack,
            this.toolStripSeparator2,
            this.txtRemoteDirectory});
            this.toolStrip2.Location = new System.Drawing.Point(3, 17);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1011, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnRemoteBack
            // 
            this.btnRemoteBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRemoteBack.Image = global::Win7FTP.Properties.Resources.GoRtlHS;
            this.btnRemoteBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoteBack.Name = "btnRemoteBack";
            this.btnRemoteBack.Size = new System.Drawing.Size(36, 22);
            this.btnRemoteBack.Text = "返回";
            this.btnRemoteBack.Click += new System.EventHandler(this.btnRemoteBack_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // txtRemoteDirectory
            // 
            this.txtRemoteDirectory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtRemoteDirectory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtRemoteDirectory.Name = "txtRemoteDirectory";
            this.txtRemoteDirectory.Size = new System.Drawing.Size(250, 25);
            this.txtRemoteDirectory.Click += new System.EventHandler(this.txtRemoteDirectory_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 639);
            this.Controls.Add(this.gbRemoteSite);
            this.Name = "frmMain";
            this.Text = "路由器管理平台";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.gbRemoteSite.ResumeLayout(false);
            this.gbRemoteSite.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public MainMenu mMain;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private MenuItem menuItem4;
        private MenuItem menuItem5;
        private MenuItem menuItem6;
        private GroupBox gbRemoteSite;
        private ToolStrip toolStrip2;
        private ToolStripButton btnRemoteBack;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripTextBox txtRemoteDirectory;
        private ToolTip toolTip1;
        public ListView lstRemoteSiteFiles;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ToolStrip toolStrip3;
        private ToolStripButton btnNewDir;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton btnRename;
        private ToolStripButton btnDelete;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton btnUpload;
        private ToolStripButton btnDownload;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ColumnHeader columnHeader12;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem 备注ToolStripMenuItem;
        private ColumnHeader columnHeader13;
        private ImageList imageList1;
        private ColumnHeader columnHeader1;
        private GroupBox groupBox1;
        private TextBox textBox1;

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Win7FTP
{
    public partial class frmNewFolder : Form
    {
        #region Members
        public string NewDirName = null;
        #endregion

        #region Constructor
        public frmNewFolder()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtNewDir.Text != null && textBox1.Text !=null)
            {
                //路由器编号，名称
                NewDirName = textBox1.Text+","+txtNewDir.Text;
                this.DialogResult = DialogResult.OK;
            }
        }
        #endregion

        private void frmNewFolder_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

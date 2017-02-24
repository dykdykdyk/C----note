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
    public partial class FrmChild : Form
    {
        public FrmChild()
        {
            InitializeComponent();
        }
        private string strValue = "";
        public string StrValue
        {
            get { return strValue; }
            set { strValue = value; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            strValue = textBox1.Text; //将文本框的值赋予窗体的属性
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

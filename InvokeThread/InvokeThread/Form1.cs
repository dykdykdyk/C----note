using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
/**
 * 线程间操作无效: 从不是创建控件“textBox1”的线程访问它。
 * 1.Control.CheckForIllegalCrossThreadCalls = false;不检查线程安全
 * 2.委托
 * 3.使用backgroundWorker控件 
 * this.backgroundWorker1.RunWorkerAsync();开始执行后台操作，然后就可以调用控件了
 * 
 */
namespace InvokeThread
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Thread workThread, workThreadtwo;
        delegate void SetTextCallBack(string text);//定义委托

        delegate void SetTextBoxValueCallBack(string message, TextBox txt, bool IsAppend);
        public void SetTextBoxValue(string message, TextBox txt, bool IsAppend)
        {
            if (txt.InvokeRequired)
            {
                SetTextBoxValueCallBack d = new SetTextBoxValueCallBack(SetTextBoxValue);
                this.Invoke(d, new object[] { message, txt, IsAppend });
            }
            else
            {
                if (IsAppend == true)
                {
                    txt.Text += message;
                }
                else
                {
                    txt.Text = message;
                }
            }
        }

        public void textBox1show() {
            //textBox1.Text = "测试";//这样是OK的

            workThread = new Thread(delegate()
            {
                //1.不检查县城安全
                //Control.CheckForIllegalCrossThreadCalls = false;
                textBox1.Text = "测试";//线程间操作无效: 从不是创建控件“textBox1”的线程访问它。
              

                //2.委托
                //SetTextBoxValue("asdf", textBox1, false);

                //3.
            });
            workThread.Start();

        }


        private void backgroundWorker1_RunWorkerCompleted(
           object sender,
           RunWorkerCompletedEventArgs e)
        {
            //textBox1show();
            this.textBox1.Text =
                "This text was set safely by BackgroundWorker.";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            textBox1show();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //this.backgroundWorker1.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.RunWorkerAsync();//
        }
    }
}

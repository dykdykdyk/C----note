using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
/*
 * ListView 两种添加数据的方式 
 * ListView 数据进行按从大到小排序 以及从小到大排序
 * 右键菜单的简单使用 
 * 两个窗体中获取到的数据的传递
 * 各种弹出框总结
 *如果是手动添加行，需要先设置View =Details .FullRowSelect = true;//是否可以选择行
 *设置管理员权限 项目 右键 添加-新建项- 应用程序清单文件 - <requestedExecutionLevel  level="asInvoker" uiAccess="false" />
 *改成  <requestedExecutionLevel level="requireAdministrator" uiAccess="false" />请求管理员权限
 *遍历文件夹
 *日志输出清单 groupBox 里面嵌套一个textbox控件 
 *
 * listview 行高设置方法：
 *1.添加一个imagelist1控件
 *设置宽度为1，高度为业务需要设置的高度
 *2.listview属性里面 smallimage 设置成imagelist1
 *
 */
namespace WidgetCollectionDemo
{
    public partial class Form1 : Form
    {
         ListViewColumnSorter lvwColumnSorter;
     
        public Form1()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            listView2.ListViewItemSorter = lvwColumnSorter;
        }
      
        private void Form1_Load(object sender, EventArgs e)
        {
            ListViewItem item = new ListViewItem();
            item.SubItems[0].Text = "Tommy";//
            //item.SubItems[1].Text = "boy";
            //item.SubItems[2].Text = "23";
            item.SubItems.Add("boy");
            item.SubItems.Add("23");
            listView1.Items.Add(item);

            ListViewItem item2 = new ListViewItem();
            item2.SubItems[0].Text = "Tomm";//
            //item.SubItems[1].Text = "boy";
            //item.SubItems[2].Text = "23";

            item2.SubItems.Add("boy");
            item2.SubItems.Add("23");
            listView1.Items.Add(item2);
            test();
            SortFiles("D:\\EditPlus\\");
            //test22();

            //日志输出信息
            displayLog("启动");

        }
        //显示日志信息
        public void displayLog(string log) {
            textBox1.AppendText(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss   ") + log + "\r\n");
        }

        //动态添加数据
        public void test() {
            listView2.Columns.Add("名字", 70, HorizontalAlignment.Center);//表头名，长度，格式
            listView2.Columns.Add("密码", 70);
            listView2.Columns.Add("状态", 70, HorizontalAlignment.Center);
               listView2.GridLines = true; //显示表格线
            listView2.View = View.Details;//显示表格细节
            listView2.HeaderStyle = ColumnHeaderStyle.Clickable;//对表头进行设置
            listView2.FullRowSelect = true;//是否可以选择行
            String[] post = new String[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
            //        DataTable dt =“你查询出来的数据";//数据查询什么的我就不啰嗦了
            for (int i = 0; i < post.Length; i++)
            {
                ListViewItem[] listViewItem = new ListViewItem[1];
                listViewItem[0] = new ListViewItem(new string[] { post[i], post[i], post[i] });
                listView2.Items.AddRange(listViewItem);
            }
        }
        /// <summary>
        /// 右键菜单 使用
        /// 1.contextMenuStrip1 点击空白地方 输入右键菜单要显示的名称
        /// 2.双击名称后出现下面的方法
        /// 3.然后写逻辑处理
        /// 4.绑定控件 选择一个listview,查看属性 contextMenuStrip1
        /// 5.选择contextMenuStrip1 即可。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 右键菜单测试1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("您点击了 右键菜单测试 菜单~");
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;

            //"确定要退出吗？"是对话框的显示信息，"退出系统"是对话框的标题

            //默认情况下，如MessageBox.Show("确定要退出吗？")只显示一个“确定”按钮。
            DialogResult dr = MessageBox.Show("确定要退出吗?", "退出系统", messButton);

            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                MessageBox.Show("您点击了 确定 菜单~");
            }
            else//如果点击“取消”按钮
            {
                MessageBox.Show("您点击了 取消 菜单~");
            }
        }

        private void SortAsFileName(ref FileInfo[] arrFi)
        {
            Array.Sort(arrFi, delegate(FileInfo x, FileInfo y) { return x.Name.CompareTo(y.Name); });
        }

         List<String> list = new List<string>();
        private void SortFiles(string sSourcePath)
        {

          
            //在指定目录及子目录下查找文件,在list中列出子目录及文件
            //

            DirectoryInfo Dir = new DirectoryInfo(sSourcePath);
            DirectoryInfo[] DirSub = Dir.GetDirectories();
            if (DirSub.Length <= 0)
            {
                foreach (FileInfo f in Dir.GetFiles("*.*", SearchOption.TopDirectoryOnly)) //查找文件
                {
                    list.Add(Dir + @"\" + f.ToString());
                    ListViewItem item = new ListViewItem();
                    item.SubItems[0].Text = Dir + @"\" + f.ToString();//
                    //item.SubItems[1].Text = "boy";
                    //item.SubItems[2].Text = "23";
                    item.SubItems.Add("boy");
                    item.SubItems.Add("23");
                    listView2.Items.Add(item);
                }
            }
            int t = 1;
            foreach (DirectoryInfo d in DirSub)//查找子目录 
            {
                SortFiles(Dir + @"\" + d.ToString());
                list.Add(Dir + @"\" + d.ToString());
                if (t == 1)
                {
                    foreach (FileInfo f in Dir.GetFiles("*.*", SearchOption.TopDirectoryOnly)) //查找文件
                    {
                        list.Add(Dir + @"\" + f.ToString());
                        ListViewItem item = new ListViewItem();
                        item.SubItems[0].Text = Dir + @"\" + f.ToString();//
                        item.SubItems.Add("boy");
                        item.SubItems.Add("23");
                        listView2.Items.Add(item);
                    }
                    t = t + 1;
                }
            }   
        }
        public void test22() {
            foreach (String li in list)
            {
                Console.WriteLine("list:" + li);
            }
        }
        //对listview进行排序 的方法 要设置ColumnClick 属性
        private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                // 检查点击的列是不是现在的排序列.
                if (e.Column == lvwColumnSorter.SortColumn)
                {
                    // 重新设置此列的排序方法.
                    if (lvwColumnSorter.Order == SortOrder.Ascending)
                    {
                        lvwColumnSorter.Order = SortOrder.Descending;
                    }
                    else
                    {
                        lvwColumnSorter.Order = SortOrder.Ascending;
                    }
                }
                else
                {
                    // 设置排序列，默认为正向排序
                    lvwColumnSorter.SortColumn = e.Column;
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
                // 用新的排序方法对ListView排序
                this.listView2.Sort();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}

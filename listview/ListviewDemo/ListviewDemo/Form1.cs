using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
/***
 *listview 添加图片
 *listview 添加数据 ：listview1.BeginUpdate();
 *listview1.EndUpdate();
 *ImageList imgList = new ImageList();  
 *imgList.ImageSize = new Size(1, 20);// 设置行高 20 //分别是宽和高  
 *listView1.SmallImageList = imgList; //这里设置listView的SmallImageL
 *自定义listview.
 * 
 */
namespace ListviewDemo
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //name();
        }
        public void name() {
            ListViewItem lv = new ListViewItem();
            
            lv.SubItems.Add("000");
            lv.SubItems.Add("111");
            //lv.SubItems.Add("222");
            lv.ImageIndex = 2;//显示imageList中的第一张图片
            listView1.Items.Add(lv);

        }
        //details
        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count != 0) {
                listView1.Clear();
            }
            ColumnHeader ch = new ColumnHeader();

            ch.Text = "列标题1";   //设置列标题  

            ch.Width = 120;    //设置列宽度  

            ch.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式  

            this.listView1.Columns.Add(ch);    //将列头添加到ListView控件。 


            //列
            ColumnHeader ch2 = new ColumnHeader();

            ch2.Text = "列标题2";   //设置列标题  

            ch2.Width = 120;    //设置列宽度  

            ch2.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式  

            this.listView1.Columns.Add(ch2);    //将列头添加到ListView控件。 

            //列
            ColumnHeader ch3 = new ColumnHeader();

            ch3.Text = "列标题1";   //设置列标题  

            ch3.Width = 120;    //设置列宽度  

            ch3.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式  

            this.listView1.Columns.Add(ch3);    //将列头添加到ListView控件。 

            //this.listView1.Columns.Add("列标题1", 120, HorizontalAlignment.Left); //一步添加  
          //添加数据
            this.listView1.BeginUpdate();  //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 
            for (int i = 0; i < 10; i++)   //添加10行数据 
            {
                ListViewItem lvi = new ListViewItem();

                lvi.ImageIndex = i;     //通过与imageList绑定，显示imageList中第i项图标 

                lvi.Text = "subitem" + i;


                lvi.ImageIndex = i;   
                lvi.SubItems.Add("第2列,第" + i + "行");

                lvi.SubItems.Add("第3列,第" + i + "行");

                this.listView1.Items.Add(lvi);
            }
            this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。
            //显示项
            foreach (ListViewItem item in this.listView1.Items)
            {
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    //MessageBox.Show(item.SubItems[i].Text);
                }
            } 

            //移除某一项
            foreach (ListViewItem lvi in listView1.SelectedItems)  //选中项遍历 
            {
                //listView1.Items.RemoveAt(lvi.Index); // 按索引移除 
                //listView1.Items.Remove(lvi);   //按项移除 
            } 

            //行高设置
            //ImageList imgList = new ImageList();

            //imgList.ImageSize = new Size(1, 20);// 设置行高 20 //分别是宽和高 

            listView1.SmallImageList = imageList1; //这里设置listView的SmallImageList ,用imgList将其撑大 

            //清空
            //this.listView1.Clear();  //从控件中移除所有项和列（包括列表头）。 

            //this.listView1.Items.Clear();  //只移除所有的项。
   
 

        }
        //largeicon
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count != 0)
            {
                listView1.Clear();
            }
            this.listView1.View = View.LargeIcon;

            this.listView1.LargeImageList = this.imageList1;

            this.listView1.BeginUpdate();

            for (int i = 0; i < 10; i++)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.ImageIndex = i;

                lvi.Text = "item" + i;

                this.listView1.Items.Add(lvi);
            }

            this.listView1.EndUpdate(); 

        }
        //smallicon
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count != 0)
            {
                listView1.Clear();
            }
            this.listView1.View = View.SmallIcon;

            this.listView1.SmallImageList = this.imageList1;

            this.listView1.BeginUpdate();

            for (int i = 0; i < 10; i++)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.ImageIndex = i;

                lvi.Text = "item" + i;

                this.listView1.Items.Add(lvi);
            }

            this.listView1.EndUpdate();  
        }
        //list
        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count != 0)
            {
                listView1.Clear();
            }
            this.listView1.View = View.List;

            this.listView1.SmallImageList = this.imageList1;

            this.listView1.BeginUpdate();

            for (int i = 0; i < 10; i++)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.ImageIndex = i;

                lvi.Text = "item" + i;

                this.listView1.Items.Add(lvi);
            }
            this.listView1.EndUpdate();  

        }
        //分组 有bug
        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count != 0)
            {
                listView1.Clear();
            }
           
            ListViewGroup man_lvg = new ListViewGroup();  //创建男生分组  

            man_lvg.Header = "男生";  //设置组的标题。  

            //man_lvg.Name = "man";   //设置组的名称。  

            man_lvg.HeaderAlignment = HorizontalAlignment.Left;   //设置组标题文本的对齐方式。（默认为Left）  

            ListViewGroup women_lvg = new ListViewGroup();  //创建女生分组  

            women_lvg.Header = "女生";

            //women_lvg.Name = "women";  

            this.listView1.View = View.List;

            women_lvg.HeaderAlignment = HorizontalAlignment.Center;   //组标题居中对齐  

            this.listView1.Groups.Add(man_lvg);    //把男生分组添加到listview中  

            this.listView1.Groups.Add(women_lvg);   //把男生分组添加到listview中  

            this.listView1.ShowGroups = true;  //记得要设置ShowGroups属性为true（默认是false），否则显示不出分组  
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.BeginUpdate();
            for (int i = 0; i < 5; i++)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.ImageIndex = i;

                lvi.Text = "item" + i;

                lvi.ForeColor = Color.Blue;  //设置行颜色  

                lvi.SubItems.Add("第2列,第" + i + "行");

                lvi.SubItems.Add("第3列,第" + i + "行");

                man_lvg.Items.Add(lvi);   //分组添加子项  

                // 或 lvi.Group = man_lvg;  //分组添加子项  

                this.listView1.Items.Add(lvi);
            }
            this.listView1.EndUpdate();

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}

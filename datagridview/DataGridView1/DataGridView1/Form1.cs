using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
/*
 *DataGridView简单课程表制作
 *label属性 如果设置大小 需要将autosize改成 false 才有用
 *隐藏头一列 RowHeadersVisible 设置为false才有用
 *隐藏最后一行 AllowUserToAddRows 
 */
namespace DataGridView1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int num, week;   // 周数节数，第二步的时候用得到。
            DataTable dt = new DataTable("subject");
            dt.Columns.Add("时段", typeof(string)); 
            dt.Columns.Add("节次", typeof(string));   //添加列集，下面都是
            dt.Columns.Add("周一", typeof(string));
            dt.Columns.Add("周二", typeof(string));
            dt.Columns.Add("周三", typeof(string));
            dt.Columns.Add("周四", typeof(string));
            dt.Columns.Add("周五", typeof(string));
            for (int i = 0; i < 10; i++)  //用循环添加4个行集~
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            dt.Rows[0][0] = "早上";  //向第一行里的第一个格中添加一个“第1节”
            dt.Rows[1][0] = "上午";  //向第二行里的第一个格中添加一个“第 2 节”
            dt.Rows[2][0] = "";  //向第三行里的第一个格中添加一个“第3节”
            dt.Rows[3][0] = "";  //向第四行里的第一个格中添加一个“第4节”
            dt.Rows[4][0] = "";  //向第一行里的第一个格中添加一个“第5节”
            dt.Rows[5][0] = "下午";  //向第二行里的第一个格中添加一个“第 6 节”
            dt.Rows[6][0] = "";  //向第三行里的第一个格中添加一个“第7节”
            dt.Rows[7][0] = "";  //向第四行里的第一个格中添加一个“第8节”
            dt.Rows[8][0] = "晚上";  //向第四行里的第一个格中添加一个“第8节”

            dt.Rows[9][1] = "2";  //向第四行里的第一个格中添加一个“第8节”
            //dt.Rows[9][1] = "2"; 

            dt.Rows[0][1] = "1";  //向第一行里的第一个格中添加一个“第1节”
            dt.Rows[1][1] = "1";  //向第二行里的第一个格中添加一个“第 2 节”
            dt.Rows[2][1] = "2";  //向第三行里的第一个格中添加一个“第3节”
            dt.Rows[3][1] = "3";  //向第四行里的第一个格中添加一个“第4节”
            dt.Rows[4][1] = "4";  //向第一行里的第一个格中添加一个“第5节”
            dt.Rows[5][1] = "1";  //向第二行里的第一个格中添加一个“第 6 节”
            dt.Rows[6][1] = "2";  //向第三行里的第一个格中添加一个“第7节”
            dt.Rows[7][1] = "3";  //向第四行里的第一个格中添加一个“第8节”
            dt.Rows[8][1] = "1";  //向第四行里的第一个格中添加一个“第8节”
            dt.Rows[8][1] = "1";  //向第四行里的第一个格中添加一个“第8节”

            //dt.Rows[9][1] = "1";  //向第三行里的第一个格中添加一个“第7节”
            //dt.Rows[9][1] = "2";  //向第四行里的第一个格中添加一个“第8节”


            dt.Rows[6][5] = "C#程序设计基础";  //向第四行里的第一个格中添加一个“第8节”
            dt.Rows[5][2] = "数学";  //向第三行里的第一个格中添加一个“第7节”
            dt.Rows[0][3] = "化学";  //向第三行里的第一个格中添加一个“第7节”
            dt.Rows[1][3] = "语文";  //向第三行里的第一个格中添加一个“第7节”

            dt.Rows[1][3] = "语文";  //向第三行里的第一个格中添加一个“第7节”
            dt.Rows[1][3] = "语文";  //向第三行里的第一个格中添加一个“第7节”

            this.dataGridView1.DataSource = dt;

            //dataGridView1.Columns[0].Visible = false;
            //dataGridView1.Rows[9].Visible = false;
            //dataGridView1.Rows[8].Visible = false;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

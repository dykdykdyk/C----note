using BleTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpDemo
{
    public partial class Login : Form
    {
        string ftpServerIP;
        string ftpUser;
        string ftpPassword;
        string ftpURI;

        public Login()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //ip
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //用户名
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        //密码：
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Cursor currentCursor = this.Cursor;
            //ftpURI = "ftp://" + textBox1.Text.ToString() + "/";
            //ftpUser = textBox2.Text.ToString();
            //ftpPassword = textBox3.Text.ToString();

            //System.Diagnostics.Debug.WriteLine(" ftpURI  " + ftpURI + "  ftpUser  " + ftpUser + " ftpPassword " + ftpPassword);
            //try
            //{
            //    StringBuilder result = new StringBuilder();
            //    //ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI));//171.1.31.1
            //    Request = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://192.168.1.75/"));
            //    //ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);
            //    Request.Credentials = new NetworkCredential("123", "123");
            //    Request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            //    Response = (FtpWebResponse)Request.GetResponse();

            //    ListFilesAndDirectories();
            //    StreamReader reader = new StreamReader(Response.GetResponseStream(), Encoding.Default);
            //    string line = reader.ReadLine() + "\r";
            //    while (line != null)
            //    {
            //        System.Diagnostics.Debug.WriteLine(" line:  " + line);
            //        result.Append(line);
            //        result.Append("\n");
            //        line = reader.ReadLine() + "\r";
            //        //System.Diagnostics.Debug.WriteLine(" result:  "+ result);
            //    }
            //    System.Diagnostics.Debug.WriteLine(" result:  " + result);
            //    result.Remove(result.ToString().LastIndexOf("\n"), 1);


            //    reader.Close();
            //    Response.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error FTP Client",
            //    MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    //downloadFiles = null;
            //    //return downloadFiles;
            //}

            //FtpHelper ftphelper = new FtpHelper("192.168.1.75", "123", "123");
            //FtpWebResponse request = ftphelper.Open(null, WebRequestMethods.Ftp.ListDirectoryDetails);
            //ftphelper.ListFiles();

            ////FileStruct[] listAll = ftphelper.ListFiles();
            //List<FileStruct> listFile = ftphelper.ListFiles();

            //listView1.GridLines = true; //显示表格线
            ////listView1.View = View.Details;//显示表格细节
            //listView1.HeaderStyle = ColumnHeaderStyle.Clickable;//对表头进行设置
            //listView1.FullRowSelect = true;//是否可以选择行
            //ListViewItem[] listViewItem = new ListViewItem[1];

            //foreach (FileStruct file in listFile)
            //{
            //    if (!file.IsDirectory)
            //    {
            //        listViewItem[0] = new ListViewItem(file.Name);
            //        listView1.Items.AddRange(listViewItem);
            //    }
            //}

            new FileExplorer.Form1().Show();
            //Show();
            this.Hide();
            //System.Diagnostics.Debug.WriteLine(" ftphelper.ListFiles();  96 :  " + ftphelper.ListFiles());
            //listView1.Items.Add(ftphelper.ListFiles().Name());



        }

        #region 列出目录文件信息
        ///**/
        ///// <summary>
        ///// 列出FTP服务器上面当前目录的所有文件和目录
        ///// </summary>
        //public void ListFilesAndDirectories()
        //{
        //    WebResponse Response = Open("ftp://192.168.1.75/", WebRequestMethods.Ftp.ListDirectoryDetails);
        //    StreamReader stream = new StreamReader(Response.GetResponseStream(), Encoding.Default);
        //    String Datastring = stream.ReadToEnd();
        //    //FileInfo list = GetList(Datastring);
        //    DirectoryInfo currentDir = new DirectoryInfo(Datastring);
        //    DirectoryInfo[] dirs = currentDir.GetDirectories(); //获取目录
        //    FileInfo[] files = currentDir.GetFiles();


        //    foreach (FileInfo file in files)
        //    {
        //        ListViewItem fileItem = listView1.Items.Add(file.Name);
        //        System.Diagnostics.Debug.WriteLine(" fileItem  ");
        //        //return list;
        //    }
        //}

        private FileStruct ParseFileStructFromWindowsStyleRecord(string Record)
        {
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            string dateStr = processstr.Substring(0, 8);
            processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
            string timeStr = processstr.Substring(0, 7);
            processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
            DateTimeFormatInfo myDTFI = new CultureInfo("en-CN", false).DateTimeFormat;
            myDTFI.ShortTimePattern = "t";
            f.CreateTime = DateTime.Parse(dateStr + " " + timeStr, myDTFI);
            if (processstr.Substring(0, 5) == "<DIR>")
            {
                f.IsDirectory = true;
                processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
            }
            else
            {
                string[] strs = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);   // true);
                processstr = strs[1];
                f.IsDirectory = false;
            }
            f.Name = processstr;
            return f;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

    }
}
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Dialogs;
using Win7FTP.Library;
using System.IO;
using System.Collections;
using System.Net;
using Raccoom.Windows.Forms;
using Win7FTP.HelperClasses;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Win7FTP
{
    public partial class frmMain : Form
    {
        #region Members
        TreeNode DirNode;
        public FTPclient FtpClient;
        CommonOpenFileDialog objOpenDialog;
        frmRename RenameDialog;
        ListViewItem Message;
        string AppID = "FTP";
        ListViewColumnSorter lvwColumnSorter;
        String filename = "";
        Thread workThread, workThreadtwo;
        //test.txt集合
        List<String> LineList = new List<String> { };
        //判断状态集合
        List<String> LinesCounts = new List<String> { };
        //listview拍续集和
        //List<ListViewItem> listviewcounts = new List<ListViewItem> { };
        
        List<FTPfileInfo> listfolders= new List<FTPfileInfo>{};
        #endregion

        #region Constructor
        public frmMain()
        {
            //Init frmMain
            InitializeComponent();

            //Set up Components
            //Set standard data provider
            //this.tvFileSystem.DataSource =
            // new Raccoom.Windows.Forms.TreeViewFolderBrowserDataProvider();

            // fill root level
            //this.tvFileSystem.Populate();
            //this.tvFileSystem.Nodes[0].Expand();
            //Set Selected Directory
            txtRemoteDirectory.Text = "/";
            lstRemoteSiteFiles.FullRowSelect = true;
            //listview头模拟点击

            // lvwColumnSorter = new ListViewColumnSorter();
            //this.lstRemoteSiteFiles.ListViewItemSorter = lvwColumnSorter;
            Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance.ApplicationId = AppID;
            ReadFile();
            test2();
            downloadtest("rsyncd");
            //读取文件
            test22();
        }

        private void lstRemoteSiteFiles_Click(object sender, ColumnClickEventArgs e)
        {
            //try {
            //    // 检查点击的列是不是现在的排序列.
            //    if (e.Column == lvwColumnSorter.SortColumn)
            //    {
            //        // 重新设置此列的排序方法.
            //        if (lvwColumnSorter.Order == SortOrder.Ascending)
            //        {
            //            lvwColumnSorter.Order = SortOrder.Descending;
            //        }
            //        else
            //        {
            //            lvwColumnSorter.Order = SortOrder.Ascending;
            //        }
            //    }
            //    else
            //    {
            //        // 设置排序列，默认为正向排序
            //        lvwColumnSorter.SortColumn = e.Column;
            //        lvwColumnSorter.Order = SortOrder.Ascending;
            //    }
            //    // 用新的排序方法对ListView排序
            //    this.lstRemoteSiteFiles.Sort();  
            //}
            //catch {
            //    throw new NotImplementedException();
            //}
            
           
        }
        public void test2()
        {
            System.Timers.Timer pTimer = new System.Timers.Timer(300000);//每隔5分钟秒执行一次，没用winfrom自带的
            pTimer.Elapsed += pTimer_Elapsed;//委托，要执行的方法
            pTimer.AutoReset = true;//获取该定时器自动执行
            pTimer.Enabled = true;//这个一定要写，要不然定时器不会执行的
            Control.CheckForIllegalCrossThreadCalls = false;//这个不太懂，有待研究
        }
        private void pTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            downloadtest("rsyncd");
            test22();
        }
    public void  test22(){

        string[] allLines = System.IO.File.ReadAllLines("C:/Windows/rsyncd");
        string lastestLine = allLines[allLines.Length - 1];
        lastestLine = lastestLine.Replace(" ", "");
        lastestLine = lastestLine.Trim();
        System.Diagnostics.Debug.WriteLine("  lastestLine  " + lastestLine);
        //将日期和时间提取出来 到分
        String Stimeminute = lastestLine.Substring(0, 15);
        System.Diagnostics.Debug.WriteLine("  Stimeminute  " + Stimeminute);

        //将年月日提取出来
        String Ytimeminute = Stimeminute.Substring(0, 10);
        System.Diagnostics.Debug.WriteLine("  Ytimeminute  " + Ytimeminute);

        //将小时和分钟提取出来
        String Htimeminute = lastestLine.Substring(10, 5);
        System.Diagnostics.Debug.WriteLine("  Htimeminute  " + Htimeminute);


        //对Htimeminute进行处理 15:00
        //获取到分钟 
        String Mtimeminute = Htimeminute.Substring(3, 2);
        int Mtimeminutes = int.Parse(Mtimeminute);
        System.Diagnostics.Debug.WriteLine("  Mtimeminute  " + Mtimeminute + " Mtimeminutes: " + Mtimeminutes);
        //获取小时
        String Hourtime = Htimeminute.Substring(0, 2);
        int Hourtimes = int.Parse(Hourtime);
        System.Diagnostics.Debug.WriteLine("  Hourtime  " + Hourtime + " Hourtimes: " + Hourtimes);
        //定义需要搜索字符串的小时和分钟
        String Hstarttime, Mstarttime;


        //时间格式 15：00
        if (Mtimeminutes == 0)
        {
            Mstarttime = "55";
            Hstarttime = (Hourtimes - 1).ToString();
            //时间格式 15：05
        }
        else if (Mtimeminutes == 5)
        {
            Mstarttime = "00";
            Hstarttime = Hourtimes.ToString();
        }
        //时间格式 15：10
        else if (Mtimeminutes == 10)
        {
            Mstarttime = "05";
            Hstarttime = Hourtimes.ToString();
        }
        ////时间格式 00：00
        //时间格式 15：55 通常
        else
        {
            Mstarttime = (Mtimeminutes - 5).ToString();
            Hstarttime = Hourtimes.ToString();
        }
        System.Diagnostics.Debug.WriteLine("  Mstarttime:  " + Mstarttime + " Hstarttime: " + Hstarttime);

        //需要搜索的字符串
        string HMstarttime = Hstarttime + ":" + Mstarttime;
        System.Diagnostics.Debug.WriteLine("  HMstarttime:  " + HMstarttime);
        //搜索的行数加入新的数组
        List<String> LinesList = new List<String> { };
        String Temptimeminute;
        String Tempyourtime;
        for (int i = 0; i < allLines.Length; i++)
        {
            allLines[i] = allLines[i].Replace(" ", "");
            allLines[i] = allLines[i].Trim();

            //当前行数的时间值是否与Sstarttime以及日期是否相等，如相等，则加入新的数组
            Temptimeminute = allLines[i].Substring(10, 5);
            Tempyourtime = allLines[i].Substring(0, 10);
            //System.Diagnostics.Debug.WriteLine("  Temptimeminute:  " + Temptimeminute);
            if ((Temptimeminute.Equals(Htimeminute) && Tempyourtime.Equals(Ytimeminute)) ||
                (Temptimeminute.Equals(HMstarttime) && Tempyourtime.Equals(Ytimeminute)))
            {
                //LinesList.Add(allLines[i]);
                allLines[i] = allLines[i].Substring(10, allLines[i].Length - 10);
                if (allLines[i].Contains("/"))
                {
                    LinesList.Add(allLines[i]);
                }
            }
        }

        //打印出新的集合 直接提取数据
       
        int index;
        int indexL;
        int tempcount;
        string countsString;
        for (int i = 0; i < LinesList.Count; i++)
        {
            //System.Diagnostics.Debug.WriteLine("  LinesList[i]:  " + LinesList[i]);
            index = LinesList[i].IndexOf("/");
            indexL = LinesList[i].LastIndexOf("/");
            tempcount = indexL - index;
            if (tempcount == 0)
            {
                countsString = LinesList[i].Substring(index + 1, 1);
            }
            else {
                countsString = LinesList[i].Substring(index + 1, tempcount - 1);
            }
            LinesCounts.Add(countsString);
          
        }
        if (FtpClient != null) {
            RefreshDirectory();
        }
        //LinesCounts.Add("1");
        //LinesCounts.Add("105");
        for (int i = 0; i < LinesCounts.Count; i++)
        {
            //System.Diagnostics.Debug.WriteLine("  LinesCounts[i]:  " + LinesCounts[i]);
        }
    }
    //下载txt文件
    public void downloadtest(string path)
    {
        String FtpPath = "ftp://120.77.249.136/" + path;
        Uri uri = new Uri(FtpPath);
        FileStream fs = null;
        Stream responseStream = null;
        try
        {
            //创建一个与FTP服务器联系的FtpWebRequest对象
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            //设置请求的方法是FTP文件下载
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.UsePassive = false;
            //连接登录FTP服务器
            request.Credentials = new NetworkCredential("uftp", "uftp2017");
            //获取一个请求响应对象
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            //获取请求的响应流
            responseStream = response.GetResponseStream();
            //判断本地文件是否存在，如果存在，则打开和重写本地文件
            if (File.Exists(path))
            {

                fs = File.Open("C:/Windows/" + path, FileMode.Open, FileAccess.ReadWrite);

            }
            //判断本地文件是否存在，如果不存在，则创建本地文件
            else
            {
                fs = File.Create("C:/Windows/" + path);
            }
            if (fs != null)
            {
                int buffer_count = 65536;
                byte[] buffer = new byte[buffer_count];
                int size = 0;
                while ((size = responseStream.Read(buffer, 0, buffer_count)) > 0)
                {
                    fs.Write(buffer, 0, size);
                }
                fs.Flush();
                fs.Close();
                responseStream.Close();
            }
        }
        finally
        {
            if (fs != null)
                fs.Close();
            if (responseStream != null)
                responseStream.Close();
        }
    }



     
      public void ReadFile(){
          //读取文件 这边 一般建议获取文本编码
          FileStream fs2 = new FileStream("C:/Windows/test.txt", FileMode.Open);
          StreamReader sr = new StreamReader(fs2, System.Text.Encoding.UTF8);
          //StreamReader sr = GetFileEncodeType("C:/Windows/test.txt");
          string sLine = "";
          //System.Diagnostics.Debug.WriteLine("" + sLine);
          //List<String> LineList = new ArrayList();
          while (sLine != null)
          {
              sLine = sr.ReadLine();
              if (sLine != null && !sLine.Equals(""))
              {
                  if (exists(sLine))
                  {
                      LineList.Add(sLine);
                  }
              }
              //System.Diagnostics.Debug.WriteLine(sLine);
          }
          //System.Diagnostics.Debug.WriteLine(LineList);
          sr.Close();
      }
      //如果 之前已经有了数据，不用再加进去
      public bool exists(string name)
      {
          for (int i = 0; i < LineList.Count; i++)
          {
              if (Separatenumber(name).Equals(Separatenumber(LineList[i])))
              {
                  return false;
              }
              else
              {
                  //return true;  
                  if (i == LineList.Count)
                  {
                      return true;
                  }
              }
          }
          return true;
      }

      public StreamReader GetFileEncodeType(string filename)
      {

          System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
          System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
          Byte[] buffer = br.ReadBytes(2);
          if (buffer[0] >= 0xEF)
          {
              if (buffer[0] == 0xEF && buffer[1] == 0xBB)
              {
                  //return System.Text.Encoding.UTF8;
                  return new StreamReader(fs, System.Text.Encoding.UTF8);
              }
              else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
              {
                  //return System.Text.Encoding.BigEndianUnicode;
                  return new StreamReader(fs, System.Text.Encoding.BigEndianUnicode);
              }
              else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
              {
                  //return System.Text.Encoding.Unicode;
                  return new StreamReader(fs, System.Text.Encoding.Unicode);
              }
              else
              {
                  //return System.Text.Encoding.Default;
                  return new StreamReader(fs, System.Text.Encoding.Default);
              }
          }
          else
          {
              //return System.Text.Encoding.Default;
              return new StreamReader(fs, System.Text.Encoding.Default);
          }
      }

      //public System.Text.Encoding GetFileEncodeType(string filename)
      //{
          
      //    System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
      //    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
      //    Byte[] buffer = br.ReadBytes(2);
      //    if (buffer[0] >= 0xEF)
      //    {
      //        if (buffer[0] == 0xEF && buffer[1] == 0xBB)
      //        {
      //            return System.Text.Encoding.UTF8;
      //        }
      //        else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
      //        {
      //            return System.Text.Encoding.BigEndianUnicode;
      //        }
      //        else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
      //        {
      //            return System.Text.Encoding.Unicode;
      //        }
      //        else
      //        {
      //            return System.Text.Encoding.Default;
      //        }
      //    }
      //    else
      //    {
      //        return System.Text.Encoding.Default;
      //    }
      //}
      public void writefile(String name) {
          //String name = "公司名称,22";
          String tempname = name;
          for (int i = 0; i < LineList.Count; i++)
          {
              //判断当前文件里面是否含有相同编号的信息，如果有，删除，并且重新添加新的信息
              if (Separatenumber(name).Equals(Separatenumber(LineList[i])))
              {
                  //name = name +":"+ SeparateMessage(LineList[i]);
                  LineList.Remove(LineList[i]);
                  //name = name + "\r\n" + LineList[i];
                  //i = LineList.Count;
              }
              else
              {
                  if (i == LineList.Count - 1)
                  {
                      //name = name + "\r\n" + LineList[i];
                  }
              }
              name = name + "\r\n" + LineList[i];
          }
          //System.Diagnostics.Debug.WriteLine("" + name);
          FileStream fs = new FileStream("C:/Windows/test.txt", FileMode.Create);
          //获得字节数组
          byte[] data = new UTF8Encoding().GetBytes(name);
          //开始写入
          fs.Write(data, 0, data.Length);
          //清空缓冲区、关闭流
          fs.Flush();
          fs.Close();
          //frmLogin fl = new frmLogin();
          //更新test.txt文件
          UpLoadFile("C:/Windows/test.txt", "ftp://120.77.249.136/test.txt");
          //
          LineList.Clear();
          ReadFile();

      }

      // 上传test文件
      public void UpLoadFile(string localFile, string ftpPath)
      {
          String FtpPath = "ftp://120.77.249.136/test.txt";
          Uri uri = new Uri(FtpPath);
          if (!File.Exists(localFile))
          {
              //Response.Write("文件：“" + localFile + "” 不存在！");
              return;
          }
          FileInfo fileInf = new FileInfo(localFile);
          FtpWebRequest reqFTP;
          //Stream responseStream = null;
          reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);// 根据uri创建FtpWebRequest对象   
          reqFTP.Credentials = new NetworkCredential("uftp", "uftp2017");// ftp用户名和密码  
          reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行  
          reqFTP.Method = WebRequestMethods.Ftp.UploadFile;// 指定执行什么命令  
          reqFTP.UseBinary = true;// 指定数据传输类型  
          reqFTP.UsePassive = false;
          reqFTP.ContentLength = fileInf.Length;// 上传文件时通知服务器文件的大小  
          int buffLength = 2048;// 缓冲大小设置为2kb  
          byte[] buff = new byte[buffLength];
          int contentLen;

          // 打开一个文件流 (System.IO.FileStream) 去读上传的文件  
          FileStream fs = fileInf.OpenRead();
          try
          {
              //FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
              //responseStream = response.GetResponseStream();
              Stream responseStream = reqFTP.GetRequestStream();// 把上传的文件写入流  
              //获取请求的响应流
              //responseStream = response.GetResponseStream();

              contentLen = fs.Read(buff, 0, buffLength);// 每次读文件流的2kb  

              while (contentLen != 0)// 流内容没有结束  
              {
                  // 把内容从file stream 写入 upload stream  
                  responseStream.Write(buff, 0, contentLen);
                  contentLen = fs.Read(buff, 0, buffLength);
              }
              // 关闭两个流  
              responseStream.Close();
              fs.Close();
              //Response.Write("文件【" + ftpPath + "/" + fileInf.Name + "】上传成功！<br/>");
          }
          catch (Exception ex)
          {
              //Response.Write("上传文件【" + ftpPath + "/" + fileInf.Name + "】时，发生错误：" + ex.Message + "<br/>");
          }
      }


        #endregion
       
        #region Functions
        /// <summary>
        /// Sets up the FTPClient for this Form.  Called from frmLogin.
        /// </summary>
        /// <param name="client">FTPclient on frmLogin is used to refrence the FtpClient here.</param>
        public void SetFtpClient(Win7FTP.Library.FTPclient client)
        {

            //Set FtpClient
            FtpClient = client;
            //downloadtest();
            //writeFile();
            //FtpClient.uploadtest();
            //upLoadFileFtp(FtpClient, "ftp://120.77.249.136/test.txt");
            //Display the Welcome Message
            //Message = new ListViewItem();
            //Message.Text = DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString();
            //Message.SubItems.Add("欢迎消息");
            //Message.SubItems.Add(FtpClient.WelcomeMessage);
            //Message.SubItems.Add("无代码");
            //Message.SubItems.Add("/");
            //lstMessages.Items.Add(Message);

            //Setup OnMessageReceived Event
            FtpClient.OnNewMessageReceived += new FTPclient.NewMessageHandler(FtpClient_OnNewMessageReceived);
            //复制集合
            List<FTPfileInfo> testlistfolders = new List<FTPfileInfo> { };
            //Open and Display Root Directory and Files/Folders in it
            foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail("/"))
            {
                listfolders.Add(folder);
                testlistfolders.Add(folder);
             }

            //转换成int类型字符串
            List<Int32> intlist =new List<Int32>{};

            //对listfolders文件夹集合按照名称（数字大小）进行排序
            for (int i = 0; i < listfolders.Count; i++) {
                //判断文件名是否包含字母
                if (SeparateAAame(listfolders[i].Filename))
                {
                    //intlist.Add(int.Parse(listfolders[i].Filename));
                    intlist.Add(int.Parse(listfolders[i].Filename));
                }
            }


            //对int集合进行排序

            for (int a = 0; a < intlist.Count - 1; a++)
            {
                for (int b = 0; b < intlist.Count - 1; b++)
                {
                    if (intlist[b] > intlist[b + 1])
                    {//交换
                        intlist[b] = intlist[b] + intlist[b + 1];
                        intlist[b + 1] = intlist[b] - intlist[b + 1];
                        intlist[b] = intlist[b] - intlist[b + 1];
                    }
                }
            }
            //输出int集合
            for (int i = 0; i < intlist.Count; i++)
            {
                //Console.WriteLine("test:" + intlist[i]);
            }

            //重排listfolders集合
            listfolders.Clear();
            for (int i = 0; i < testlistfolders.Count; i++)
            {
                //判断文件名是否包含字母
                if (SeparateAAame(testlistfolders[i].Filename))
                {
                    //intlist.Add(int.Parse(listfolders[i].Filename));//只有一个1
                    for (int a = 0; a < testlistfolders.Count; a++)
                    {
                        if (i < intlist.Count) {
                            if ((testlistfolders[a].Filename).Equals(intlist[i].ToString()))
                            {
                                listfolders.Add(testlistfolders[a]);
                                //a = testlistfolders.Count;
                            }
                        }
                    
                    }
                        
                }
            }

            //输出listfolders集合
            for (int i = 0; i < listfolders.Count; i++)
            {
                //Console.WriteLine("test:" + listfolders[i].Filename);
            }


            //遍历listfolders集合，添加数据进listview，并显示
            lstRemoteSiteFiles.BeginUpdate();
            //workThread = new Thread(delegate()
            //{
                for (int i = 0; i < listfolders.Count; i++)
                {
                    ListViewItem item = new ListViewItem();
                    //item.Text = NameSeparatename(filename);
                    for (int a = 0; a < LineList.Count; a++)
                    {
                        if ((Separatenumber(LineList[a])).Equals(listfolders[i].Filename))
                        {
                            item.Text = Separatename(LineList[a]);
                            a = LineList.Count;
                        }
                        else
                        {
                            item.Text = listfolders[i].Filename;
                        }
                    }
                    if (listfolders[i].FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                        item.SubItems.Add("文件夹");
                    else
                        item.SubItems.Add("文件");

                    //item.SubItems.Add(listfolders[i].FullNametest);
                    item.SubItems.Add(listfolders[i].Filename);//
                    item.SubItems.Add(listfolders[i].FileDateTime.ToShortDateString() + " " + listfolders[i].FileDateTime.ToShortTimeString());
                    item.SubItems.Add(GetFileSize(listfolders[i].Size));

                    for (int b = 0; b < LinesCounts.Count; b++)
                    {
                        //判断是否在线
                        if (listfolders[i].Filename.Equals(LinesCounts[b]))
                        {
                            item.SubItems.Add("在线");
                            b = LinesCounts.Count;
                        }
                        else
                        {
                            if (b == LinesCounts.Count - 1)
                            {
                                item.SubItems.Add("不在线");
                            }
                            //item.SubItems.Add("");
                        }
                    }

                    for (int c = 0; c < LineList.Count; c++)
                    {
                        //添加详细信息
                        if (listfolders[i].Filename.Equals(Separatenumber(LineList[c])))
                        {
                            item.SubItems.Add(SeparateMessage(LineList[c]));
                            c = LineList.Count;
                        }
                        else
                        {
                            if (c == LineList.Count - 1)
                            {
                                item.SubItems.Add("");
                            }
                            //item.SubItems.Add("不在线");
                        }
                    }
                    lstRemoteSiteFiles.Items.Add(item);

                }
                lstRemoteSiteFiles.EndUpdate();

            //});
            //workThread.Start();
            
    }



        //判断字符串是否包含字母
        public bool SeparateAAame(string name)
        {
            char[] ch = name.ToCharArray();
            for (int i = 0; i < ch.Length; i++)
            {
                if (Char.IsLetter(ch[i]))
                {
                    return false;
                }
                else {
                    if (i == ch.Length - 1) {
                        return true;
                    }
                }
            }
            return true;
        }

        //获取编号
        public String Separatenumber(string name)
        {
            int index = name.IndexOf(",");
            //编号
            string bb = name.Substring(0, index);
            //Console.WriteLine(bb);
            return bb;

        }

        //名字
        public String Separatename(string name)
        {
            int index = name.IndexOf(",");
            int counts;
            string cc;
            if (name.Contains(":"))
            {
                counts = name.IndexOf(":");
                cc = name.Substring(index + 1, counts - index - 1);
            }
            else
            {
                cc = name.Substring(index + 1);
            }
            return cc;

        }
        //详细信息
        public String SeparateMessage(string name)
        {

            int counts;
            string cc;
            if (name.Contains(":"))
            {
                counts = name.IndexOf(":");
                cc = name.Substring(counts + 1);
            }
            else
            {
                cc = "";
            }
            //名字
            //Console.WriteLine(cc);
            return cc;
        }
        /// <summary>
        /// Reload all Directories and Files in Current Directory
        /// </summary>
        private void RefreshDirectory()
        {
            //Clear all items
            lstRemoteSiteFiles.Items.Clear();
            if (!txtRemoteDirectory.Text.Equals("/")){
                //不是在根目录
                lstRemoteSiteFiles.BeginUpdate();
               foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail(txtRemoteDirectory.Text))
            {
                ListViewItem item = new ListViewItem();
                if (!folder.Filename.Equals("test.txt"))
                {
                    for (int i = 0; i < LineList.Count; i++)
                    {
                        if ((Separatenumber(LineList[i])).Equals(folder.Filename))
                        {
                            item.Text = Separatename(LineList[i]);
                            i = LineList.Count;
                        }
                        else
                        {
                            item.Text = folder.Filename;
                        }
                    }

                    //NameSeparatename(filename.Filename);
                    if (folder.FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                        item.SubItems.Add("文件夹");
                    else
                        item.SubItems.Add("文件");


                    item.SubItems.Add(folder.Filename);
                    //item.SubItems.Add(folder.Permission);
                    item.SubItems.Add(folder.Filename);//
                    item.SubItems.Add(folder.FileDateTime.ToShortDateString() + " " + folder.FileDateTime.ToShortTimeString());
                    item.SubItems.Add(GetFileSize(folder.Size));
                    for (int i = 0; i < LinesCounts.Count; i++)
                    {
                        //判断是否在线
                        if (folder.Filename.Equals(LinesCounts[i]))
                        {
                            item.SubItems.Add("在线");
                            i = LinesCounts.Count;
                        }
                        else
                        {
                            if (i == LinesCounts.Count - 1)
                            {
                                item.SubItems.Add("不在线");
                            }
                            //item.SubItems.Add("");
                        }
                    }

                    for (int i = 0; i < LineList.Count; i++)
                    {
                        //添加详细信息
                        if (folder.Filename.Equals(Separatenumber(LineList[i])))
                        {
                            item.SubItems.Add(SeparateMessage(LineList[i]));
                            i = LineList.Count;
                        }
                        else
                        {
                            if (i == LineList.Count - 1)
                            {
                                item.SubItems.Add("");
                            }
                            //item.SubItems.Add("不在线");
                            }
                        }
                        lstRemoteSiteFiles.Items.Add(item);
                    }
                }
               lstRemoteSiteFiles.EndUpdate();
            }else { 
                //根目录

             List<FTPfileInfo> listfo= new List<FTPfileInfo> { };
            List<FTPfileInfo> testlistfolders = new List<FTPfileInfo> { };
            foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail(txtRemoteDirectory.Text))
            {
                listfo.Add(folder);
                testlistfolders.Add(folder);
            }

            //转换成int类型字符串
            List<Int32> intlist = new List<Int32> { };

            //对listfolders文件夹集合按照名称（数字大小）进行排序
            for (int i = 0; i < listfo.Count; i++)
            {
                //判断文件名是否包含字母
                if (SeparateAAame(listfo[i].Filename))
                {
                    //intlist.Add(int.Parse(listfolders[i].Filename));
                    intlist.Add(int.Parse(listfo[i].Filename));
                }
            }


            //对int集合进行排序

            for (int a = 0; a < intlist.Count - 1; a++)
            {
                for (int b = 0; b < intlist.Count - 1; b++)
                {
                    if (intlist[b] > intlist[b + 1])
                    {//交换
                        intlist[b] = intlist[b] + intlist[b + 1];
                        intlist[b + 1] = intlist[b] - intlist[b + 1];
                        intlist[b] = intlist[b] - intlist[b + 1];
                    }
                }
            }
            //输出int集合
            for (int i = 0; i < intlist.Count; i++)
            {
                //Console.WriteLine("test:" + intlist[i]);
            }

            //重排listfolders集合
            listfo.Clear();
            for (int i = 0; i < testlistfolders.Count; i++)
            {
                //判断文件名是否包含字母
                if (SeparateAAame(testlistfolders[i].Filename))
                {
                    //intlist.Add(int.Parse(listfolders[i].Filename));//只有一个1
                    for (int a = 0; a < testlistfolders.Count; a++)
                    {
                        if (i < intlist.Count)
                        {
                            if ((testlistfolders[a].Filename).Equals(intlist[i].ToString()))
                            {
                                listfo.Add(testlistfolders[a]);
                                //a = testlistfolders.Count;
                            }
                        }

                    }

                }
            }

            //输出listfolders集合
            for (int i = 0; i < listfo.Count; i++)
            {
                //Console.WriteLine("test:" + listfo[i].Filename);
            }



            //遍历listfolders集合，添加数据进listview，并显示
            //   workThread = new Thread(delegate()
            //{
                lstRemoteSiteFiles.BeginUpdate();
                for (int i = 0; i < listfo.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                //item.Text = NameSeparatename(filename);
                for (int a = 0; a < LineList.Count; a++)
                {
                    if ((Separatenumber(LineList[a])).Equals(listfo[i].Filename))
                    {
                        item.Text = Separatename(LineList[a]);
                        a = LineList.Count;
                    }
                    else
                    {
                        item.Text = listfo[i].Filename;
                    }
                }
                if (listfo[i].FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                    item.SubItems.Add("文件夹");
                else
                    item.SubItems.Add("文件");

                //item.SubItems.Add(listfolders[i].FullNametest);
                item.SubItems.Add(listfo[i].Filename);//
                item.SubItems.Add(listfo[i].FileDateTime.ToShortDateString() + " " + listfo[i].FileDateTime.ToShortTimeString());
                item.SubItems.Add(GetFileSize(listfo[i].Size));

                for (int b = 0; b < LinesCounts.Count; b++)
                {
                    //判断是否在线
                    if (listfo[i].Filename.Equals(LinesCounts[b]))
                    {
                        item.SubItems.Add("在线");
                        b = LinesCounts.Count;
                    }
                    else
                    {
                        if (b == LinesCounts.Count - 1)
                        {
                            item.SubItems.Add("不在线");
                        }
                        //item.SubItems.Add("");
                    }
                }

                for (int c = 0; c < LineList.Count; c++)
                {
                    //添加详细信息
                    if (listfo[i].Filename.Equals(Separatenumber(LineList[c])))
                    {
                        item.SubItems.Add(SeparateMessage(LineList[c]));
                        c = LineList.Count;
                    }
                    else
                    {
                        if (c == LineList.Count - 1)
                        {
                            item.SubItems.Add("");
                        }
                        //item.SubItems.Add("不在线");
                    }
                }
                lstRemoteSiteFiles.Items.Add(item);
            }
            //});
               //workThread.Start();

               lstRemoteSiteFiles.EndUpdate();

            }
            ////Open and Display Root Directory
            //foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail(txtRemoteDirectory.Text))
            //{
            //    ListViewItem item = new ListViewItem();
            //    if (!folder.Filename.Equals("test.txt"))
            //    {
            //        for (int i = 0; i < LineList.Count; i++)
            //        {
            //            if ((Separatenumber(LineList[i])).Equals(folder.Filename))
            //            {
            //                item.Text = Separatename(LineList[i]);
            //                i = LineList.Count;
            //            }
            //            else
            //            {
            //                item.Text = folder.Filename;
            //            }
            //        }
           
         }

        #region Download File
        /// <summary>
        /// Download File: When the Download ToolStripButton is clicked. Displays a SaveFileDialog.
        /// </summary>
        /// <param name="FileName">Name of the File to Download</param>
        /// <param name="CurrentDirectory">CurrentDirectory (Directory from which to download on server)</param>
        private void DownloadFile(string FileName, string CurrentDirectory)
        {
            //Setup and Show 
            CommonSaveFileDialog SaveDialog = new CommonSaveFileDialog("保存文件到...");
            SaveDialog.AddToMostRecentlyUsedList = true;
            SaveDialog.DefaultFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            SaveDialog.DefaultFileName = FileName;
            if (SaveDialog.ShowDialog() == CommonFileDialogResult.OK)
            {
                //Setup and Open Download Form
                frmDownload DownloadForm = new frmDownload(FileName, CurrentDirectory, SaveDialog.FileName, FtpClient);
            }
            else
                TaskDialog.Show("下载取消.");        //Notify user that Download has been cancelled.
        }

        /// <summary>
        /// Download File: When File is dragged from ListView to the tvFileSystem.  No SaveAsDialog/SavePath is the selected node in
        /// tvFileSystem
        /// </summary>
        /// <param name="FileName">Name of File to Download</param>
        /// <param name="CurrentDirectory">CurrentDirectory (Directory from which to download on server)</param>
        /// <param name="SavePath">Path where file will be downloaded.</param>
        private void DownloadFile(string FileName, string CurrentDirectory, string SavePath)
        {
            //Setup and Open Download Form
            if (SavePath.EndsWith("\\"))
            {
                frmDownload DownloadForm = new frmDownload(FileName, CurrentDirectory, SavePath + FileName, FtpClient);
            }
            else
            {
                //Setup and Open Download Form
                frmDownload DownloadForm = new frmDownload(FileName, CurrentDirectory, SavePath + "\\" + FileName, FtpClient);
            }
        }
        #endregion

        //Code Below Converts Bytes to KB, MB, GB, or just Bytes.  Makes the App more look :)
        //Obtained from: http://www.freevbcode.com/ShowCode.Asp?ID=1971
        private string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";

            return size;
        }
        #endregion

        #region Events
        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                // navigate to specific folder
                DirNode = new TreeNode();
                //tvFileSystem.ShowFolder(txtLocalFolderName.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }
        }

        private void btnOpenDialog_Click(object sender, EventArgs e)
        {
            //Setup the Open Dialog
            objOpenDialog = new CommonOpenFileDialog("选择文件");
            objOpenDialog.IsFolderPicker = true;
            objOpenDialog.Multiselect = false;
            objOpenDialog.AddToMostRecentlyUsedList = true;
            objOpenDialog.AllowNonFileSystemItems = false;
            objOpenDialog.EnsurePathExists = true;

            //Show Dialog
            if (objOpenDialog.ShowDialog() == CommonFileDialogResult.OK)
            {
                //txtLocalFolderName.Text = objOpenDialog.FileName;
                try
                {
                    // navigate to specific folder on tvFileSystem
                    DirNode = new TreeNode();
                    //tvFileSystem.ShowFolder(txtLocalFolderName.Text);
                }
                catch (Exception ex)
                {
                    //Display Error
                    MessageBox.Show("错误: " + ex.Message);
                }
            }

            //Dispose it!
            objOpenDialog.Dispose();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            //Resizing the controls will keep the form looking good and neat.
            //Keep both GroupBoxes even on the Form
            //gbFileSystem.Size = new Size(this.Size.Width / 2, gbFileSystem.Height);
            //gbRemoteSite.Size = gbFileSystem.Size;

            ////Resize txtFolderName so it doesn't get hidden if form is too small
            //txtLocalFolderName.Size = new Size(toolStrip1.Size.Width - 115, txtLocalFolderName.Size.Height);
            //txtRemoteDirectory.Size = txtLocalFolderName.Size;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Keep both GroupBoxes even on the Form
            //gbFileSystem.Size = new Size(this.Size.Width / 2, gbFileSystem.Height);
            //gbRemoteSite.Size = gbFileSystem.Size;
            //tvFileSystem.AllowDrop = true;
        }

        private void FtpClient_OnNewMessageReceived(object myObject, NewMessageEventArgs e)
        {
            //Display Meesage in lstMessages
            //Message = new ListViewItem();
            //Message.Text = DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString();
            //Message.SubItems.Add(e.StatusType);
            //Message.SubItems.Add(e.StatusMessage);
            ////Message.SubItems.Add(e.StatusCode);
            //Message.SubItems.Add(txtRemoteDirectory.Text);
            //lstMessages.Items.Add(Message);

            //this.lstMessages.EnsureVisible(this.lstMessages.Items.Count - 1);
        }

        //Fires when the Selected File/Folder is changed in lstRemoteFiles.
        private void lstRemoteSiteFiles_SelectedValueChanged(object sender, EventArgs e)
        {
            //Check if there is a file; if there isn't select the first one.
            //All this comes under Try,Catch.  In case, there isn't a file or something.
            try
            {
                if (lstRemoteSiteFiles.SelectedItems[0] == null)
                {
                    lstRemoteSiteFiles.Items[0].Selected = true;
                }
            }
            catch
            {
                return;
            }

            //If the Selected Item is a File, then we want its related buttons to be enables
            if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "文件")
            {
                //Enable the buttons that have to do with the FILE
                btnRename.Enabled = true;
                btnDownload.Enabled = true;
            }
            else if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "文件夹") // Its a Directory, Disable buttons
            {
                //Disable the buttons that have nothing to do with the FOLDER
                //btnRename.Enabled = false;
                btnDownload.Enabled = false;

                btnRename.Enabled = true;
            }
        }

        private void btnRemoteBack_Click(object sender, EventArgs e)
        {
            // Locate the Last "/"
            if (txtRemoteDirectory.Text != "/")
            {
                //The below code works fine, even though it repeats.  I'll explain why it repeats:
                //1) If we have "/Dire1/Drect2/" we first take out the extra "/" and then we do it again to remove the other directory name
                //2) I didn't really test much of it...after I got it to work
                //3) If you can get it to work in a more efficient way than below, then ur most welcome to fix it!
                int endTagStartPosition = txtRemoteDirectory.Text.LastIndexOf("/");
                txtRemoteDirectory.Text = txtRemoteDirectory.Text.Substring(0, endTagStartPosition);
                int endTagStartPosition1 = txtRemoteDirectory.Text.LastIndexOf("/");
                txtRemoteDirectory.Text = txtRemoteDirectory.Text.Substring(0, endTagStartPosition1);

                //If there is "/" that means that we are at root and we don't need "//",
                //if not, then we add "/" at the end of the directory, so our above code
                //works w/o errors
                if (txtRemoteDirectory.Text != "/")
                    txtRemoteDirectory.Text += "/";

                //Empty lstRemoteFiles
                lstRemoteSiteFiles.Items.Clear();
                //Set Current Directory
                FtpClient.CurrentDirectory = txtRemoteDirectory.Text;

                ////Get Files and Folders from Selected Direcotry
                //foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail(txtRemoteDirectory.Text))
                //{
                //    ListViewItem item = new ListViewItem();
                //    if (!folder.Filename.Equals("test.txt"))
                //    {
                //        for (int i = 0; i < LineList.Count; i++)
                //        {
                //            if ((Separatenumber(LineList[i])).Equals(folder.Filename))
                //            {
                //                item.Text = Separatename(LineList[i]);
                //                i = LineList.Count;
                //            }
                //            else
                //            {
                //                item.Text = folder.Filename;
                //            }
                //        }

                //        //NameSeparatename(filename.Filename);
                //        if (folder.FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                //            item.SubItems.Add("文件夹");
                //        else
                //            item.SubItems.Add("文件");


                //        item.SubItems.Add(folder.Filename);
                //        //item.SubItems.Add(folder.Permission);
                //        item.SubItems.Add(folder.Filename);//
                //        item.SubItems.Add(folder.FileDateTime.ToShortTimeString() + folder.FileDateTime.ToShortDateString());
                //        item.SubItems.Add(GetFileSize(folder.Size));
                //        for (int i = 0; i < LinesCounts.Count; i++)
                //        {
                //            //判断是否在线
                //            if (folder.Filename.Equals(LinesCounts[i]))
                //            {
                //                item.SubItems.Add("在线");
                //                i = LinesCounts.Count;
                //            }
                //            else
                //            {
                //                if (i == LinesCounts.Count - 1)
                //                {
                //                    item.SubItems.Add("不在线");
                //                }
                //                //item.SubItems.Add("");
                //            }
                //        }

                //        for (int i = 0; i < LineList.Count; i++)
                //        {
                //            //添加详细信息
                //            if (folder.Filename.Equals(Separatenumber(LineList[i])))
                //            {
                //                item.SubItems.Add(SeparateMessage(LineList[i]));
                //                i = LineList.Count;
                //            }
                //            else
                //            {
                //                if (i == LineList.Count - 1)
                //                {
                //                    item.SubItems.Add("");
                //                }
                //                //item.SubItems.Add("不在线");
                //            }
                //        }
                //        lstRemoteSiteFiles.Items.Add(item);
                //    }
                //}
                //遍历listfolders集合，添加数据进listview，并显示
                List<FTPfileInfo> listfo = new List<FTPfileInfo> { };
                List<FTPfileInfo> testlistfolders = new List<FTPfileInfo> { };
                foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail(txtRemoteDirectory.Text))
                {
                    listfo.Add(folder);
                    testlistfolders.Add(folder);
                }

                //转换成int类型字符串
                List<Int32> intlist = new List<Int32> { };

                //对listfolders文件夹集合按照名称（数字大小）进行排序
                for (int i = 0; i < listfo.Count; i++)
                {
                    //判断文件名是否包含字母
                    if (SeparateAAame(listfo[i].Filename))
                    {
                        //intlist.Add(int.Parse(listfolders[i].Filename));
                        intlist.Add(int.Parse(listfo[i].Filename));
                    }
                }


                //对int集合进行排序

                for (int a = 0; a < intlist.Count - 1; a++)
                {
                    for (int b = 0; b < intlist.Count - 1; b++)
                    {
                        if (intlist[b] > intlist[b + 1])
                        {//交换
                            intlist[b] = intlist[b] + intlist[b + 1];
                            intlist[b + 1] = intlist[b] - intlist[b + 1];
                            intlist[b] = intlist[b] - intlist[b + 1];
                        }
                    }
                }
                //输出int集合
                for (int i = 0; i < intlist.Count; i++)
                {
                    //Console.WriteLine("test:" + intlist[i]);
                }

                //重排listfolders集合
                listfo.Clear();
                for (int i = 0; i < testlistfolders.Count; i++)
                {
                    //判断文件名是否包含字母
                    if (SeparateAAame(testlistfolders[i].Filename))
                    {
                        //intlist.Add(int.Parse(listfolders[i].Filename));//只有一个1
                        for (int a = 0; a < testlistfolders.Count; a++)
                        {
                            if (i < intlist.Count)
                            {
                                if ((testlistfolders[a].Filename).Equals(intlist[i].ToString()))
                                {
                                    listfo.Add(testlistfolders[a]);
                                    //a = testlistfolders.Count;
                                }
                            }

                        }

                    }
                }

                //输出listfolders集合
                for (int i = 0; i < listfo.Count; i++)
                {
                    //Console.WriteLine("test:" + listfo[i].Filename);
                }

                //遍历listfolders集合，添加数据进listview，并显示
                lstRemoteSiteFiles.BeginUpdate();
                //workThread = new Thread(delegate()
                //{
                    for (int i = 0; i < listfo.Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        //item.Text = NameSeparatename(filename);
                        for (int a = 0; a < LineList.Count; a++)
                        {
                            if ((Separatenumber(LineList[a])).Equals(listfo[i].Filename))
                            {
                                item.Text = Separatename(LineList[a]);
                                a = LineList.Count;
                            }
                            else
                            {
                                item.Text = listfo[i].Filename;
                            }
                        }
                        if (listfo[i].FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                            item.SubItems.Add("文件夹");
                        else
                            item.SubItems.Add("文件");

                        //item.SubItems.Add(listfolders[i].FullNametest);
                        item.SubItems.Add(listfo[i].Filename);//
                        item.SubItems.Add(listfo[i].FileDateTime.ToShortDateString() + " " + listfo[i].FileDateTime.ToShortTimeString());
                        item.SubItems.Add(GetFileSize(listfo[i].Size));

                        for (int b = 0; b < LinesCounts.Count; b++)
                        {
                            //判断是否在线
                            if (listfo[i].Filename.Equals(LinesCounts[b]))
                            {
                                item.SubItems.Add("在线");
                                b = LinesCounts.Count;
                            }
                            else
                            {
                                if (b == LinesCounts.Count - 1)
                                {
                                    item.SubItems.Add("不在线");
                                }
                                //item.SubItems.Add("");
                            }
                        }

                        for (int c = 0; c < LineList.Count; c++)
                        {
                            //添加详细信息
                            if (listfo[i].Filename.Equals(Separatenumber(LineList[c])))
                            {
                                item.SubItems.Add(SeparateMessage(LineList[c]));
                                c = LineList.Count;
                            }
                            else
                            {
                                if (c == LineList.Count - 1)
                                {
                                    item.SubItems.Add("");
                                }
                                //item.SubItems.Add("不在线");
                            }
                        }
                        lstRemoteSiteFiles.Items.Add(item);
                    }
                    lstRemoteSiteFiles.EndUpdate();
                //});
                //workThread.Start();

            }
        }

        private void txtLocalFolderName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    // navigate to specific folder
                    DirNode = new TreeNode();
                    //tvFileSystem.ShowFolder(txtLocalFolderName.Text);
                    //PopulateTree(txtLocalFolderName.Text, DirNode);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误: " + ex.Message);
                }
            }
        }

        //文件夹重命名按键
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRename frmrename = new frmRename();
            string tempfileName =null;
            try
            {
                if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "文件")
                {
                    string extension = lstRemoteSiteFiles.SelectedItems[0].Text.Substring(lstRemoteSiteFiles.SelectedItems[0].Text.LastIndexOf("."));
                    //Create a RenameDialog and display it.
                    RenameDialog = new frmRename(lstRemoteSiteFiles.SelectedItems[0].Text, txtRemoteDirectory.Text, this, extension);
                    RenameDialog.ShowDialog(this);

                }
                else{
                 //说明是文件夹


                    //if (frmrename.ShowDialog() == DialogResult.OK)
                    //{

                    for (int i = 0; i < LineList.Count; i++)
                    {
                        if ((Separatename(LineList[i])).Equals(lstRemoteSiteFiles.SelectedItems[0].Text))
                        {
                            //item.Text = NameSeparatenumber(LineList[i]);
                            tempfileName = Separatenumber(LineList[i]);
                            i = LineList.Count;
                        }
                        else
                        {
                            tempfileName = lstRemoteSiteFiles.SelectedItems[0].Text;
                            //item.Text = folder.Filename;
                        }
                    }



                    RenameDialog = new frmRename(tempfileName, txtRemoteDirectory.Text, this, "");
                    RenameDialog.ShowDialog(this);


                    //找到他的备注信息，添加进来
                    for (int i = 0; i < LineList.Count; i++)
                    {
                        //判断当前文件里面是否含有相同编号的信息，如果有，删除，并且重新添加新的信息
                        if (Separatenumber(Program.LoginFrmCmbValue).Equals(Separatenumber(LineList[i])))
                        {
                            Program.LoginFrmCmbValue = Program.LoginFrmCmbValue + ":" + SeparateMessage(LineList[i]);
                            //LineList.Remove(LineList[i]);
                            //name = name + "\r\n" + LineList[i];
                            //i = LineList.Count;
                        }
                    }

                    writefile(Program.LoginFrmCmbValue);
                    //Refresh, because the Filename has been changed by the user.
                    //RefreshDirectory();
                }
                RefreshDirectory();
            }
              
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstRemoteSiteFiles.SelectedItems[0] != null)
            {
                //Check if File or Folder
                if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "文件")
                {
                    try
                    {
                        //Delete the FILE
                        //FtpClient.FtpDelete(lstRemoteSiteFiles.SelectedItems[0].Text);
                        deleteFiles("ftp://120.77.249.136" + txtRemoteDirectory.Text + lstRemoteSiteFiles.SelectedItems[0].Text);
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("删除文件出错.  错误信息: " + ex.Message);
                    }
                }
                else
               //删除整个文件夹
                {

                    try
                    {
                        //Delete the FOLDER
                        //FtpClient.FtpDeleteDirectory(txtRemoteDirectory.Text+lstRemoteSiteFiles.SelectedItems[0].SubItems[2].Text);




                        deleteFilestest("ftp://120.77.249.136" + txtRemoteDirectory.Text + lstRemoteSiteFiles.SelectedItems[0].SubItems[2].Text);
                        //String FtpPath = "ftp://120.77.249.136" + lstRemoteSiteFiles.SelectedItems[0].SubItems[2].Text;
                        //FtpWebRequest reqFTP;
                        //string result ;
                        //try
                        //{
                        //    //string ui = (ftpPath + dirName).Trim();
                        //    reqFTP = (FtpWebRequest)FtpWebRequest.Create(FtpPath);
                        //    reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                        //    reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                        //    reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行  
                        //    reqFTP.UsePassive = false;//设置不被动模式访问
                        //    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                        //    Stream ftpStream = response.GetResponseStream();
                        //    StreamReader sr = new StreamReader(ftpStream);
                        //    result = sr.ReadToEnd();
                        //    sr.Close();
                        //    response.Close();
                        //    ftpStream.Close();
                        //    response.Close();
                        //    //System.Diagnostics.Debug.WriteLine("文件夹【" + dirName + "】创建成功！<br/>");
                        //    //Response.Write("文件夹【" + dirName + "】创建成功！<br/>");
                        //}

                        //catch (Exception ex)
                        //{
                        //    System.Diagnostics.Debug.WriteLine("新建文件夹【"  + "】时，发生错误：" + ex.Message);
                        //    //Response.Write("新建文件夹【" + dirName + "】时，发生错误：" + ex.Message);
                        //}

                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("删除文件出错.  错误信息: " + ex.Message);
                    }
                }

                RefreshDirectory();
            }
        }

        //删除所有文件夹
        public void deleteFilestest(String url)
        {
            FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(url);
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            listRequest.Credentials = new NetworkCredential("uftp", "uftp2017");
            listRequest.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行
            listRequest.UseBinary = true;// 指定数据传输类型  
            listRequest.UsePassive = false;
            List<string> lines = new List<string>();

            using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
            using (Stream listStream = listResponse.GetResponseStream())
            using (StreamReader listReader = new StreamReader(listStream))
            {
                while (!listReader.EndOfStream)
                {
                    lines.Add(listReader.ReadLine());
                }
            }

            foreach (string line in lines)
            {
                string[] tokens = line.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                string name = tokens[8];
                string permissions = tokens[0];

                string fileUrl = url + "/" + name;

                if (permissions[0] == 'd')
                {
                    deleteFilestest(fileUrl + "/");
                }
                else
                {
                    FtpWebRequest deleteRequest = (FtpWebRequest)WebRequest.Create(fileUrl);
                    deleteRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                    deleteRequest.Credentials = new NetworkCredential("uftp", "uftp2017");
                    deleteRequest.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行
                    deleteRequest.UseBinary = true;// 指定数据传输类型  
                    deleteRequest.UsePassive = false;
                    deleteRequest.GetResponse();
                }
            }
            try{
                FtpWebRequest removeRequest = (FtpWebRequest)WebRequest.Create(url);
                removeRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
                removeRequest.Credentials = new NetworkCredential("uftp", "uftp2017");
                removeRequest.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行
                removeRequest.UseBinary = true;// 指定数据传输类型  
                removeRequest.UsePassive = false;
                removeRequest.GetResponse();
            }catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                System.Diagnostics.Debug.WriteLine("删除文件出错：" + ex.ToString());
            }
        }


        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Clear the Progress
            TaskBarManager.ClearProgressValue();

            try
            {
                if (lstRemoteSiteFiles.SelectedItems[0] != null)
                {
                    if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "文件")
                    {
                        //Download File
                        DownloadFile(lstRemoteSiteFiles.SelectedItems[0].Text, FtpClient.CurrentDirectory);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //新建主机按钮点击事件
        private void btnNewDir_Click(object sender, EventArgs e)
        {
            try
            {
                //New instance of frmNewFolder
                frmNewFolder NewFolderForm = new frmNewFolder();
                if (NewFolderForm.ShowDialog() == DialogResult.OK)
                {
                    //Create New Directory
                    filename = NewFolderForm.NewDirName;
                    writefile(NewFolderForm.NewDirName);
                    //TaskDialog.Show("创建文件出错.  错误信息:  " + NameSeparatename(filename));
                    //创建文件
                    FtpClient.FtpCreateDirectory(FtpClient.CurrentDirectory + Separatenumber(NewFolderForm.NewDirName));
                    //Refresh Current Directory to view the newly created directory
                    RefreshDirectory();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("创建文件出错.  错误信息:  " + ex.Message);
            }
        }

        #region Drag & Drop to FileSystem from Remote Server
        private void lstRemoteSiteFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lstRemoteSiteFiles.DoDragDrop(e.Item, DragDropEffects.Copy);
        }

        private void tvFileSystem_DragEnter(object sender, DragEventArgs e)
        {
            // this code can be in DragOver also
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                ListViewItem li = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                //Allow a FIle to be compied, no folders.
                if (li.SubItems[1].Text == "文件")
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void tvFileSystem_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                ListViewItem li = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

                try
                {
                    //Double Check if its a file....Just in case :)
                    if (li.SubItems[1].Text == "文件")
                    {
                        //Point pos = tvFileSystem.PointToClient(new Point(e.X, e.Y));
                        //TreeNode targetNode = tvFileSystem.GetNodeAt(pos);
                        //if (targetNode != null)
                        //{
                            //Set SelectedNode
                            //TreeNodePath SelectedNode = targetNode as TreeNodePath;
                            ////Ask User if he/she wants to save
                            //DialogResult DownloadConfirm = MessageBox.Show("你想保存 " + li.Text + " 到 " + SelectedNode.Path + "?", "下载文件?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            ////DialogResult
                            //if (DownloadConfirm == DialogResult.Yes)
                            //{
                            //    //Download File if DownloadResult = YES
                            //    DownloadFile(li.Text, FtpClient.CurrentDirectory, SelectedNode.Path);
                            //}
                        //}
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void tvFileSystem_DragOver(object sender, DragEventArgs e)
        {
            // this code can be in DragOver also
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                ListViewItem li = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                if (li.SubItems[1].Text == "文件")
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                    e.Effect = DragDropEffects.None;

            }
        }
        #endregion

        private void lstRemoteSiteFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstRemoteSiteFiles.Items.Count != 0)
            {
                try
                {
                    if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "文件")
                    {
                        //Enable the buttons that are related to the FILE
                        btnRename.Enabled = true;
                        btnDownload.Enabled = true;
                        //Set Current Directory for Download
                        FtpClient.CurrentDirectory = txtRemoteDirectory.Text;
                        //Its a File, so Ask them if they want to Save it...
                        if (MessageBox.Show("你想保存这个文件: " + txtRemoteDirectory.Text + lstRemoteSiteFiles.SelectedItems[0].Text + "/" + "?", "下载文件?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            //Save the File to location
                            downloadToolStripMenuItem_Click(this, e);
                        }
                    }
                    else if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "文件夹") // Its a Directory
                    {
                        //Set Directory to txtRemoteDirectory.Text + selectedItem + "/"
                        //Result - /SelectedDirecotory/  -- good for navigation, keeping user informed and code :)





                        //txtRemoteDirectory.Text += lstRemoteSiteFiles.SelectedItems[0].Text + "/";
                        //lstRemoteSiteFiles.Items.Clear();
                        //FtpClient.CurrentDirectory = txtRemoteDirectory.Text;


                        for(int i=0;i<LineList.Count;i++){
                            if ((Separatename(LineList[i])).Equals(lstRemoteSiteFiles.SelectedItems[0].Text))
                            {
                                lstRemoteSiteFiles.SelectedItems[0].Text = Separatenumber(LineList[i]);
                            }    
                        }


                        //Console.WriteLine("lstRemoteSiteFiles.SelectedItems[0].Text " + lstRemoteSiteFiles.SelectedItems[0].Text);
                        txtRemoteDirectory.Text += lstRemoteSiteFiles.SelectedItems[0].Text + "/";
                        lstRemoteSiteFiles.Items.Clear();
                        FtpClient.CurrentDirectory = txtRemoteDirectory.Text;

                        ////Set Current Dir
                        //FtpClient.CurrentDirectory = txtRemoteDirectory.Text;

                        lstRemoteSiteFiles.BeginUpdate();
                        //Get Files and Folders from Selected Direcotry
                        foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail(txtRemoteDirectory.Text))
                        {
                            ListViewItem item = new ListViewItem();
                            if (!folder.Filename.Equals("test.txt"))
                            {
                                //进入文件夹之后的操作
                                for (int i = 0; i < LineList.Count; i++)
                                {
                                    if ((Separatenumber(LineList[i])).Equals(folder.Filename))
                                    {
                                        item.Text = Separatename(LineList[i]);
                                        i = LineList.Count;
                                    }
                                    else
                                    {
                                        item.Text = folder.Filename;
                                    }
                                }
                                //NameSeparatename(filename.Filename);
                                if (folder.FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                                    item.SubItems.Add("文件夹");
                                else
                                    item.SubItems.Add("文件");
                                //item.SubItems.Add(folder.Filename);
                                //item.SubItems.Add(folder.Permission);
                                item.SubItems.Add(folder.Filename);//
                                item.SubItems.Add(folder.FileDateTime.ToShortDateString() + " " + folder.FileDateTime.ToShortTimeString());
                                item.SubItems.Add(GetFileSize(folder.Size));
                                for (int i = 0; i < LinesCounts.Count; i++)
                                {
                                    //判断是否在线
                                    if (folder.Filename.Equals(LinesCounts[i]))
                                    {
                                        item.SubItems.Add("在线");
                                        i = LinesCounts.Count;
                                    }
                                    else
                                    {
                                        if (i == LinesCounts.Count - 1)
                                        {
                                            item.SubItems.Add("不在线");
                                        }
                                        //item.SubItems.Add("");
                                    }
                                }

                                for (int i = 0; i < LineList.Count; i++)
                                {
                                    //添加详细信息
                                    if (folder.Filename.Equals(Separatenumber(LineList[i])))
                                    {
                                        item.SubItems.Add(SeparateMessage(LineList[i]));
                                        i = LineList.Count;
                                    }
                                    else
                                    {
                                        if (i == LineList.Count - 1)
                                        {
                                            item.SubItems.Add("");
                                        }
                                        //item.SubItems.Add("不在线");
                                    }
                                }
                                lstRemoteSiteFiles.Items.Add(item);
                            }
                        }
                        lstRemoteSiteFiles.EndUpdate();
                        //遍历listfolders集合，添加数据进listview，并显示
                        //for (int i = 0; i < listfolders.Count; i++)
                        //{
                        //    ListViewItem item = new ListViewItem();
                        //    //item.Text = NameSeparatename(filename);
                        //    for (int a = 0; a < LineList.Count; a++)
                        //    {
                        //        if ((Separatenumber(LineList[a])).Equals(listfolders[i].Filename))
                        //        {
                        //            item.Text = Separatename(LineList[a]);
                        //            a = LineList.Count;
                        //        }
                        //        else
                        //        {
                        //            item.Text = listfolders[i].Filename;
                        //        }
                        //    }
                        //    if (listfolders[i].FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                        //        item.SubItems.Add("文件夹");
                        //    else
                        //        item.SubItems.Add("文件");

                        //    item.SubItems.Add(listfolders[i].FullNametest);
                        //    item.SubItems.Add(listfolders[i].Filename);//
                        //    item.SubItems.Add(listfolders[i].FileDateTime.ToShortTimeString() + listfolders[i].FileDateTime.ToShortDateString());
                        //    item.SubItems.Add(GetFileSize(listfolders[i].Size));

                        //    for (int b = 0; b < LinesCounts.Count; b++)
                        //    {
                        //        //判断是否在线
                        //        if (listfolders[i].Filename.Equals(LinesCounts[b]))
                        //        {
                        //            item.SubItems.Add("在线");
                        //            b = LinesCounts.Count;
                        //        }
                        //        else
                        //        {
                        //            if (b == LinesCounts.Count - 1)
                        //            {
                        //                item.SubItems.Add("不在线");
                        //            }
                        //            //item.SubItems.Add("");
                        //        }
                        //    }

                        //    for (int c = 0; c < LineList.Count; c++)
                        //    {
                        //        //添加详细信息
                        //        if (listfolders[i].Filename.Equals(Separatenumber(LineList[c])))
                        //        {
                        //            item.SubItems.Add(SeparateMessage(LineList[c]));
                        //            c = LineList.Count;
                        //        }
                        //        else
                        //        {
                        //            if (c == LineList.Count - 1)
                        //            {
                        //                item.SubItems.Add("");
                        //            }
                        //            //item.SubItems.Add("不在线");
                        //        }
                        //    }
                        //    lstRemoteSiteFiles.Items.Add(item);

                        //}
                    }
                }
                 catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //根据名字去判断出编号
        public void findnumber(String  tempname) { 
            
           
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Display the Hidden frmLogin form
            //Application.OpenForms[0].Show();
            System.Environment.Exit(0);
            //Application.Exit();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            //Clear the Progress of TaskBar Progressbar
            TaskBarManager.ClearProgressValue();

            objOpenDialog = new CommonOpenFileDialog("选择一个文件上传");
            objOpenDialog.Multiselect = false;
            CommonFileDialogFilter Filter = new CommonFileDialogFilter("所有文件类型", "*.*");
            objOpenDialog.Filters.Add(Filter);
            objOpenDialog.RestoreDirectory = true;
            if (objOpenDialog.ShowDialog() == CommonFileDialogResult.OK)
            {
                //Declare and Setup out UploadForm Variable.  The frmUpload Constructor will do everything else, including showing the form.
                frmUpload UploadForm = new frmUpload(objOpenDialog.FileName, FtpClient.CurrentDirectory, FtpClient);
            }
            //Call RefreshDirectory; Refresh the Files and Folders for Current Directory.
            RefreshDirectory();
        }
        #endregion        

        private void tvFileSystem_SelectedDirectoriesChanged(object sender, SelectedDirectoriesChangedEventArgs e)
        {

        }

        private void txtRemoteDirectory_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //文件夹
            //FolderBrowserDialog dialog = new FolderBrowserDialog();
            //dialog.Description = "请选择文件路径";
            //if (dialog.ShowDialog() == DialogResult.OK)
            //{



            //    foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail("/"))
            //    {
            //        if (!folder.Filename.Equals("test.txt"))
            //        {
            //            if (folder.FileType == FTPfileInfo.DirectoryEntryTypes.Directory) {
            //                workThread = new Thread(delegate()
            //                {
            //                    string foldPath = dialog.SelectedPath;
            //                    string localPath = foldPath.Substring(0, foldPath.LastIndexOf("\\") + 1);
            //                    string fileName = foldPath.Substring(foldPath.LastIndexOf("\\") + 1);
            //                    Upfilesorfile(localPath, fileName, folder.Filename);
            //                });
            //                workThread.Start();
            //                //MessageBox.Show("已选择文件夹:" + foldPath + "localPath: " + localPath + "   fileName" + fileName, "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            }
            //        }
            //    }
            //    RefreshDirectory();





                
               
            //}

            //单个文件
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                
                //foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail("/"))
                //{
                    //if (!folder.Filename.Equals("test.txt"))
                    //{
                        //if (folder.FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                        for (int i = 0; i < listfolders.Count; i++) {

                            string file = fileDialog.FileName;
                            //MessageBox.Show("已选择文件:" + file, "选择文件提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //string file = fileDialog.FileName;
                            string localPath = file.Substring(0, file.LastIndexOf("\\") + 1);
                            string fileName = file.Substring(file.LastIndexOf("\\") + 1);
                            //upsimplefile(file, folder.Filename, fileName);
                            upsimplefile(file, listfolders[i].Filename, fileName);
                            Thread.Sleep(10);
                        }
                        
                    //}
                //}
               
            }
            //RefreshDirectory();
        }


        public void upsimplefile(string localFile,string ftpPath,string localfilename)
        {
            String FtpPath = "ftp://120.77.249.136/" + ftpPath + "/" + localfilename;
            Uri uri = new Uri(FtpPath);
            if (!File.Exists(localFile))
            {
                //Response.Write("文件：“" + localFile + "” 不存在！");
                return;
            }
            FileInfo fileInf = new FileInfo(localFile);
            FtpWebRequest reqFTP;
            //Stream responseStream = null;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);// 根据uri创建FtpWebRequest对象   
            reqFTP.Credentials = new NetworkCredential("uftp", "uftp2017");// ftp用户名和密码  
            reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行  
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;// 指定执行什么命令  
            reqFTP.UseBinary = true;// 指定数据传输类型  
            reqFTP.UsePassive = false;
            reqFTP.ContentLength = fileInf.Length;// 上传文件时通知服务器文件的大小  
            int buffLength = 2048;// 缓冲大小设置为2kb  
            byte[] buff = new byte[buffLength];
            int contentLen;

            // 打开一个文件流 (System.IO.FileStream) 去读上传的文件  
            FileStream fs = fileInf.OpenRead();
            try
            {
                //FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                //responseStream = response.GetResponseStream();
                Stream responseStream = reqFTP.GetRequestStream();// 把上传的文件写入流  
                //获取请求的响应流
                //responseStream = response.GetResponseStream();

                contentLen = fs.Read(buff, 0, buffLength);// 每次读文件流的2kb  

                while (contentLen != 0)// 流内容没有结束  
                {
                    // 把内容从file stream 写入 upload stream  
                    responseStream.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // 关闭两个流  
                responseStream.Close();
                fs.Close();
                displayLog("文件【" + FtpPath + "】上传成功！");
            }
            catch (Exception ex)
            {
                //Response.Write("上传文件【" + ftpPath + "/" + fileInf.Name + "】时，发生错误：" + ex.Message + "<br/>");
            }
        
        }
        public void displayLog(string log)
        {
            textBox1.AppendText(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss   ") + log + "\r\n");
        }



        String ftpUserID = "uftp";
        String ftpPassword = "uftp2017";
        //上传文件夹测试方法
        public void Upfilesorfile(String localPath, String fileName,String path)
        {
            //FTP地址  
            string ftpPath = "ftp://120.77.249.136/"+path+"/";


            FileInfo fi = new FileInfo(localPath);
            //判断上传文件是文件还是文件夹  
            if ((fi.Attributes & FileAttributes.Directory) != 0)
            {
                //dir 如果是文件夹，则调用[上传文件夹]方法  
                UploadDirectory(localPath, ftpPath, fileName);
            }
            else
            {
                //file 如果是文件，则调用[上传文件]方法  
                UpLoadFiledemo(localPath + fileName, ftpPath);
            }
        }


        /// 上传文件  
        /// </summary>  
        /// <param name="localFile">要上传到FTP服务器的本地文件</param>  
        /// <param name="ftpPath">FTP地址</param>  
        public void UpLoadFiledemo(string localFile, string ftpPath)
        {
            if (!File.Exists(localFile))
            {
                //Response.Write("文件：“" + localFile + "” 不存在！");
                System.Diagnostics.Debug.WriteLine("文件：“" + localFile + "” 不存在！");
                return;
            }
            FileInfo fileInf = new FileInfo(localFile);
            FtpWebRequest reqFTP;

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(ftpPath);// 根据uri创建FtpWebRequest对象   
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);// ftp用户名和密码  
            reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行  
            reqFTP.UsePassive = false;//设置不被动模式访问
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;// 指定执行什么命令  
            reqFTP.UseBinary = true;// 指定数据传输类型  
            reqFTP.ContentLength = fileInf.Length;// 上传文件时通知服务器文件的大小  
            int buffLength = 2048;// 缓冲大小设置为2kb  
            byte[] buff = new byte[buffLength];
            int contentLen;

            // 打开一个文件流 (System.IO.FileStream) 去读上传的文件  
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();// 把上传的文件写入流  
                contentLen = fs.Read(buff, 0, buffLength);// 每次读文件流的2kb  

                while (contentLen != 0)// 流内容没有结束  
                {
                    // 把内容从file stream 写入 upload stream  
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // 关闭两个流  
                strm.Close();
                fs.Close();
                //Response.Write("文件【" + ftpPath + "/" + fileInf.Name + "】上传成功！<br/>");
                System.Diagnostics.Debug.WriteLine("文件【" + ftpPath + "/" + fileInf.Name + "】上传成功！<br/>");
            }
            catch (Exception ex)
            {
                //Response.Write("上传文件【" + ftpPath + "/" + fileInf.Name + "】时，发生错误：" + ex.Message + "<br/>");
                System.Diagnostics.Debug.WriteLine("上传文件【" + ftpPath + "/" + fileInf.Name + "】时，发生错误：" + ex.Message + "<br/>");
            }
        }

        /// 上传整个目录  
        /// </summary>  
        /// <param name="localDir">要上传的目录的上一级目录</param>  
        /// <param name="ftpPath">FTP路径</param>  
        /// <param name="dirName">要上传的目录名</param>  
        /// <param name="ftpUser">FTP用户名（匿名为空）</param>  
        /// <param name="ftpPassword">FTP登录密码（匿名为空）</param>  
        public void UploadDirectory(string localDir, string ftpPath, string dirName)
        {
            string dir = localDir + dirName + @"\"; //获取当前目录（父目录在目录名）  
            //检测本地目录是否存在  
            if (!Directory.Exists(dir))
            {
                //Response.Write("本地目录：“" + dir + "” 不存在！<br/>");
                System.Diagnostics.Debug.WriteLine("本地目录：“" + dir + "” 不存在！<br/>");
                return;
            }
            //检测FTP的目录路径是否存在  
            if (CheckDirectoryExist(ftpPath, dirName))
            {
                MakeDir(ftpPath, dirName);//不存在，则创建此文件夹  
            }
            List<List<string>> infos = GetDirDetails(dir); //获取当前目录下的所有文件和文件夹  

            //先上传文件  
            //Response.Write(dir + "下的文件数：" + infos[0].Count.ToString() + "<br/>");  
            for (int i = 0; i < infos[0].Count; i++)
            {
                Console.WriteLine(infos[0][i]);
                UpLoadFiledemo(dir + infos[0][i], ftpPath + dirName + @"/" + infos[0][i]);
            }
            //再处理文件夹  
            //Response.Write(dir + "下的目录数：" + infos[1].Count.ToString() + "<br/>");  
            for (int i = 0; i < infos[1].Count; i++)
            {
                UploadDirectory(dir, ftpPath + dirName + @"/", infos[1][i]);
                //Response.Write("文件夹【" + dirName + "】上传成功！<br/>");  
            }
        }

        /// <summary>  
        /// 判断ftp服务器上该目录是否存在  
        /// </summary>  
        /// <param name="ftpPath">FTP路径目录</param>  
        /// <param name="dirName">目录上的文件夹名称</param>  
        /// <returns></returns>  
        private bool CheckDirectoryExist(string ftpPath, string dirName)
        {
            bool flag = true;
            try
            {
                //实例化FTP连接  
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpPath + dirName);
                ftp.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftp.UsePassive = false;
                ftp.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        /// 创建文件夹    
        /// </summary>    
        /// <param name="ftpPath">FTP路径</param>    
        /// <param name="dirName">创建文件夹名称</param>    
        public void MakeDir(string ftpPath, string dirName)
        {

            FtpWebRequest reqFTP;
            try
            {
                string ui = (ftpPath + dirName).Trim();
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(ui);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
                System.Diagnostics.Debug.WriteLine("文件夹【" + dirName + "】创建成功！<br/>");
                //Response.Write("文件夹【" + dirName + "】创建成功！<br/>");
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("新建文件夹【" + dirName + "】时，发生错误：" + ex.Message);
                //Response.Write("新建文件夹【" + dirName + "】时，发生错误：" + ex.Message);
            }

        }

        /// 获取目录下的详细信息  
        /// </summary>  
        /// <param name="localDir">本机目录</param>  
        /// <returns></returns>  
        private List<List<string>> GetDirDetails(string localDir)
        {
            List<List<string>> infos = new List<List<string>>();
            try
            {
                infos.Add(Directory.GetFiles(localDir).ToList()); //获取当前目录的文件  

                infos.Add(Directory.GetDirectories(localDir).ToList()); //获取当前目录的目录  

                for (int i = 0; i < infos[0].Count; i++)
                {
                    int index = infos[0][i].LastIndexOf(@"\");
                    infos[0][i] = infos[0][i].Substring(index + 1);
                }
                for (int i = 0; i < infos[1].Count; i++)
                {
                    int index = infos[1][i].LastIndexOf(@"\");
                    infos[1][i] = infos[1][i].Substring(index + 1);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return infos;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (lstRemoteSiteFiles.SelectedItems[0] != null)
            {
                //Check if File or Folder
                if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "文件")
                {
                    try
                    {
                        //Delete the FILE
                        FtpClient.FtpDelete(lstRemoteSiteFiles.SelectedItems[0].Text);

                        //遍历ftp目录下的所有文件及文件夹

                        foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail("/"))
                        {
                            if (!folder.Filename.Equals("test.txt"))
                            {
                                if (folder.FileType == FTPfileInfo.DirectoryEntryTypes.Directory) {
                                    deleteFilesonle("ftp://120.77.249.136" +"/"+ folder.Filename +"/"+ lstRemoteSiteFiles.SelectedItems[0].SubItems[2].Text);
                              
                                }
                                else if (folder.Filename.Equals(lstRemoteSiteFiles.SelectedItems[0].Text))
                                {
                                    deleteFiles("ftp://120.77.249.136" +"/"+folder.Filename);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("删除文件出错.  错误信息: " + ex.Message);
                    }
                }
                RefreshDirectory();
            }
        }

        //删除特定的文件
        public void deleteFilesonle(String url)
        {
            //删除文件
            String FtpPath = url;
            //删除文件夹
            //String FtpPath = "ftp://120.77.249.136/" + path;
            Uri uri = new Uri(FtpPath);
            FtpWebRequest reqFTP;
            string result;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);// 根据uri创建FtpWebRequest对象   
                reqFTP.Credentials = new NetworkCredential("uftp", "uftp2017");// ftp用户名和密码  
                reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行
                reqFTP.UseBinary = true;// 指定数据传输类型  
                reqFTP.UsePassive = false;
                //删除文件
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                //删除文件夹
                //reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(ftpStream);
                result = sr.ReadToEnd();
                sr.Close();
                response.Close();
                ftpStream.Close();
                response.Close();
                displayLog("删除文件  " + url+"成功！");
                //Response.Write("文件夹【" + dirName + "】创建成功！<br/>");
            }
            catch (Exception ex)
            {
                displayLog("文件【" + url+"】不存在：");
                //continue;
                //Response.Write("新建文件夹【" + dirName + "】时，发生错误：" + ex.Message);
            }
        }


        public void deleteFiles(String path)
        {
            //删除文件
            String FtpPath = path;
            //删除文件夹
            //String FtpPath = "ftp://120.77.249.136/" + path;
            Uri uri = new Uri(FtpPath);
            FtpWebRequest reqFTP;
            string result;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);// 根据uri创建FtpWebRequest对象   
                reqFTP.Credentials = new NetworkCredential("uftp", "uftp2017");// ftp用户名和密码  
                reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行
                reqFTP.UseBinary = true;// 指定数据传输类型  
                reqFTP.UsePassive = false;
                //删除文件
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                //删除文件夹
                //reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(ftpStream);
                result = sr.ReadToEnd();
                sr.Close();
                response.Close();
                ftpStream.Close();
                response.Close();
                displayLog("文件【" + FtpPath + "】删除成功！<br/>");
                //Response.Write("文件夹【" + dirName + "】创建成功！<br/>");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("删除文件夹【" + "】时，发生错误：" + ex.Message);
                //Response.Write("新建文件夹【" + dirName + "】时，发生错误：" + ex.Message);
            }
        }



        //删除所有目录下的同命名文件夹 
        //private void toolStripButton3_Click(object sender, EventArgs e)
        //{
        //    deleteFilestest("ftp://120.77.249.136" + txtRemoteDirectory.Text + lstRemoteSiteFiles.SelectedItems[0].SubItems[2].Text);
        //    //遍历ftp服务器上面所有文件夹
        //    foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail("/"))
        //    {
        //        if (!folder.Filename.Equals("test.txt"))
        //        {
        //            if (folder.FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
        //            {
        //                deleteFilestest("ftp://120.77.249.136" + "/" + folder.Filename + "/" + lstRemoteSiteFiles.SelectedItems[0].SubItems[2].Text);

        //            }
        //            else if (folder.Filename.Equals(lstRemoteSiteFiles.SelectedItems[0].Text))
        //            {
        //                deleteFiles("ftp://120.77.249.136/" + folder.Filename);
        //            }
        //        }
        //    }

        //    RefreshDirectory();


        //    ////上传所有文件夹
        //    //FolderBrowserDialog dialog = new FolderBrowserDialog();
        //    //dialog.Description = "请选择文件路径";
        //    //if (dialog.ShowDialog() == DialogResult.OK)
        //    //{
        //    //    foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail("/"))
        //    //    {
        //    //        if (!folder.Filename.Equals("test.txt"))
        //    //        {
        //    //            if (folder.FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
        //    //            {
        //    //                string foldPath = dialog.SelectedPath;
        //    //                string localPath = foldPath.Substring(0, foldPath.LastIndexOf("\\") + 1);
        //    //                string fileName = foldPath.Substring(foldPath.LastIndexOf("\\") + 1);
        //    //                Upfilesorfile(localPath, fileName, folder.Filename);
        //    //                Thread.Sleep(10);
        //    //                //MessageBox.Show("已选择文件夹:" + foldPath + "localPath: " + localPath + "   fileName" + fileName, "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    //            }
        //    //        }
        //    //    }
        //    //    RefreshDirectory();
        //    //}


        //}

        private void 备注ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstRemoteSiteFiles.SelectedItems[0] != null)
            {
                //Check if File or Folder
                if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "文件")
                {
                }
                else
                //文件夹
                {
                    try
                    {
                        //FtpClient.FtpDeleteDirectory(txtRemoteDirectory.Text+lstRemoteSiteFiles.SelectedItems[0].SubItems[2].Text);
                        //lstRemoteSiteFiles.SelectedItems[0].SubItems[4].Text = "3500";
                        //弹出对话框 获取到值 并且保存
                        FrmChild frmChild = new FrmChild();
                        frmChild.ShowDialog();
                        if (frmChild.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            lstRemoteSiteFiles.SelectedItems[0].SubItems[4].Text  = frmChild.StrValue;//获取弹出窗体的属性值
                            //将获取到的值输入文件，并上传
                            writefile(lstRemoteSiteFiles.SelectedItems[0].SubItems[2].Text + ","
                                + lstRemoteSiteFiles.SelectedItems[0].Text + ":" + frmChild.StrValue);  


                        }
                        //编号,名称
                       
                        
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("删除文件出错.  错误信息: " + ex.Message);
                    }
                }

                RefreshDirectory();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }



    }
}


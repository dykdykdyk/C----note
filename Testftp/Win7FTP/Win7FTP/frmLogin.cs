using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Win7FTP.Library;
using System.Net;
using System.IO;

namespace Win7FTP
{
    public partial class frmLogin : GlassForm
    {
        #region Members
        public static Win7FTP.Library.FTPclient objFtp;
        private frmMain Main;
        #endregion

        #region Contructor
        public frmLogin()
        {
            //Init Form
            InitializeComponent();

            //Aero Composition Event 
            AeroGlassCompositionChanged += new AeroGlassCompositionChangedEvent(Form1_AeroGlassCompositionChanged);

            if (AeroGlassCompositionEnabled)
            {
                //We don't want pnlNonTransparent and the controls in it to be part of AERO
                //but we do want Aero...looks cool ;)
                ExcludeControlFromAeroGlass(pnlNonTransparent);
            }
            else
            {
                this.BackColor = Color.Teal;
            }
        }
        #endregion

        #region Events
        void Form1_AeroGlassCompositionChanged(object sender, AeroGlassCompositionChangedEvenArgs e)
        {
            // When the desktop composition mode changes the window exclusion must be changed appropriately.
            if (e.GlassAvailable)
            {
                ExcludeControlFromAeroGlass(pnlNonTransparent);
                Invalidate();
            }
            else
            {
                this.BackColor = Color.Teal;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //Below code will never fire....But I have it for Development purposes..
            //in case someone decides to make it resizable
            Rectangle panelRect = ClientRectangle;
            panelRect.Inflate(-30, -30);
            pnlNonTransparent.Bounds = panelRect;
            ExcludeControlFromAeroGlass(pnlNonTransparent);
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                //Set FTP
                FTPclient objFtp = new FTPclient(txtHostName.Text, txtUserName.Text, txtPassword.Text);
                //FTPclient objFtp = new FTPclient("120.77.249.136", "uftp", "uftp2017");
                objFtp.CurrentDirectory = "/";

                if (!txtUserName.Text.ToString().Trim().Equals("") && !txtPassword.Text.ToString().Trim().Equals("")
                    && !txtHostName.Text.ToString().Trim().Equals(""))
                {
                    Main = new frmMain();
                    //Set FTP Client in MAIN form
                    Main.SetFtpClient(objFtp);
                    System.Threading.Thread.Sleep(10);
                    //Show MAIN form and HIDE this one
                    Main.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("无法连接FTP服务器，请重新输入！");
                }
            }
            catch (Exception ex)
            {
                //Display Error
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void frmLogin_Load(object sender, EventArgs e)
        {
            downloadtest("test.txt");
            downloadtest("test1.txt");
            //downloadtest("rsyncd");

            //UpLoadFile("C:/Windows/test1.txt", "test1.txt");
            //UpLoadFiletest("C:/1/0.jpg", "11");
            //for (int i = 17; i < 151;i++ )
            //{
            //    MakeDir(i.ToString());
            //}
            //deleteFilestest("ftp://120.77.249.136/" + "103");
            //MakeDir("254");
            //Upfilesorfile();
        }
        String ftpUserID = "uftp";
        String ftpPassword = "uftp2017";
        //上传文件夹测试方法
        public void Upfilesorfile() {
            //FTP地址  
            string ftpPath = "ftp://120.77.249.136/";
            //本机要上传的目录的父目录  
            string localPath = "E:\\发布\\";
            //要上传的目录名  
            string fileName = "test1";

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

                string fileUrl = url +"/"+ name;

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

            FtpWebRequest removeRequest = (FtpWebRequest)WebRequest.Create(url);
            removeRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
            removeRequest.Credentials = new NetworkCredential("uftp", "uftp2017");
            removeRequest.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行
            removeRequest.UseBinary = true;// 指定数据传输类型  
            removeRequest.UsePassive = false;
            removeRequest.GetResponse();
        }



        public void deleteFile(String path) {
            //删除文件
        //    String FtpPath = "ftp://120.77.249.136/" + path+"/0.jpg";
            //删除文件夹
            String FtpPath = "ftp://120.77.249.136/" + path;
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
             //   reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                //删除文件夹
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(ftpStream);
                result = sr.ReadToEnd();
                sr.Close();
                response.Close();
                ftpStream.Close();
                response.Close();
                //System.Diagnostics.Debug.WriteLine("文件夹【" + dirName + "】创建成功！<br/>");
                //Response.Write("文件夹【" + dirName + "】创建成功！<br/>");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("删除文件夹【" + "】时，发生错误：" + ex.Message);
                //Response.Write("新建文件夹【" + dirName + "】时，发生错误：" + ex.Message);
            }
        }

        public void deleteFiles(String path)
        {
            //删除文件
            String FtpPath = "ftp://120.77.249.136/" + path + "/0.jpg";
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
                //System.Diagnostics.Debug.WriteLine("文件夹【" + dirName + "】创建成功！<br/>");
                //Response.Write("文件夹【" + dirName + "】创建成功！<br/>");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("删除文件夹【" + "】时，发生错误：" + ex.Message);
                //Response.Write("新建文件夹【" + dirName + "】时，发生错误：" + ex.Message);
            }
        }


        /// 创建文件夹
        public void MakeDir(string dirName)
        {
            String FtpPath = "ftp://120.77.249.136/" + dirName;
            Uri uri = new Uri(FtpPath);
            //if (!File.Exists(localFile))
            //{
            //    //Response.Write("文件：“" + localFile + "” 不存在！");
            //    return;
            //}
            FtpWebRequest reqFTP;
            //Stream responseStream = null;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);// 根据uri创建FtpWebRequest对象   
            reqFTP.Credentials = new NetworkCredential("uftp", "uftp2017");// ftp用户名和密码  
            reqFTP.KeepAlive = false;// 默认为true，连接不会被关闭 // 在一个命令之后被执行
            reqFTP.UseBinary = true;// 指定数据传输类型  
            reqFTP.UsePassive = false;
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex.ToString());
            }

            try
            {
                UpLoadFiles("C:/Windows/aa.txt", FtpPath+"/aa.txt");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex.ToString());
            }

        }

        public void UpLoadFiles(string localFile, string FtpPath)
        {
            //上传到服务器上面的绝对路径
            //String FtpPath = "ftp://120.77.249.136/test.txt";
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
                System.Diagnostics.Debug.WriteLine("" + ex.ToString());
                //System.Diagnostics.Debug.
            }
        }

      

        // 上传test文件
        public void UpLoadFile(string localFile, string ftpPath)
        {
            String FtpPath = "ftp://120.77.249.136/" + ftpPath;
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

                    fs = File.Open("C:/Windows/"+path, FileMode.Open, FileAccess.ReadWrite);

                }
                //判断本地文件是否存在，如果不存在，则创建本地文件
                else
                {
                    fs = File.Create("C:/Windows/"+path);
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

        private void lblUserName_Click(object sender, EventArgs e)
        {

        }

        private void pnlNonTransparent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

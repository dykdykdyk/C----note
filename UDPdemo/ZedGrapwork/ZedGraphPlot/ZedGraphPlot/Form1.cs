﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ZedGraphPlot
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        bool m_is_running = true;
        // pane used to draw your chart
        GraphPane myPane1 = new GraphPane();
        GraphPane myPane2 = new GraphPane();
        GraphPane myPane3 = new GraphPane();
        GraphPane myPane4 = new GraphPane();
        Thread workThread, workThreadtwo;
        // poing pair lists
        PointPairList listPoints11 = new PointPairList();
        PointPairList listPoints31 = new PointPairList();
        PointPairList listPoints51 = new PointPairList();
        PointPairList listPoints71 = new PointPairList();
        List<String> devicenamelist = new List<String> { };

        List<String> heartdatalist = new List<String> { };
        List<String> stepdatalist = new List<String> { };
        List<String> temperaturedatalist = new List<String> { };
        List<String> rssidatalist = new List<String> { };

        List<String> Arraycache=new List<String> {};
        List<String> ArraycacheTemp = new List<String> { };
        // line item
        LineItem myCurve1;
        LineItem myCurve3;
        LineItem myCurve5;
        LineItem myCurve7;
        Socket server, client;
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form_Closing);

            intiipaddress();
        }

        private void intiipaddress() {
            string hostName = Dns.GetHostName();//本机名                                      
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                    ListViewItem lv = new ListViewItem();
                    lv.SubItems[0].Text = AddressIP;
                    listView2.Items.Add(lv);
                    System.Diagnostics.Debug.WriteLine("收 " + AddressIP);
                }
            }
        }
       

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("收到： Form_Closing");
            m_is_running = false;
            if (server != null) {
                server.Close();
            }
            System.Environment.Exit(0);
        }
        string aa = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            //加载图表
            myPane1 = zedGraphControl1.GraphPane;
            myPane1.Title.Text = "实时心率折线图";
            myPane1.XAxis.Title.Text = "时间";
            myPane1.YAxis.Title.Text = "心率";
            //myPane1.XAxis.Scale.Max = 30;	//X轴最大30
            myCurve1 = myPane1.AddCurve(null, listPoints11, Color.Blue, SymbolType.None);
            myCurve1.Line.Width = 2;

            zedGraphControl1.AxisChange();
            //2
            myPane2 = zedGraphControl2.GraphPane;
            myPane2.Title.Text = "实时体温折线图";
            myPane2.XAxis.Title.Text = "时间";
            myPane2.YAxis.Title.Text = "体温";
            
            myCurve3 = myPane2.AddCurve(null, listPoints31, Color.Blue, SymbolType.None);
            myCurve3.Line.Width = 2;
            zedGraphControl2.AxisChange();
            //3
            myPane3 = zedGraphControl3.GraphPane;
            myPane3.Title.Text = "实时步数折线图";
            myPane3.XAxis.Title.Text = "时间";
            myPane3.YAxis.Title.Text = "步数";
            myCurve5 = myPane3.AddCurve(null, listPoints51, Color.Blue, SymbolType.None);
            myCurve5.Line.Width = 2;
            zedGraphControl3.AxisChange();


            myPane4 = zedGraphControl4.GraphPane;
            myPane4.Title.Text = "实时信号折线图";
            myPane4.XAxis.Title.Text = "时间";
            myPane4.YAxis.Title.Text = "信号";
            myCurve7 = myPane4.AddCurve(null, listPoints71, Color.Blue, SymbolType.None);
            myCurve7.Line.Width = 2;
            zedGraphControl4.AxisChange();
        }
        int counts = -1;
        private int length2;

        public void ServiceReciveMsg()
        {
            while (m_is_running)
            {
                
                //获取发送方的ip
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                //aa = "南音";
                //接收数据
                byte[] buffer2 = new byte[1024];
                try
                {
                   
                    length2 = server.ReceiveFrom(buffer2, ref point);
                }
                catch (SocketException e)
                {
                    continue;
                }
                counts++;
                byte[] data2 = new byte[31];
                // 3,1, 32, 2, 26, 255, 66, 50, 26, 255, 255, 255, 255, 19, 19, 19, 19, 80, 1, 52, 10, 0, 1, 1, 1, 0, 0, 1, 1, 0, 2
                Array.Copy(buffer2, 0, data2, 0, 31);
                System.Diagnostics.Debug.WriteLine("");
                //打印信息
                Cache(data2);
                for (int i = 0; i < 31; i++)
                {
                   
                    System.Diagnostics.Debug.Write(data2[i]+",");
                   
                    // if (i == 0)
                    //{
                    //    displayLog(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss   ") + ":" + Convert.ToString(data2[i], 10) + ",", 0);
                    //}
                    //else if (i == 30)
                    //{
                    //    displayLog(Convert.ToString(data2[i], 10), 1);
                    //    Array.Clear(data2, 0, data2.Length);
                    //}
                    //else
                    //{
                    //    displayLog(Convert.ToString(data2[i], 10) + ",", 0);
                    //}


                }
                System.Diagnostics.Debug.WriteLine("");
                byte[] buf = new byte[31];
                Array.Copy(buffer2, 0, buf, 0, 31);
                //buf = System.Text.Encoding.Default.GetBytes(message);
                //判断心跳包与数据包

                byte[] data = new byte[31];
                if (buf[1] == 3)
                {
                    //心跳包
                }
                else if (buf[1] == 1)
                {
                    //获取到有用的数据包
                    Array.Copy(buf,0, data, 0, 30);
                }
                //有效数据
                if (data[0] == 0x03 && data[1] == 0x01 
                    && data[4] == 0x1A && data[5] == 0xFF)
                {
                    //设置固定UUID后的处理办法
                    //datareslut(data);
                    byte[] devicedata = new byte[8];
                    Array.Copy(data, 6, devicedata, 0, 8);
                    //处理发送方ip地址
                    string devicename = Convert.ToString(devicedata[0], 16) + "," +
                        Convert.ToString(devicedata[1], 16) + "," + Convert.ToString(devicedata[2], 16) + "," +
                        Convert.ToString(devicedata[3], 16) + "," + Convert.ToString(devicedata[4], 16) + "," +
                        Convert.ToString(devicedata[5], 16) + "," + Convert.ToString(devicedata[6], 16) + "," +
                        Convert.ToString(devicedata[7], 16);
                    System.Diagnostics.Debug.WriteLine("devicename : " + devicename);
                    devicenameadd(devicename);//加入集合
                    int heart = (data[16] & 0xff); //心率数据
                    System.Diagnostics.Debug.WriteLine("心率数据 : " + heart);
                        deivceTest(devicename, heart);
                    int rssi = (data[17] & 0xff); //信号强度
                    sbyte i = (sbyte)rssi;
                    System.Diagnostics.Debug.WriteLine("信号强度 : " + i);
                    deivceTest2(devicename, i);
                    //zedGraphControl1.IsEnableHZoom = true; //允许水平缩放
                    //zedGraphControl1.IsEnableVZoom = true; //允许垂直缩放
                    int step = (((int)(data[18] & 0xff) & 0x7f) << 10)
                    | (((int)(data[19] & 0xff)) << 2) | ((data[20] & 0xff) >> 6); //运动步数
                    System.Diagnostics.Debug.WriteLine("运动步数 : " + step);
                    deivceTest3(devicename, step);
                    int temp = ((int)(data[23] & 0xff) & 0x7f) << 4 | ((data[24] & 0xff) >> 4); //环境温度，放大十倍 
                    System.Diagnostics.Debug.WriteLine("环境温度: " + temp/10);
                    deivceTest5(devicename, temp/10);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("数据包不对: ");
                }
            }
            server.Close();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Arraycache.Count == 0) {
                return;
            }
            if (testfinish)
            {
                testfinish = false;
                for (int j = 0; j < 31; j++)
                {
                    ArraycacheTemp.Add(Arraycache[j]);
                }
                //显示数据
                for (int j = 0; j < 31; j++)
                {
                    if (j == 0)
                    {
                        displayLog(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss   ") + "服务端接受到数据:" + ArraycacheTemp[j] + ",", 0);
                    }
                    else if (j == 30)
                    {
                        displayLog(ArraycacheTemp[j], 1);
                        ArraycacheTemp.RemoveRange(0, 31);
                        Arraycache.RemoveRange(0, 31);
                        testfinish = true;
                    }
                    else
                    {
                        displayLog(ArraycacheTemp[j] + ",", 0);
                    }
                }
            }
           
        }
        //缓存数据的方法
        bool testfinish =true,threadstart=true;
        private void Cache(byte[] arr)
        {
           
            for (int j = 0; j < 31; j++)
            {
                Arraycache.Add(Convert.ToString(arr[j], 10));
            }
              
           
            //}
        }
        //添加对listview的双击事件
        //添加数据步骤：在list集合去寻找数据 
        bool test = true;
        int j ;
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //查找出uuid
            j = 0;
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
                for (int i = 0; i < devicenamelist.Count; i++)
                {
                    if (listView1.SelectedItems[0].Text.Equals(devicenamelist[i]))
                    {
                        getdatashow(listView1.SelectedItems[0].Text, j);
                        refreshchart();
                    if (j < heartdatalist.Count) {
                        j++;
                    }
                    }
                }
        }
        //清空图表
        private void button3_Click(object sender, EventArgs e)
        {
                timer1.Stop();
                listPoints11.Clear();
                zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();
                
                listPoints51.Clear();
                zedGraphControl3.AxisChange();
                zedGraphControl3.Refresh();
                listPoints31.Clear();
                zedGraphControl2.AxisChange();
                zedGraphControl2.Refresh();
                listPoints71.Clear();
                zedGraphControl4.AxisChange();
                zedGraphControl4.Refresh();

        }
         int se1 = 1;
         int se2 = 1;
         int se3 = 1;
         int se4 = 1;
        //显示数据在图标控件
        public void getdatashow(string  name,int data) {
            //show

            //for (int i = 0; i < heartdatalist.Count; i++) {
            if (data < heartdatalist.Count) {
                if (name.Equals(getname(heartdatalist[data])))
                {
                   
                    //listPoints11.Add(se1, getdata(heartdatalist[data]));
                    listPoints11.Add(se1, getdata(heartdatalist[data]));
                    zedGraphControl1.AxisChange();
                    zedGraphControl1.Refresh();
                    se1++;
                }
            }
            if (data < temperaturedatalist.Count)
            {
                if (name.Equals(getname(temperaturedatalist[data])))
                {
                    listPoints31.Add(se2, getdata(temperaturedatalist[data]));
                    zedGraphControl2.AxisChange();
                    zedGraphControl2.Refresh();
                    se2++;
                }
            }
       
            if (data < stepdatalist.Count)
            {
                if (name.Equals(getname(stepdatalist[data])))
                {
                    listPoints51.Add(se3, getdata(stepdatalist[data]));
                    zedGraphControl3.AxisChange();
                    zedGraphControl3.Refresh();
                    se3++;
                }
            }
            //信号
            if (data < rssidatalist.Count)
            {
                if (name.Equals(getname(rssidatalist[data])))
                {
                    listPoints71.Add(se4, getdata(rssidatalist[data]));
                    zedGraphControl4.AxisChange();
                    zedGraphControl4.Refresh();
                    se4++;
                }
            }
        }
        public string getname(string name) {
            int counts = name.IndexOf(":");
            string tempname = name.Substring(0, counts);
            return tempname;
        }
        public int getdata(string name)
        {
            int counts = name.IndexOf(":");
            string tempname = name.Substring(counts+1);
            return int.Parse(tempname);
        }
        public void refreshchart() {
            if (listPoints11.Count > 60)
            {
                listPoints11.RemoveAt(0);
                zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();
            }
            //}
            if (listPoints31.Count > 60)
            {
                listPoints31.RemoveAt(0);
                zedGraphControl2.AxisChange();
                zedGraphControl2.Refresh();
            }
            if (listPoints51.Count > 60)
            {
                listPoints51.RemoveAt(0);
                zedGraphControl3.AxisChange();
                zedGraphControl3.Refresh();
            }
            if (listPoints71.Count > 60)
            {
                listPoints71.RemoveAt(0);
                zedGraphControl4.AxisChange();
                zedGraphControl4.Refresh();
            }
        }
        //处理设备的名字
        public void devicenameadd(String name)
        {
            //myCurve1 = myPane1.AddCurve(name, null, Color.Blue, SymbolType.None);
            //myCurve3 = myPane2.AddCurve(name, null, Color.Blue, SymbolType.None);
            //myCurve5 = myPane3.AddCurve(name, null, Color.Blue, SymbolType.None);
            if (devicenamelist.Count == 0) {
                devicenamelist.Add(name);
                adddatatolistview(name);
            }
            for (int i = 0; i < devicenamelist.Count; i++)
            {
                if (name.Equals(devicenamelist[i]))
                {
                    return;
                    
                }else if (i == devicenamelist.Count - 1 )
                {
                    devicenamelist.Add(name);
                    adddatatolistview(name);
                }
            }
        }
        

        int i1 = 0;
        //心率数据处理
        public void deivceTest(String name, int hear)
        {
            heartdatalist.Add(name+":"+ hear);
                //listPoints11.Add(i1, heart);
                //i1++;
        }
        //信号数据处理
        public void deivceTest2(String name, int hear)
        {
            rssidatalist.Add(name + ":" + hear);
            //listPoints11.Add(i1, heart);
            //i1++;
        }
        int x1 = 0;
        //运动步数
        public void deivceTest3(String name, int hea)
        {
            stepdatalist.Add(name + ":" + hea);
        }
        int y1 = 0;
        //环境温度
        public void deivceTest5(String name, int he)
        {
         temperaturedatalist.Add(name + ":" + he);
        }
        public void datareslut(byte[] data)
        {
            Form1 f = new Form1();
            //标准报文头0，1 字节
            int le = data[0] & 0xff; //长度 
            System.Diagnostics.Debug.WriteLine("长度1 : " + le);

            int type = data[1] & 0xff; //类型  
            System.Diagnostics.Debug.WriteLine("类型1 : " + type);

            //标准报文包 2，3 字节
            byte[] namebyte = { data[2], data[3] }; //设备短名 data[2],data[3] 
                                                    //
            String name = System.Text.Encoding.Default.GetString(namebyte);
            //     String name = new String(namebyte);
            System.Diagnostics.Debug.WriteLine("设备短名 : " + name);

            //标准报文头 4，5 字节
            int le2 = data[4] & 0xff; //长度 
            System.Diagnostics.Debug.WriteLine("长度2 : " + le2);
            int type2 = data[5] & 0xff; //类型  
            System.Diagnostics.Debug.WriteLine("类型2 : " + type2);
            

            // UUID  6~13 
            byte[] uuid = new byte[8];   //4.1.3.UUID 字节数组
            Array.Copy(data, 6, uuid, 0, 8);
            //System.Diagnostics.Debug.WriteLine("UUID : " + Arrays.toString(uuid));
            System.Diagnostics.Debug.WriteLine("UUID : " + uuid[0] + "," + uuid[1] + "," + uuid[2] + "," + uuid[3] + "," + uuid[4] + "," + uuid[5] + "," + uuid[6] + "," + uuid[7]);

            //4.1.4.数据 14~25
            int pack = data[14] & 0xff;  //一号报文格式
            System.Diagnostics.Debug.WriteLine("一号报文格式 : " + pack);

            int a = (data[15] & 0xff) >> 7; //电量更新
            System.Diagnostics.Debug.WriteLine("电量更新 : " + a);

            int b = (data[15] & 0xff) & 0x7f; //电量数据
            System.Diagnostics.Debug.WriteLine("电量数据 : " + b + "%");

            int heart = (data[16] & 0xff); //心率数据
            System.Diagnostics.Debug.WriteLine("心率数据 : " + heart);

            int rssi = (data[17] & 0xff); //心率数据
            sbyte i = (sbyte)rssi;
            BitConverter.GetBytes(rssi);
            //BitConverter.ToInt32(data[17], rssi);
            System.Diagnostics.Debug.WriteLine("信号 : " + rssi);
            System.Diagnostics.Debug.WriteLine("信号2 : " + i);

            int heart_update = (data[18] & 0xff) >> 7;  //心率更新
            System.Diagnostics.Debug.WriteLine("心率更新 : " + heart_update);

            int step = (((int)(data[18] & 0xff) & 0x7f) << 10)

                   | (((int)(data[19] & 0xff)) << 2) | ((data[20] & 0xff) >> 6); //运动步数
            System.Diagnostics.Debug.WriteLine("运动步数 : " + step);
            int sleep_light = ((int)((data[20] & 0xff) & 0x3f) << 5) | ((data[21] & 0xff) >> 3); //浅睡总时间
            System.Diagnostics.Debug.WriteLine("浅睡总时间 : " + sleep_light);

            int sleep_deep = (((int)(data[21] & 0xff) & 7) << 8) | (data[22] & 0xff); //深睡总时间
            System.Diagnostics.Debug.WriteLine("深睡总时间 : " + sleep_deep);

            int temp_update = (data[23] & 0xff) >> 7; //温度更新
            System.Diagnostics.Debug.WriteLine("温度更新 : " + temp_update);

            int temp = ((int)(data[23] & 0xff) & 0x7f) << 4 | ((data[24] & 0xff) >> 4); //环境温度，放大十倍 
            System.Diagnostics.Debug.WriteLine("环境温度: " + temp);


            int device_status = ((data[24] & 0xff) & 0x0f) >> 3; //设备状态 
            System.Diagnostics.Debug.WriteLine("设备状态 : " + device_status);

            //时间戳  26~29字节
            byte[] times = { data[26], data[27], data[28], data[29] };
            long s = bytesToLong(times, 0); //时间戳 ,单位秒
            System.Diagnostics.Debug.WriteLine("时间戳 ,单位秒 : " + s);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(s);
            System.Diagnostics.Debug.WriteLine("时间 : " + dt.ToString("yyyy/MM/dd HH:mm:ss:ffff"));
            //校验 30
                int xx = data[30] & 0xff;
                System.Diagnostics.Debug.WriteLine("校验 : " + xx);
                System.Diagnostics.Debug.WriteLine("只有30个字节");

        }
        public static long bytesToLong(byte[] src, int offset)
        {
            long value;
            value = ((long)(src[offset] & 0xFF)) << 24
                    | (((long)(src[offset + 1] & 0xFF)) << 16)
                    | (((long)(src[offset + 2] & 0xFF)) << 8) | ((long)src[offset + 3] & 0xFF);
            return value;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }
        //启动服务
        int port;
        private void button1_Click(object sender, EventArgs e)
        {
            if (threadstart)
            {
                timer2.Interval = 1000;
                timer2.Start();
                threadstart = false;
            }

            //6001 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            if (!listView2.SelectedItems[0].Text.ToString().Equals("") && !textBox2.Text.ToString().Equals(""))
            {
                //server.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.75"), 6001));//绑定端口号和IP
                string str = string.Empty;
                str = textBox2.Text.ToString();
                port = int.Parse(str);
                //这个ip地址 必须是你本机的ip地址
                server.Bind(new IPEndPoint(IPAddress.Parse(listView2.SelectedItems[0].Text.ToString()), port));//绑定端口号和IP
                Console.WriteLine("服务端已经开启：" + listView2.SelectedItems[0].Text.ToString() + "  port: " + port);
                Thread t = new Thread(ServiceReciveMsg);//开启接收消息线程
                t.Start();
                server.ReceiveTimeout = 60000;
                button1.Enabled = false;

            }
            else
            {
                MessageBox.Show("请输入您本机的ip地址和端口号后重试！");
                return;
            }

            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(ServiceReciveMsg);
            thread.IsBackground = true;
            thread.Start();

        }

        public void displayLog(string log,int number)
        {
            //textBox3.AppendText(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss   ") + log + "\r\n");
            if (number == 0)
            {
                textBox3.AppendText(log);
            }
            else if (number == 1)
            {
                textBox3.AppendText(log +"\n");
            }
           

        }

        //向listview中添加数据
        public void adddatatolistview(string  name) {
            ListViewItem lv = new ListViewItem();
            lv.SubItems[0].Text = name;
            listView1.Items.Add(lv);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine(listView2.SelectedItems[0].Text);
            displayLog("您选择的IP地址为：" + listView2.SelectedItems[0].Text.ToString(), 1);
            listView2.Enabled = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }



        //启动客户端 //测试假数据
        private void button2_Click(object sender, EventArgs e)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            client.Bind(new IPEndPoint(IPAddress.Parse(listView2.SelectedItems[0].Text.ToString()), 6002));
            Thread t = new Thread(sendMsg);
            t.Start();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        public void sendMsg()
        {
            EndPoint point = new IPEndPoint(IPAddress.Parse(listView2.SelectedItems[0].Text.ToString()), port);
            //while (true)
            //{
                //System.Threading.Thread.Sleep(2000);
                byte[] array1 = new byte[] { 3,1, 32, 2, 26, 255, 66, 50, 26, 255, 255, 255, 255, 19, 19, 19, 19, 80, 1, 52, 10, 0, 1, 1, 1, 0, 0, 1, 1, 0, 2 };
                byte[] array2 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 25, 25, 25, 25, 25, 25, 25, 25, 1, 52, 20, 0, 2, 2, 2, 0, 0, 2, 2, 0, 2, 33, 51, 114, 63 };
                byte[] array3 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 3, 255, 255, 255, 255, 255, 255, 255, 1, 52, 30, 0, 3, 3, 3, 0, 0, 3, 3, 0, 2, 33, 51, 114, 63 };
                byte[] array4 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 4, 255, 255, 255, 255, 255, 255, 255, 1, 52, 40, 0, 4, 4, 4, 0, 0, 4, 4, 0, 2, 33, 51, 114, 63 };
                byte[] array5 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 5, 255, 255, 255, 255, 255, 255, 255, 1, 52, 50, 0, 5, 5, 5, 0, 0, 5, 5, 0, 2, 33, 51, 114, 63 };
                byte[] array6 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 6, 255, 255, 255, 255, 255, 255, 255, 1, 52, 60, 0, 6, 6, 6, 0, 0, 6, 6, 0, 2, 33, 51, 114, 63 };
                byte[] array7 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 7, 255, 255, 255, 255, 255, 255, 255, 1, 52, 70, 0, 7, 7, 7, 0, 0, 7, 7, 0, 2, 33, 51, 114, 63 };
                byte[] array8 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 8, 255, 255, 255, 255, 255, 255, 255, 1, 52, 80, 0, 8, 8, 8, 0, 0, 8, 8, 0, 2, 33, 51, 114, 63 };
            //string msg ="赖以~~";
            for(int i = 0; i < 3; i++) { 
                    client.SendTo(array1, point);
            }

        }
    }
}

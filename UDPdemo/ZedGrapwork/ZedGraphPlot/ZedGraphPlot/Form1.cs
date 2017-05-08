using System;
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
        bool m_is_running;//判断服务是否在运行变量
        // pane used to draw your chart
        GraphPane myPane1;
        GraphPane myPane2;
        GraphPane myPane3;
        GraphPane myPane4;
        // poing pair lists
        PointPairList listPoints11;//图表中对应的点 同以下
        PointPairList listPoints31;
        PointPairList listPoints51;
        PointPairList listPoints71;
        List<String> devicenamelist;//显示及保存设备UUID名称集合
        List<String> heartdatalist;//保存心率数据集合
        List<String> stepdatalist;//保存步数数据集合
        List<String> temperaturedatalist;//保存环境温度集合
        List<String> rssidatalist;//保存信号强度集合

        List<String> Arraycache;//缓存数据集合
        List<String> ArraycacheTemp;//缓存数据临时集合
        // line item
        LineItem myCurve1;//显示在图表中的线条 同以下
        LineItem myCurve3;
        LineItem myCurve5;
        LineItem myCurve7;
        Socket server, client;//UDP服务
        int counts = -1;
        private int length2;
        int clientport = -1;
        string clientip;
        List<byte> listdyk = new List<byte>();
        Thread workThread, workThread2;
        public Form1()
        {
            InitializeComponent();//系统初始化窗体控件方法
            initView();//初始化zedGraphplot图表及变量
            intiipaddress();//初始化PCip地址列表显示
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form_Closing);//监听窗体关闭的委托方法
        }
        private void initView()
        {
            m_is_running = true;
            myPane1 = new GraphPane();
            myPane2 = new GraphPane();
            myPane3 = new GraphPane();
            myPane4 = new GraphPane();
            listPoints11 = new PointPairList();
            listPoints31 = new PointPairList();
            listPoints51 = new PointPairList();
            listPoints71 = new PointPairList();
            devicenamelist = new List<String> { };
            heartdatalist = new List<String> { };
            stepdatalist = new List<String> { };
            temperaturedatalist = new List<String> { };
            rssidatalist = new List<String> { };
            Arraycache = new List<String> { };
            ArraycacheTemp = new List<String> { };

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
        private void intiipaddress()
        {
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
            if (server != null)
            {
                server.Close();
            }
            System.Environment.Exit(0);
        }
        byte[] devicedata = new byte[8];
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        bool testTaskisfinish = false;
        public void ServiceReciveMsg()
        {
            while (m_is_running)
            {
                //获取发送方的ip
                IPEndPoint receiver = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                EndPoint point = (EndPoint)receiver;
                //接收数据
                byte[] buffer2 = new byte[1024];
                try
                {
                    length2 = server.ReceiveFrom(buffer2, ref point);
                    string strReceive = string.Format((point as IPEndPoint).Address.ToString() + ":" + (point as IPEndPoint).Port.ToString());
                    //System.Diagnostics.Debug.WriteLine("收到point2：" + point);
                    clientport = int.Parse((point as IPEndPoint).Port.ToString());
                    clientip = (point as IPEndPoint).Address.ToString();
                }
                catch (SocketException e)
                {
                    continue;
                }
                counts++;
                //基站返回的数据包
                if (buffer2[0] == 128 && buffer2[1] == 2)
                {
                    if ( !testTaskisfinish) {
                        testTaskisfinish = true;
                        byte[] bufferTemp = new byte[256];
                        Array.Copy(buffer2, 0, bufferTemp, 0, 256);
                        displayLogtextBox4("接收到包:", 2);
                        for (int i = 0; i < bufferTemp.Length; i++)
                        {
                            Console.Write("," + buffer2[i]);
                            //displayLogtextBox4("," + buffer2[i],2);
                        }
                        Console.WriteLine("包完毕" + "\n");
                        string backstring = System.Text.Encoding.ASCII.GetString(bufferTemp);
                        Console.Write("接收字符串为:" + backstring);
                        //displayLogtextBox4("", 1);
                        displayLogtextBox4(backstring,2);
                        testTaskisfinish = false;
                    }
                    //string backstring = System.Text.Encoding.ASCII.GetString(bufferTemp);
                    //Console.Write("接收字符串为:" + backstring);
                    //displayLogtextBox4(backstring, 1);
                   
                    //for (int i = 5; i < 128; i++) {
                    //    if (buffer2[i] == 13 && buffer2[i + 1] == 10 && buffer2[i + 2] == 13 && buffer2[i + 3] == 10) {
                    //        for (int k = 2; k < i+4; k++) {
                    //            listdyk.Add(buffer2[k]);
                    //            displayLogtextBox4(buffer2[k]+",",2);
                    //        }
                    //        byte[] name = listdyk.ToArray();
                    //        string backstring2 = System.Text.Encoding.ASCII.GetString(name);

                    //        displayLogtextBox4(backstring2,1);
                    //    }
                    //}
                }
                byte[] data2 = new byte[31];
                Array.Copy(buffer2, 0, data2, 0, 31);
                //System.Diagnostics.Debug.WriteLine("收到");
                //打印信息
                Cache(data2);
                for (int i = 0; i < 31; i++)
                {
                    //System.Diagnostics.Debug.Write(data2[i] + ",");
                }
                //System.Diagnostics.Debug.WriteLine("");
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
                    Array.Copy(buf, 0, data, 0, 30);
                }
                //有效数据
                if (data[0] == 0x03 && data[1] == 0x01
                    && data[4] == 0x1A && data[5] == 0xFF)
                {
                    //设置固定UUID后的处理办法
                    //datareslut(data);

                    Array.Copy(data, 6, devicedata, 0, 8);
                    //处理发送方ip地址
                    string devicename = Convert.ToString(devicedata[0], 16) + "," +
                        Convert.ToString(devicedata[1], 16) + "," + Convert.ToString(devicedata[2], 16) + "," +
                        Convert.ToString(devicedata[3], 16) + "," + Convert.ToString(devicedata[4], 16) + "," +
                        Convert.ToString(devicedata[5], 16) + "," + Convert.ToString(devicedata[6], 16) + "," +
                        Convert.ToString(devicedata[7], 16);
                    string devicenameten = Convert.ToString(devicedata[0], 10) + "," +
                        Convert.ToString(devicedata[1], 10) + "," + Convert.ToString(devicedata[2], 10) + "," +
                        Convert.ToString(devicedata[3], 10) + "," + Convert.ToString(devicedata[4], 10) + "," +
                        Convert.ToString(devicedata[5], 10) + "," + Convert.ToString(devicedata[6], 10) + "," +
                        Convert.ToString(devicedata[7], 10);
                    //Console.WriteLine("设备UUID："+ devicenameten);
                    //System.Diagnostics.Debug.WriteLine("devicename : " + devicename);
                    devicenameadd(devicename);//加入集合
                    int heart = (data[16] & 0xff); //心率数据
                                                   //System.Diagnostics.Debug.WriteLine("心率数据 : " + heart);
                    heartdatalist.Add(devicename + ":" + heart);
                    int rssi = (data[17] & 0xff); //信号强度
                    sbyte i = (sbyte)rssi;
                    //System.Diagnostics.Debug.WriteLine("信号强度 : " + i);
                    rssidatalist.Add(devicename + ":" + i);
                    //zedGraphControl1.IsEnableHZoom = true; //允许水平缩放
                    //zedGraphControl1.IsEnableVZoom = true; //允许垂直缩放
                    int step = (((int)(data[18] & 0xff) & 0x7f) << 10)
                    | (((int)(data[19] & 0xff)) << 2) | ((data[20] & 0xff) >> 6); //运动步数
                    //System.Diagnostics.Debug.WriteLine("运动步数 : " + step);
                    stepdatalist.Add(devicename + ":" + step);
                    int temp = ((int)(data[23] & 0xff) & 0x7f) << 4 | ((data[24] & 0xff) >> 4); //环境温度，放大十倍 
                    //System.Diagnostics.Debug.WriteLine("环境温度: " + temp/10);
                    temperaturedatalist.Add(devicename + ":" + temp / 10);
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine("数据包不对: ");
                }
            }
            server.Close();
        }
        private void test() {
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Arraycache.Count == 0)
            {
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
        bool testfinish = true, threadstart = true;
        private void Cache(byte[] arr)
        {

            for (int j = 0; j < 31; j++)
            {
                Arraycache.Add(Convert.ToString(arr[j], 10));
            }
        }
        //添加对listview的双击事件
        //添加数据步骤：在list集合去寻找数据
        int j;
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
                    if (j < heartdatalist.Count)
                    {
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
        public void getdatashow(string name, int data)
        {
            //show
            if (data < heartdatalist.Count)
            {
                if (name.Equals(Utils.getname(heartdatalist[data])))
                {
                    listPoints11.Add(se1, Utils.getdata(heartdatalist[data]));
                    zedGraphControl1.AxisChange();
                    zedGraphControl1.Refresh();
                    se1++;
                }
            }
            if (data < temperaturedatalist.Count)
            {
                if (name.Equals(Utils.getname(temperaturedatalist[data])))
                {
                    listPoints31.Add(se2, Utils.getdata(temperaturedatalist[data]));
                    zedGraphControl2.AxisChange();
                    zedGraphControl2.Refresh();
                    se2++;
                }
            }
            if (data < stepdatalist.Count)
            {
                if (name.Equals(Utils.getname(stepdatalist[data])))
                {
                    listPoints51.Add(se3, Utils.getdata(stepdatalist[data]));
                    zedGraphControl3.AxisChange();
                    zedGraphControl3.Refresh();
                    se3++;
                }
            }
            //信号
            if (data < rssidatalist.Count)
            {
                if (name.Equals(Utils.getname(rssidatalist[data])))
                {
                    listPoints71.Add(se4, Utils.getdata(rssidatalist[data]));
                    zedGraphControl4.AxisChange();
                    zedGraphControl4.Refresh();
                    se4++;
                }
            }
        }

        public void refreshchart()
        {
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
            if (devicenamelist.Count == 0)
            {
                devicenamelist.Add(name);
                adddatatolistview(name);
            }
            for (int i = 0; i < devicenamelist.Count; i++)
            {
                if (name.Equals(devicenamelist[i]))
                {
                    return;

                }
                else if (i == devicenamelist.Count - 1)
                {
                    devicenamelist.Add(name);
                    adddatatolistview(name);
                }
            }
        }
        //启动服务
        int port;
        private void button1_Click(object sender, EventArgs e)
        {

            if (threadstart)
            {
                timer2.Interval = 6;//设置打印信息的间隔..
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
                displayLog("服务已经开启： ip:" + listView2.SelectedItems[0].Text.ToString() + "  端口号: " + port, 1);
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
        public void displayLog(string log, int number)
        {
            if (number == 0)
            {
                textBox3.AppendText(log);
            }
            else if (number == 1)
            {
                textBox3.AppendText(log + "\n");
            }
        }
        public void displayLogtextBox4(string log, int i)
        {
            Console.WriteLine("收到应答:" + log);
            if (i == 1)
            {
                textBox4.AppendText(log + "\n");
            }
            if (i == 2)
            {
                textBox4.AppendText(log);
            }

        }
        //向listview中添加数据
        public void adddatatolistview(string name)
        {
            ListViewItem lv = new ListViewItem();
            lv.SubItems[0].Text = name;
            listView1.Items.Add(lv);
        }
        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(listView2.SelectedItems[0].Text);
            displayLog("您选择的IP地址为：" + listView2.SelectedItems[0].Text.ToString(), 1);
            listView2.Enabled = false;
        }
        /// 电脑向基站发送数据包
        public void ServicesendMsg(string request)
        {
            Console.WriteLine("clientport : " + clientport + ",clientip:" + clientip);
            if (clientport == -1 && clientip == null)
            {
                MessageBox.Show("与基站通信异常，请确定能够收到基站数据后，重新尝试！");
                return;
            }
            EndPoint point = new IPEndPoint(IPAddress.Parse(clientip), clientport);
            byte[] temp = new byte[128];
            temp[0] = 128;
            temp[1] = 5;
            request = request + "\r\n";
            byte[] byte2 = System.Text.Encoding.Default.GetBytes(request);
            Console.WriteLine("byte2.Length:"+ byte2.Length);
            Console.WriteLine("request:" + request);
            Array.Copy(byte2, 0, temp, 2, byte2.Length);
            Console.WriteLine("向基站发送数据:");
            for (int i = 0; i < 128; i++)
            {
                System.Diagnostics.Debug.Write(temp[i] + ",");
            }
            System.Diagnostics.Debug.WriteLine("");
            server.SendTo(temp, point);
        }
        //电脑端发送数据给基站 
        private void button4_Click(object sender, EventArgs e)
        {
            //Thread workThread = new Thread(delegate ()
            //{
                ServicesendMsg(textBox1.Text.ToString());
            //});
            //workThread.Start();


        }
        //启动客户端 //测试假数据
        private void button2_Click(object sender, EventArgs e)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            client.Bind(new IPEndPoint(IPAddress.Parse(listView2.SelectedItems[0].Text.ToString()), 6002));
            Thread t = new Thread(sendMsg);
            t.Start();
        }
        public void sendMsg()
        {
            EndPoint point = new IPEndPoint(IPAddress.Parse(listView2.SelectedItems[0].Text.ToString()), port);
            byte[] array1 = new byte[] { 3, 1, 32, 2, 26, 255, 66, 50, 26, 255, 255, 255, 255, 19, 19, 19, 19, 80, 1, 52, 10, 0, 1, 1, 1, 0, 0, 1, 1, 0, 2 };
            byte[] array2 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 25, 25, 25, 25, 25, 25, 25, 25, 1, 52, 20, 0, 2, 2, 2, 0, 0, 2, 2, 0, 2, 33, 51, 114, 63 };
            byte[] array3 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 3, 255, 255, 255, 255, 255, 255, 255, 1, 52, 30, 0, 3, 3, 3, 0, 0, 3, 3, 0, 2, 33, 51, 114, 63 };
            byte[] array4 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 4, 255, 255, 255, 255, 255, 255, 255, 1, 52, 40, 0, 4, 4, 4, 0, 0, 4, 4, 0, 2, 33, 51, 114, 63 };
            byte[] array5 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 5, 255, 255, 255, 255, 255, 255, 255, 1, 52, 50, 0, 5, 5, 5, 0, 0, 5, 5, 0, 2, 33, 51, 114, 63 };
            byte[] array6 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 6, 255, 255, 255, 255, 255, 255, 255, 1, 52, 60, 0, 6, 6, 6, 0, 0, 6, 6, 0, 2, 33, 51, 114, 63 };
            byte[] array7 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 7, 255, 255, 255, 255, 255, 255, 255, 1, 52, 70, 0, 7, 7, 7, 0, 0, 7, 7, 0, 2, 33, 51, 114, 63 };
            byte[] array8 = new byte[] { 5, 1, 255, 255, 255, 255, 32, 2, 3, 8, 66, 50, 26, 255, 8, 255, 255, 255, 255, 255, 255, 255, 1, 52, 80, 0, 8, 8, 8, 0, 0, 8, 8, 0, 2, 33, 51, 114, 63 };
            for (int i = 0; i < 3; i++)
            {
                client.SendTo(array1, point);
            }
        }
    }
}

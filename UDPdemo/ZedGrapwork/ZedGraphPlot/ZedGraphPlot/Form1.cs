using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace ZedGraphPlot
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        bool m_is_running = true;
        // pane used to draw your chart
        GraphPane myPane1 = new GraphPane();
        GraphPane myPane2 = new GraphPane();
        GraphPane myPane3 = new GraphPane();
        Thread workThread, workThreadtwo;
        // poing pair lists
        PointPairList listPoints11 = new PointPairList();
        PointPairList listPoints12 = new PointPairList();
        PointPairList listPoints13 = new PointPairList();
        PointPairList listPoints14 = new PointPairList();
        PointPairList listPoints15 = new PointPairList();
        PointPairList listPoints16 = new PointPairList();
        PointPairList listPoints17 = new PointPairList();
        PointPairList listPoints18 = new PointPairList();

        PointPairList listPoints31 = new PointPairList();
        PointPairList listPoints32 = new PointPairList();
        PointPairList listPoints33 = new PointPairList();
        PointPairList listPoints34 = new PointPairList();
        PointPairList listPoints35 = new PointPairList();
        PointPairList listPoints36 = new PointPairList();
        PointPairList listPoints37 = new PointPairList();
        PointPairList listPoints38 = new PointPairList();



        PointPairList listPoints51 = new PointPairList();
        PointPairList listPoints52 = new PointPairList();
        PointPairList listPoints53 = new PointPairList();
        PointPairList listPoints54 = new PointPairList();
        PointPairList listPoints55 = new PointPairList();
        PointPairList listPoints56 = new PointPairList();
        PointPairList listPoints57 = new PointPairList();
        PointPairList listPoints58 = new PointPairList();
        List<String> devicenamelist = new List<String> { };
        //String[] devicenamelist = new String[8];

        // line item
        LineItem myCurve1;
        LineItem myCurve3;
        LineItem myCurve5;
        Socket server, client;
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form_Closing);
            //devicenamelist.Add("242343");
            //devicenamelist.Add("242343");
            //devicenamelist.Add("242343");
            System.Diagnostics.Debug.WriteLine("收到： " + devicenamelist.Count);

        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("收到： Form_Closing");
            m_is_running = false;
            System.Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // set your pane
            myPane1 = zedGraphControl1.GraphPane;
            // set a title
            myPane1.Title.Text = "实时心率折线图";
            // set X and Y axis titles
            myPane1.XAxis.Title.Text = "时间";
            myPane1.YAxis.Title.Text = "心率";
            // ---- CURVE ONE ----
            // draw a sin curve
            //for (int i = 0; i < 100; i++)
            //{
            //    listPointsOne.Add(i, Math.Sin(i));
            //}

            // set lineitem to list of points
            //myCurveOne = myPane.AddCurve(null, listPointsOne, Color.Black, SymbolType.Circle);    
            //listPoints11.Add(1, 20);
            myCurve1 = myPane1.AddCurve(null, listPoints11, Color.Blue, SymbolType.None);
            myCurve1 = myPane1.AddCurve(null, listPoints12, Color.DarkGray, SymbolType.None);
            myCurve1 = myPane1.AddCurve(null, listPoints13, Color.Green, SymbolType.None);
            myCurve1 = myPane1.AddCurve(null, listPoints14, Color.Orange, SymbolType.None);
            myCurve1 = myPane1.AddCurve(null, listPoints15, Color.Pink, SymbolType.None);
            myCurve1 = myPane1.AddCurve(null, listPoints16, Color.SkyBlue, SymbolType.None);
            myCurve1 = myPane1.AddCurve(null, listPoints17, Color.Yellow, SymbolType.None);
            myCurve1 = myPane1.AddCurve(null, listPoints18, Color.Red, SymbolType.None);
            //myCurve1.Line.Width =5;
            myCurve1.Line.Width = 2;
            zedGraphControl1.AxisChange();
            //2
            myPane2 = zedGraphControl2.GraphPane;
            // set a title
            myPane2.Title.Text = "实时体温折线图";
            // set X and Y axis titles
            myPane2.XAxis.Title.Text = "时间";
            myPane2.YAxis.Title.Text = "体温";
            //listPoints3.Add(1, 30);
            //listPoints3.Add(50, 50);        
            // set lineitem to list of points


            myCurve3 = myPane2.AddCurve(null, listPoints31, Color.Blue, SymbolType.None);
            myCurve3 = myPane2.AddCurve(null, listPoints32, Color.DarkGray, SymbolType.None);
            myCurve3 = myPane2.AddCurve(null, listPoints33, Color.Green, SymbolType.None);
            myCurve3 = myPane2.AddCurve(null, listPoints34, Color.Orange, SymbolType.None);
            myCurve3 = myPane2.AddCurve(null, listPoints35, Color.Pink, SymbolType.None);
            myCurve3 = myPane2.AddCurve(null, listPoints36, Color.SkyBlue, SymbolType.None);
            myCurve3 = myPane2.AddCurve(null, listPoints37, Color.Yellow, SymbolType.None);
            myCurve3 = myPane2.AddCurve(null, listPoints38, Color.Red, SymbolType.None);
            myCurve3.Line.Width = 2;
            zedGraphControl2.AxisChange();




            //3
            myPane3 = zedGraphControl3.GraphPane;
            // set a title
            myPane3.Title.Text = "实时步数折线图";
            // set X and Y axis titles
            myPane3.XAxis.Title.Text = "时间";
            myPane3.YAxis.Title.Text = "步数";
            //listPoints5.Add(1, 40);
            //listPointsTwo.Add(50, 50);        
            // set lineitem to list of points



            myCurve5 = myPane3.AddCurve(null, listPoints51, Color.Blue, SymbolType.None);
            myCurve5 = myPane3.AddCurve(null, listPoints52, Color.DarkGray, SymbolType.None);
            myCurve5 = myPane3.AddCurve(null, listPoints53, Color.Green, SymbolType.None);
            myCurve5 = myPane3.AddCurve(null, listPoints54, Color.Orange, SymbolType.None);
            myCurve5 = myPane3.AddCurve(null, listPoints55, Color.Pink, SymbolType.None);
            myCurve5 = myPane3.AddCurve(null, listPoints56, Color.SkyBlue, SymbolType.None);
            myCurve5 = myPane3.AddCurve(null, listPoints57, Color.Yellow, SymbolType.None);
            myCurve5 = myPane3.AddCurve(null, listPoints58, Color.Red, SymbolType.None);
            myCurve5.Line.Width = 2;
            zedGraphControl3.AxisChange();
        }
        int i = 0;
        private int length2;

        public void ServiceReciveMsg()
        {
            while (m_is_running)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                //byte[] buffer = new byte[1024];
                //int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                //string message = Encoding.UTF8.GetString(buffer, 0, length);


                byte[] buffer2 = new byte[1024];
                try
                {
                    length2 = server.ReceiveFrom(buffer2, ref point);
                }
                catch (SocketException e)
                {
                    //System.Diagnostics.Debug.WriteLine(" 異常： " + e.ToString());
                    continue;
                }
                //接收数据


                string message = System.Text.Encoding.Default.GetString(buffer2, 0, length2);
                System.Diagnostics.Debug.WriteLine("收到： ");
                for (int i = 0; i < 31; i++)
                {
                    System.Diagnostics.Debug.Write("," + buffer2[i]);
                }
                System.Diagnostics.Debug.WriteLine("");
                byte[] data = new byte[32];
                data = System.Text.Encoding.Default.GetBytes(message);
                if (data[0] == 0x03 && data[1] == 0x08 && data[2] == 0x42 && data[3] == 0x32
                    && data[4] == 0x1A && data[5] == 0xFF)
                {
                    //datareslut(data);


                    //处理设备的不同 画不同的线 
                    byte[] devicedata = new byte[8];
                    Array.Copy(data, 6, devicedata, 0, 8);
                    string devicename = System.Text.Encoding.Default.GetString(devicedata);
                    //devicenamelist.add
                    //string devicename = System.Text.Encoding.Default.GetString(devicedata);

                    devicenameadd(devicename);//加入集合

                    int heart = (data[16] & 0xff); //心率数据
                    System.Diagnostics.Debug.WriteLine("心率数据 : " + heart);
                    //listPoints11.Add(i, heart);
                    //listPoints12.Add(1, 70);
                    //listPoints12.Add(2, 70);
                    //listPoints12.Add(3, 70);
                    //listPoints12.Add(4, 70);
                    //listPoints12.Add(5, 70);
                    //listPoints12.Add(6, 70);
                    //listPoints12.Add(7, 70);
                    //listPoints12.Add(8, 70);
                    deivceTest(devicename, heart);
                    int step = (((int)(data[18] & 0xff) & 0x7f) << 10)

                    | (((int)(data[19] & 0xff)) << 2) | ((data[20] & 0xff) >> 6); //运动步数
                    System.Diagnostics.Debug.WriteLine("运动步数 : " + step);
                    deivceTest3(devicename, step);
                    workThread = new Thread(delegate ()
                    {
                        //listPoints51.Add(i, step);
                        zedGraphControl3.AxisChange();
                        zedGraphControl3.Refresh();
                    });
                    workThread.Start();
                    System.Diagnostics.Debug.WriteLine("workThread: " + workThread.ManagedThreadId);
                    //添加进步数
                    int temp = ((int)(data[23] & 0xff) & 0x7f) << 4 | ((data[24] & 0xff) >> 4); //环境温度，放大十倍 
                    System.Diagnostics.Debug.WriteLine("环境温度: " + temp);
                    deivceTest5(devicename, temp);
                    workThreadtwo = new Thread(delegate ()
                    {
                        //listPoints31.Add(i, temp);
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    });
                    workThreadtwo.Start();
                    System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                    zedGraphControl1.AxisChange();
                    zedGraphControl1.Refresh();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("数据包不对: ");
                }
            }

            server.Close();
        }

        public void devicenameadd(String name)
        {
            if (devicenamelist.Count == 0)
            {
                devicenamelist.Add(name);
            }
            else if (devicenamelist.Count == 1)
            {
                if (!name.Equals(devicenamelist[0]))
                {
                    devicenamelist.Add(name);
                }
            }
            else if (devicenamelist.Count == 2)
            {
                if (!name.Equals(devicenamelist[0]) && !name.Equals(devicenamelist[1]))
                {
                    devicenamelist.Add(name);
                }
            }
            else if (devicenamelist.Count == 3)
            {
                if (!name.Equals(devicenamelist[0]) && !name.Equals(devicenamelist[1]) && !name.Equals(devicenamelist[2]))
                {
                    devicenamelist.Add(name);
                }
            }
            else if (devicenamelist.Count == 4)
            {
                if (!name.Equals(devicenamelist[0]) && !name.Equals(devicenamelist[1]) && !name.Equals(devicenamelist[2]) && !name.Equals(devicenamelist[3]))
                {
                    devicenamelist.Add(name);
                }
            }
            else if (devicenamelist.Count == 5)
            {
                if (!name.Equals(devicenamelist[0]) && !name.Equals(devicenamelist[1]) && !name.Equals(devicenamelist[2]) && !name.Equals(devicenamelist[3])
                && !name.Equals(devicenamelist[4]))
                {
                    devicenamelist.Add(name);
                }
            }
            else if (devicenamelist.Count == 6)
            {
                if (!name.Equals(devicenamelist[0]) && !name.Equals(devicenamelist[1]) && !name.Equals(devicenamelist[2]) && !name.Equals(devicenamelist[3])
                && !name.Equals(devicenamelist[4]) && !name.Equals(devicenamelist[5]))
                {
                    devicenamelist.Add(name);
                }
            }
            else if (devicenamelist.Count == 7)
            {
                if (!name.Equals(devicenamelist[0]) && !name.Equals(devicenamelist[1]) && !name.Equals(devicenamelist[2]) && !name.Equals(devicenamelist[3])
                && !name.Equals(devicenamelist[4]) && !name.Equals(devicenamelist[5]) && !name.Equals(devicenamelist[6]))
                {
                    devicenamelist.Add(name);
                }
            }
            else if (devicenamelist.Count == 8)
            {
                //if (!name.Equals(devicenamelist[0]) && !name.Equals(devicenamelist[1]) && !name.Equals(devicenamelist[2]) && !name.Equals(devicenamelist[3])
                //&& !name.Equals(devicenamelist[4]) && !name.Equals(devicenamelist[5]) && !name.Equals(devicenamelist[6]))
                //{
                //    devicenamelist.Add(name);
                //}
            }

            //if (!name.Equals(devicenamelist[0]) && !name.Equals(devicenamelist[1]) && !name.Equals(devicenamelist[2]) && !name.Equals(devicenamelist[3])
            //    && !name.Equals(devicenamelist[4]) && !name.Equals(devicenamelist[6]) && !name.Equals(devicenamelist[7]))
            //{
            //    //devicenamelist[]
            //    System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
            //}
        }
        int i1 = 0;
        int i2 = 0;
        int i3 = 0;
        int i4 = 0;
        int i5 = 0;
        int i6 = 0;
        int i7 = 0;
        int i8 = 0;
        public void deivceTest(String name, int heart)
        {

            if (devicenamelist.Count == 0)
            {

                listPoints11.Add(i1, heart);
                i1++;
                //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
            }
            else if (devicenamelist.Count == 1)
            {
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints11.Add(i1, heart);
                    i1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 2)
            {
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints12.Add(i2, heart);
                    i2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints11.Add(i1, heart);
                    i1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 3)
            {
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints13.Add(i3, heart);
                    i3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints12.Add(i2, heart);
                    i2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints11.Add(i1, heart);
                    i1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 4)
            {
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints14.Add(i4, heart);
                    i4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints13.Add(i3, heart);
                    i3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints12.Add(i2, heart);
                    i2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints11.Add(i1, heart);
                    i1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 5)
            {
                if (name.Equals(devicenamelist[4]))
                {
                    listPoints15.Add(i5, heart);
                    i5++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints14.Add(i4, heart);
                    i4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints13.Add(i3, heart);
                    i3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints12.Add(i2, heart);
                    i2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints11.Add(i1, heart);
                    i1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 6)
            {
                if (name.Equals(devicenamelist[5]))
                {
                    listPoints16.Add(i6, heart);
                    i6++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[4]))
                {
                    listPoints15.Add(i5, heart);
                    i5++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints14.Add(i4, heart);
                    i4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints13.Add(i3, heart);
                    i3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints12.Add(i2, heart);
                    i2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints11.Add(i1, heart);
                    i1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 7)
            {
                if (name.Equals(devicenamelist[6]))
                {
                    listPoints17.Add(i7, heart);
                    i7++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[5]))
                {
                    listPoints16.Add(i6, heart);
                    i6++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[4]))
                {
                    listPoints15.Add(i5, heart);
                    i5++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints14.Add(i4, heart);
                    i4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints13.Add(i3, heart);
                    i3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints12.Add(i2, heart);
                    i2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints11.Add(i1, heart);
                    i1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 8)
            {
                if (name.Equals(devicenamelist[7]))
                {
                    listPoints18.Add(i8, heart);
                    i8++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[6]))
                {
                    listPoints17.Add(i7, heart);
                    i7++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[5]))
                {
                    listPoints16.Add(i6, heart);
                    i6++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[4]))
                {
                    listPoints15.Add(i5, heart);
                    i5++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints14.Add(i4, heart);
                    i4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints13.Add(i3, heart);
                    i3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints12.Add(i2, heart);
                    i2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints11.Add(i1, heart);
                    i1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }

        }
        int x1 = 0;
        int x2 = 0;
        int x3 = 0;
        int x4 = 0;
        int x5 = 0;
        int x6 = 0;
        int x7 = 0;
        int x8 = 0;
        public void deivceTest3(String name, int heart)
        {

            if (devicenamelist.Count == 0)
            {

                listPoints31.Add(x1, heart);
                x1++;
                //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
            }
            else if (devicenamelist.Count == 1)
            {
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints31.Add(x1, heart);
                    x1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 2)
            {
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints32.Add(x2, heart);
                    x2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints31.Add(x1, heart);
                    x1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 3)
            {
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints33.Add(x3, heart);
                    x3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints32.Add(x2, heart);
                    x2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints31.Add(x1, heart);
                    x1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 4)
            {
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints34.Add(x4, heart);
                    x4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints33.Add(x3, heart);
                    x3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints32.Add(x2, heart);
                    x2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints31.Add(x1, heart);
                    x1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 5)
            {
                if (name.Equals(devicenamelist[4]))
                {
                    listPoints35.Add(x5, heart);
                    x5++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints34.Add(x4, heart);
                    x4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints33.Add(x3, heart);
                    x3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints32.Add(x2, heart);
                    x2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints31.Add(x1, heart);
                    x1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 6)
            {
                if (name.Equals(devicenamelist[5]))
                {
                    listPoints36.Add(x6, heart);
                    x6++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[4]))
                {
                    listPoints35.Add(x5, heart);
                    x5++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints34.Add(x4, heart);
                    x4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints33.Add(x3, heart);
                    x3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints32.Add(x2, heart);
                    x2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints31.Add(x1, heart);
                    x1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 7)
            {
                if (name.Equals(devicenamelist[6]))
                {
                    listPoints37.Add(x7, heart);
                    x7++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[5]))
                {
                    listPoints36.Add(x6, heart);
                    x6++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[4]))
                {
                    listPoints35.Add(x5, heart);
                    x5++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints34.Add(x4, heart);
                    x4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints33.Add(x3, heart);
                    x3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints32.Add(x2, heart);
                    x2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints31.Add(x1, heart);
                    x1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 8)
            {
                if (name.Equals(devicenamelist[7]))
                {
                    listPoints38.Add(x8, heart);
                    x8++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[6]))
                {
                    listPoints37.Add(x7, heart);
                    x7++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[5]))
                {
                    listPoints36.Add(x6, heart);
                    x6++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[4]))
                {
                    listPoints35.Add(x5, heart);
                    x5++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints34.Add(x4, heart);
                    x4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints33.Add(x3, heart);
                    x3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints32.Add(x2, heart);
                    x2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints31.Add(x1, heart);
                    x1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }

        }
        int y1 = 0;
        int y2 = 0;
        int y3 = 0;
        int y4 = 0;
        int y5 = 0;
        int y6 = 0;
        int y7 = 0;
        int y8 = 0;
        public void deivceTest5(String name, int heart)
        {

            if (devicenamelist.Count == 0)
            {

                listPoints51.Add(y1, heart);
                y1++;
                //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
            }
            else if (devicenamelist.Count == 1)
            {
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints51.Add(y1, heart);
                    y1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 2)
            {
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints52.Add(y2, heart);
                    y2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints51.Add(y1, heart);
                    y1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 3)
            {
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints53.Add(y3, heart);
                    y3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints52.Add(y2, heart);
                    y2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints51.Add(y1, heart);
                    y1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 4)
            {
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints54.Add(y4, heart);
                    y4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints53.Add(y3, heart);
                    y3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints52.Add(y2, heart);
                    y2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints51.Add(y1, heart);
                    y1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 5)
            {
                if (name.Equals(devicenamelist[4]))
                {
                    listPoints55.Add(y5, heart);
                    y5++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints54.Add(y4, heart);
                    y4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints53.Add(y3, heart);
                    y3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints52.Add(y2, heart);
                    y2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints51.Add(y1, heart);
                    y1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 6)
            {
                if (name.Equals(devicenamelist[5]))
                {
                    listPoints56.Add(y6, heart);
                    y6++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[4]))
                {
                    listPoints55.Add(y5, heart);
                    y5++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints54.Add(y4, heart);
                    y4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints53.Add(y3, heart);
                    y3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints52.Add(y2, heart);
                    y2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints51.Add(y1, heart);
                    y1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 7)
            {
                if (name.Equals(devicenamelist[6]))
                {
                    listPoints57.Add(y7, heart);
                    y7++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[5]))
                {
                    listPoints56.Add(y6, heart);
                    y6++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[4]))
                {
                    listPoints55.Add(y5, heart);
                    y5++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints54.Add(y4, heart);
                    y4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints53.Add(y3, heart);
                    y3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints52.Add(y2, heart);
                    y2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints51.Add(y1, heart);
                    y1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }
            else if (devicenamelist.Count == 8)
            {
                if (name.Equals(devicenamelist[7]))
                {
                    listPoints58.Add(y8, heart);
                    y8++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[6]))
                {
                    listPoints57.Add(y7, heart);
                    y7++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[5]))
                {
                    listPoints56.Add(y6, heart);
                    y6++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[4]))
                {
                    listPoints55.Add(y5, heart);
                    y5++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[3]))
                {
                    listPoints54.Add(y4, heart);
                    y4++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[2]))
                {
                    listPoints53.Add(y3, heart);
                    y3++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[1]))
                {
                    listPoints52.Add(y2, heart);
                    y2++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
                if (name.Equals(devicenamelist[0]))
                {
                    listPoints51.Add(y1, heart);
                    y1++;
                    //System.Diagnostics.Debug.WriteLine("workThreadtwo: " + workThreadtwo.ManagedThreadId);
                }
            }

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

            //客户自定义报文包

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

            if (data.Length == 31)
            {
                int xx = data[30] & 0xff;
                System.Diagnostics.Debug.WriteLine("校验 : " + xx);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("只有30个字节");
            }

        }
        public static long bytesToLong(byte[] src, int offset)
        {
            long value;
            value = ((long)(src[offset] & 0xFF)) << 24
                    | (((long)(src[offset + 1] & 0xFF)) << 16)
                    | (((long)(src[offset + 2] & 0xFF)) << 8) | ((long)src[offset + 3] & 0xFF);
            return value;
        }
        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void zedGraphControl2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //启动服务
        private void button1_Click(object sender, EventArgs e)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            if (!textBox1.Text.ToString().Equals("") && !textBox2.Text.ToString().Equals(""))
            {
                //server.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.75"), 6001));//绑定端口号和IP
                string str = string.Empty;
                str = textBox2.Text.ToString();
                int port = int.Parse(str);
                server.Bind(new IPEndPoint(IPAddress.Parse(textBox1.Text.ToString()), port));//绑定端口号和IP

                Console.WriteLine("服务端已经开启：" + textBox1.Text.ToString() + "  port: " + port);
                Console.WriteLine("服务端已经开启：");
                Thread t = new Thread(ServiceReciveMsg);//开启接收消息线程
                t.Start();
                server.ReceiveTimeout = 10000;
            }




            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(ServiceReciveMsg);
            thread.IsBackground = true;
            thread.Start();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        //启动客户端 //测试假数据
        private void button2_Click(object sender, EventArgs e)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            client.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.75"), 6000));
            Thread t = new Thread(sendMsg);
            t.Start();
            //Thread t2 = new Thread(ReciveMsg);
            //t2.Start();
            Console.WriteLine("客户端已经开启");
        }
        public void sendMsg()
        {
            EndPoint point = new IPEndPoint(IPAddress.Parse("192.168.1.75"), 6001);
            while (true)
            {
                System.Threading.Thread.Sleep(400);
                byte[] array1 = new byte[] { 3, 8, 66, 50, 26, 255, 1, 255, 255, 255, 255, 255, 255, 255, 1, 52, 10, 0, 1, 1, 1, 0, 0, 1, 1, 0, 2, 33, 51, 114, 63 };
                byte[] array2 = new byte[] { 3, 8, 66, 50, 26, 255, 2, 255, 255, 255, 255, 255, 255, 255, 1, 52, 20, 0, 2, 2, 2, 0, 0, 2, 2, 0, 2, 33, 51, 114, 63 };
                byte[] array3 = new byte[] { 3, 8, 66, 50, 26, 255, 3, 255, 255, 255, 255, 255, 255, 255, 1, 52, 30, 0, 3, 3, 3, 0, 0, 3, 3, 0, 2, 33, 51, 114, 63 };
                byte[] array4 = new byte[] { 3, 8, 66, 50, 26, 255, 4, 255, 255, 255, 255, 255, 255, 255, 1, 52, 40, 0, 4, 4, 4, 0, 0, 4, 4, 0, 2, 33, 51, 114, 63 };
                byte[] array5 = new byte[] { 3, 8, 66, 50, 26, 255, 5, 255, 255, 255, 255, 255, 255, 255, 1, 52, 50, 0, 5, 5, 5, 0, 0, 5, 5, 0, 2, 33, 51, 114, 63 };
                byte[] array6 = new byte[] { 3, 8, 66, 50, 26, 255, 6, 255, 255, 255, 255, 255, 255, 255, 1, 52, 60, 0, 6, 6, 6, 0, 0, 6, 6, 0, 2, 33, 51, 114, 63 };
                byte[] array7 = new byte[] { 3, 8, 66, 50, 26, 255, 7, 255, 255, 255, 255, 255, 255, 255, 1, 52, 70, 0, 7, 7, 7, 0, 0, 7, 7, 0, 2, 33, 51, 114, 63 };
                byte[] array8 = new byte[] { 3, 8, 66, 50, 26, 255, 8, 255, 255, 255, 255, 255, 255, 255, 1, 52, 80, 0, 8, 8, 8, 0, 0, 8, 8, 0, 2, 33, 51, 114, 63 };
                //string msg ="赖以~~";
                client.SendTo(array1, point);
                client.SendTo(array2, point);
                client.SendTo(array3, point);
                client.SendTo(array4, point);
                client.SendTo(array5, point);
                client.SendTo(array6, point);
                client.SendTo(array7, point);
                client.SendTo(array8, point);
                //Console.WriteLine("客户端发送的数据：" + Encoding.UTF8.GetBytes(msg));
            }


        }
        private void zedGraphControl3_Load(object sender, EventArgs e)
        {

        }
    }
}

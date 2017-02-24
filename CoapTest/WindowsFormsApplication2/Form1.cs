using CoAP;
using CoAP.Server;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }
        static Socket server;
        static Socket client;
        //客户端
        private  void button1_Click(object sender, EventArgs e)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            client.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.75"), 6000));
            Thread t = new Thread(sendMsg);
            t.Start();
            Thread t2 = new Thread(ReciveMsg);
            t2.Start();
            Console.WriteLine("客户端已经开启");
        }
        /// <summary>
        /// 向特定ip的主机的端口发送数据报
        /// </summary>
        static void sendMsg()
        {
            EndPoint point = new IPEndPoint(IPAddress.Parse("192.168.1.75"), 6001);
            while (true)
            {
                string msg = Console.ReadLine();
                client.SendTo(Encoding.UTF8.GetBytes(msg), point);
            }
        }
        /// <summary>
        /// 接收发送给本机ip对应端口号的数据报
        /// </summary>
        static void ReciveMsg()
        {
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                byte[] buffer = new byte[1024];
                int length = client.ReceiveFrom(buffer, ref point);//接收数据报
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                Console.WriteLine("客户端接收 ："+point.ToString() + message);
            }
        }







        //服务端
        private void button2_Click(object sender, EventArgs e)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.75"), 6001));//绑定端口号和IP
            Console.WriteLine("服务端已经开启");
            Thread t = new Thread(ServerReciveMsg);//开启接收消息线程
            t.Start();
            Thread t2 = new Thread(ServersendMsg);//开启发送消息线程
            t2.Start();
        }
        static void ServersendMsg()
        {
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                byte[] buffer = new byte[1024];
                int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                Console.WriteLine("服务端发送："+point.ToString() + message);
            }
        }

        static void ServerReciveMsg()
        {
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                byte[] buffer = new byte[1024];
                int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                Console.WriteLine("服务端接收：" + point.ToString() + message);
            }
        }

    }
}

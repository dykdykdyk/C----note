using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;  

namespace UdpSecondConnect
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //开启服务端
        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse("192.168.1.75");//服务器端IP地址  
            IPEndPoint iep = new IPEndPoint(ip, 6001);//服务端地址及端口  
            //1.建立套接字，以Tcp协议链接，字节流的方式进行数据传输  
            Socket MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //设置Socket地址可重复使用  
            // MySocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);  
            //2.绑定套接字  
            MySocket.Bind(iep);
            //3.监听套接字  
            MySocket.Listen(10);
            //4.等待客户端请求  
            Socket MyClient = MySocket.Accept();
            //5.向客户端发送消息  
            MyClient.Send(Encoding.Unicode.GetBytes("链接成功"));
            //定义字节数组  
            byte[] buf = new byte[1024];
            //接收来自客户端的信息，并保存在字节数组中  
            int k = MyClient.Receive(buf);
            //输出显示客户端发送信息  
            Console.WriteLine(Encoding.Unicode.GetString(buf, 0, k));
            textBox1.Text = Encoding.Unicode.GetString(buf, 0, k)+"  服务端数据";
            //再次向客户端发送信息  
            MyClient.Send(Encoding.Unicode.GetBytes("客户你好"));
            //关闭套接字的发送与接收  
            MyClient.Shutdown(SocketShutdown.Both);
            //关闭MyClient  
            MyClient.Close();
            //关闭MySocket  
            MySocket.Close();  
        }
        //开启客户端
        private void button2_Click(object sender, EventArgs e)
        {
            //所连接服务器端IP地址  
            IPAddress remoteHost = IPAddress.Parse("192.168.1.75");
            //所连接服务端地址及端口  
            IPEndPoint iep = new IPEndPoint(remoteHost, 6001);
            //1.建立套接字，以Tcp协议链接，字节流的方式进行数据传输  
            Socket MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //2.建立与服务端的链接  
                MySocket.Connect(iep);
                //定义字节数组  
                byte[] buf = new byte[1024];
                //3.接收来自服务端的信息，并保存在字节数组中  
                int k = MySocket.Receive(buf);
                //输出显示服务端发送的信息  
                Console.WriteLine(Encoding.Unicode.GetString(buf, 0, k));
                //3.向服务端发送信息  
                MySocket.Send(Encoding.Unicode.GetBytes("服务器你好"));
                //接收来自服务端的的信息  
                k = MySocket.Receive(buf);
                //输出显示服务端发送的信息  
                Console.WriteLine(Encoding.Unicode.GetString(buf, 0, k));

            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("ArgumentNullException:{0}", ex.ToString());
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException:{0}", ex.ToString());
            }

            catch (Exception ex)
            {
                Console.WriteLine("Unexpection Exception:{0}", ex.ToString());
            }
            finally
            {
                //4.关闭套接字  
                //MySocket.Shutdown(SocketShutdown.Both);
                MySocket.Close();
            }

            Console.Read();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }



    }
}

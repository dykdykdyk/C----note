using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace UDP_client
{
    class Program
    {
        static Socket client;
        static void Main(string[] args)
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
            while(true){
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
                Console.WriteLine(point.ToString() + message);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZedGraphPlot
{
    class DataParser
    {
     /*
      数据解析类
      */   

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
    }
}

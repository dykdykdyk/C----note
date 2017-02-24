using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTemplate
{
    class Utils
    {
        /** 时间转换 */
        public static byte[] nowTimeToBytes()
        {
            byte[] result = new byte[4];
            int year = DateTime.Now.Year - 2016;
            int month = DateTime.Now.Month + 1;
            int day = DateTime.Now.Day;
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int second = DateTime.Now.Second;
            result[0] = (byte)((year << 2) | (month >> 2 & 0x03));
            result[1] = (byte)(((month & 0x03) << 6) | (day << 1) | (hour >> 4 & 0x01));
            result[2] = (byte)(((hour & 0x0F) << 4) | (minute >> 2 & 0x0F));
            result[3] = (byte)(((minute & 0x03) << 6) | second);
            return result;
        }
        /***
   * 添加一个闹钟
   */
        public static byte[] alarmToBytes11()
        {
            int year = 2016 - 2016;
            int month = 11;
            int day = 29;
            byte[] alarmByte = new byte[5];

            int hour = 17;
            int min = 45;
            int alarmID = 1;
            int dayFlags = 127;
            int a = 1;
            System.Diagnostics.Debug.WriteLine("1.设置的闹钟..." + year + "/" + month + "/" + day
                  + " 时:" + hour + "分" + min + "  重复日：" + dayFlags + " ID："
                  + alarmID + "开关：" + a);
            alarmByte[0] = (byte)((year << 2) | (month >> 2 & 0x03));
            alarmByte[1] = (byte)(((month & 0x03) << 6) | (day << 1) | (hour >> 4 & 0x01));
            alarmByte[2] = (byte)(((hour & 0x0F) << 4) | (min >> 2));
            alarmByte[3] = (byte)((min & 0X03) << 6 | (alarmID << 3));
            alarmByte[4] = (byte)((dayFlags & 0x7F) | ((a & 0x01) << 7));
            return alarmByte;
        }

        /** 用户信息设置转换 */
        public static byte[] userToByte(int sex, int age, int height, int weight)
        {
            byte[] userByte = new byte[4];
            userByte[0] = (byte)(sex | age);
            userByte[1] = (byte)(height >> 1);
            userByte[2] = (byte)((byte)((height & 0x01) << 8) | (weight >> 7));
            userByte[3] = (byte)(weight & 0x07 << 5);
            return userByte;
        }

        /**
	 * 添加两个
	 */
        public static byte[] alarmToBytes22()
        {
            int year = 2016 - 2016;
            int month = 11;
            int day = 29;

            byte[] alarmByte = new byte[5];
            int hour = 17;
            int min = 47;
            int alarmID = 2;
            int dayFlags = 127;
            int a = 1;
            System.Diagnostics.Debug.WriteLine("2.设置的闹钟..." + year + "/" + month + "/" + day
                    + " 时:" + hour + "分" + min + "  重复日：" + dayFlags + " ID："
                    + alarmID + "开关：" + a);

            alarmByte[0] = (byte)((year << 2) | (month >> 2 & 0x03));
            alarmByte[1] = (byte)(((month & 0x03) << 6) | (day << 1) | (hour >> 4 & 0x01));
            alarmByte[2] = (byte)(((hour & 0x0F) << 4) | (min >> 2));
            alarmByte[3] = (byte)((min & 0X03) << 6 | (alarmID << 3));
            alarmByte[4] = (byte)((dayFlags & 0x7F) | ((a & 0x01) << 7));
            return alarmByte;
        }
        /**
	 * 添加三个
	 */
        public static byte[] alarmToBytes33()
        {
            int year = 2016 - 2016;
            int month = 11;
            int day = 29;

            byte[] alarmByte = new byte[5];
            int hour = 17;
            int min = 39;
            int alarmID = 3;
            int dayFlags = 127;
            int a = 1;
            System.Diagnostics.Debug.WriteLine("3.设置的闹钟..." + year + "/" + month + "/" + day
                    + " 时:" + hour + "分" + min + "  重复日：" + dayFlags + " ID："
                    + alarmID + " 开关：" + a);

            alarmByte[0] = (byte)((year << 2) | (month >> 2 & 0x03));
            alarmByte[1] = (byte)(((month & 0x03) << 6) | (day << 1) | (hour >> 4 & 0x01));
            alarmByte[2] = (byte)(((hour & 0x0F) << 4) | (min >> 2));
            alarmByte[3] = (byte)((min & 0X03) << 6 | (alarmID << 3));
            alarmByte[4] = (byte)((dayFlags & 0x7F) | ((a & 0x01) << 7));
            return alarmByte;
        }
        //计步目标
        public static byte[] intToByteArray(int i)
        {
            byte[] result = new byte[4];
            //由高位到低位
            result[0] = (byte)((i >> 24) & 0xFF);
            result[1] = (byte)((i >> 16) & 0xFF);
            result[2] = (byte)((i >> 8) & 0xFF);
            result[3] = (byte)(i & 0xFF);
            return result;
        }
        /**longSit 久坐提醒*/
        public static byte[] longSitByte()
        {
            int longTime = 65530;

            byte[] lgSitByte = new byte[8];
            lgSitByte[0] = 0x00;
            lgSitByte[1] = 0x01; //0x00 关闭久坐提醒； 0x01打开久坐提醒
            Array.Copy(longTimeToByteArray(longTime), 0, lgSitByte, 3, longTimeToByteArray(longTime).Length);
            //		lgSitByte[2] = 10;   //阀值  步数第一这个才体现 0-65535 
            //		lgSitByte[3] = 10;   //阀值
            lgSitByte[4] = 5;   //久坐时间  以分钟为单位，超过这个时间才提醒  
            lgSitByte[5] = 0;   //开始时间
            lgSitByte[6] = 23;   //结束时间
            lgSitByte[7] = 127;    //重复日
            return lgSitByte;

        }

        public static byte[] strToByteArray(String str)
        {
            byte[] byBuffer = new byte[16];
            //UTF8  16 12   UTF7 16 13
            byBuffer = System.Text.UnicodeEncoding.UTF8.GetBytes(str);
            //byBuffer = System.Text.UTF8Encoding.Unicode.GetBytes(str);//24
            return byBuffer;
        }
        public static byte[] intArraysTobyteArrays(int[] data)
        {
            byte[] b = new byte[data.Length];

            for (int i = 0; i < data.Length; ++i)
            {
                b[i] = (byte)(data[i] & 255);
            }

            return b;
        }

        public static byte[] record_date(int year, int month, int day, int hours)
        {
            byte[] by = new byte[4];
            System.Diagnostics.Debug.WriteLine(year + "/" + month + "/" + day + "/" + hours);
            by[0] = (byte)((year << 4) | month);
            by[1] = (byte)((day << 3) | (hours >> 2));
            by[2] = (byte)((hours & 0x03) << 6);
            by[3] = 0;
            return by;
        }

        public static byte[] longTimeToByteArray(int i)
        {
            byte[] result = new byte[2];
            //由高位到低位
            result[0] = (byte)((i >> 8) & 0xFF);
            result[1] = (byte)((i >> 0) & 0xFF);

            return result;
        }
    }
}

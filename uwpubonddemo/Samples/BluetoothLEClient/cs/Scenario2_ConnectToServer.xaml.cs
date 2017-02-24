using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Runtime.InteropServices;
using Windows.System.Profile;
using Windows.Devices.Enumeration;
using Windows.Data.Xml.Dom;
using System.Threading.Tasks;

namespace SDKTemplate
{
    public sealed partial class Scenario2_ConnectToServer : Page, BLEDeviceListener
    {
        private MainPage rootPage = MainPage.Current;
        private byte[] cc;
        BLEDevice bleDevice;
        private String message;

        public Scenario2_ConnectToServer()
        {
            InitializeComponent();
            bleDevice = new BLEDevice(this);
            ResultsListView = new ListView();
            grid1.Children.Add(ResultsListView);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (string.IsNullOrEmpty(rootPage.SelectedBleDeviceId))
            {
                ConnectButton.IsEnabled = false;
            }
        }
        //离开
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            bleDevice.ClearSelectedCharacteristic();
            bleDevice.ClearBluetoothLEDevice();
        }

        #region Enumerating services
        private void ConnectButton_Click()
        {
            ConnectButton.IsEnabled = false;
            bleDevice.ClearBluetoothLEDevice();
            bleDevice.connect(rootPage.SelectedBleDeviceId, Dispatcher);
            ConnectButton.IsEnabled = true;
        }
        #endregion
        #region Accessing characteristics
        private  void CharacteristicNotifyButton_Click()
        {
            System.Diagnostics.Debug.WriteLine("  CharacteristicNotifyButton_Click ");
            bleDevice.CharacteristicNotify();
        }
        #endregion
        //设置相关命令
        private void comboBox1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            e.AddedItems.Add(comboBox1.SelectedIndex);
            string sUserId = comboBox1.SelectedIndex.ToString();
            System.Diagnostics.Debug.WriteLine("   Convert.ToString(sender)      " + Convert.ToString(sender));
            System.Diagnostics.Debug.WriteLine("  e    " + Convert.ToString(e));
            System.Diagnostics.Debug.WriteLine("  1      " + sUserId);
            if (sUserId.Equals("1")) {
                String message = " cmd：0x01 , 时间设置 ";
                ResultsListView.Items.Add(message);
                byte[] time = Utils.nowTimeToBytes();
                bleDevice.write( time.Length, 0x01, time);

            }
            else if (sUserId.Equals("2"))
            {
                System.Diagnostics.Debug.WriteLine("  闹钟设置      " + sUserId);
                String message = " cmd：0x02 , 闹钟设置 ";
                ResultsListView.Items.Add(message);
                byte[] result = new byte[15];
                byte[] alarmToBytes1 = Utils.alarmToBytes11();
                byte[] alarmToBytes2 = Utils.alarmToBytes22();
                byte[] alarmToBytes3 = Utils.alarmToBytes33();
                Array.Copy(alarmToBytes1, 0, result, 0, alarmToBytes1.Length);
                Array.Copy(alarmToBytes2, 0, result, 5, alarmToBytes2.Length);
                Array.Copy(alarmToBytes3, 0, result, 10, alarmToBytes3.Length);
                bleDevice.write(result.Length, 0x02, result);
            }
            else if (sUserId.Equals("3"))
            {
                String message = " cmd：0x03 , 获取设备闹钟列表请求 ";
                ResultsListView.Items.Add(message);
                byte[] alar = { 0x01 };
                 bleDevice.write(alar.Length, 0x03, alar);
            }
            else if (sUserId.Equals("4"))
            {
                String message = " cmd：0x04 , 计步目标设定 ";
                ResultsListView.Items.Add(message);
                byte[] b = Utils.intToByteArray(5000);
                 bleDevice.write(b.Length, 0x04, b);
            }
            else if (sUserId.Equals("5"))
            {
                String message = " cmd：0x05 , 用户信息设置命令 ";
                ResultsListView.Items.Add(message);
                byte[] user = Utils.userToByte(1, 20, 180, 60);
                 bleDevice.write( user.Length, 0x05, user);
            }
            else if (sUserId.Equals("6"))
            {
                String message = " cmd：0x06 , 防丢设置 ";
                ResultsListView.Items.Add(message);
                byte[] d = { 0x01 };
                bleDevice.write(d.Length, 0x06, d);
            }
            else if (sUserId.Equals("7"))
            {
                String message = " cmd：0x07 , 久坐提醒设置 ";
                ResultsListView.Items.Add(message);
                byte[] lgSit = Utils.longSitByte();
                bleDevice.write(lgSit.Length, 0x07, lgSit);
            }
            else if (sUserId.Equals("8"))
            {
                String message = " cmd：0x09 , 出厂设置 ";
                ResultsListView.Items.Add(message);
                byte[] r = { 0x00 };
                 bleDevice.write( r.Length, 0x09, r);
            }
            if (sUserId.Equals("9"))
            {
                // 云(0) 石(1) 智(2) 能(3)
                byte[] oneFonts = { 0x40, 0x40, 0x44, 0x44, 0x44, 0x44,
                            (byte) 0xC4, (byte) 0xC4, 0x44, 0x44, 0x46, 0x46,
                            0x64, 0x60, 0x40, 0x00, 0x00, 0x20, 0x70, 0x38,
                            0x2C, 0x27, 0x23, 0x31, 0x10, 0x12, 0x14, 0x18,
                            0x70, 0x20, 0x00, 0x00 };
                byte[] twoFonts = { 0x02, 0x02, 0x02, 0x02, (byte) 0x82,
                            (byte) 0xF2, 0x4E, 0x42, 0x42, 0x42, 0x42, 0x42,
                            (byte) 0xC2, 0x02, 0x02, 0x00, 0x10, 0x08, 0x04,
                            0x02, 0x01, 0x7F, 0x20, 0x20, 0x20, 0x20, 0x20,
                            0x20, 0x7F, 0x00, 0x00, 0x00 };
                byte[] threeFonts = { 0x10, 0x14, 0x13, (byte) 0x92, 0x7E,
                            0x32, 0x52, (byte) 0x92, 0x00, 0x7C, 0x44, 0x44,
                            0x44, 0x7C, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00,
                            (byte) 0xFF, 0x49, 0x49, 0x49, 0x49, 0x49, 0x49,
                            (byte) 0xFF, 0x00, 0x00, 0x00, 0x00 };
                byte[] fourFonts = { 0x10, (byte) 0xB8, (byte) 0x97,
                            (byte) 0x92, (byte) 0x90, (byte) 0x94, (byte) 0xB8,
                            0x10, 0x00, 0x7F, 0x48, 0x48, 0x44, 0x74, 0x20,
                            0x00, 0x00, (byte) 0xFF, 0x0A, 0x0A, 0x4A,
                            (byte) 0x8A, 0x7F, 0x00, 0x00, 0x3F, 0x44, 0x44,
                            0x42, 0x72, 0x20, 0x00 };
                byte[] time = Utils.nowTimeToBytes();
                String message = " cmd：0x0A , 名字设置 ";
                ResultsListView.Items.Add(message);
                bleDevice.wirteUserName(0x0A, oneFonts, twoFonts, threeFonts,
                            fourFonts);
            }
            else if (sUserId.Equals("10"))
            {
                String message = " cmd：0x0B , 图片设置 ";
                ResultsListView.Items.Add(message);
                int index1 = 1; //组编号
                byte[] data = new byte[128 * 8];
                Array.Copy(Image.a1, 0, data, 128 * 0, Image.a1.Length);
                Array.Copy(Image.a2, 0, data, 128 * 1, Image.a2.Length);
                Array.Copy(Image.a3, 0, data, 128 * 2, Image.a3.Length);
                Array.Copy(Image.a4, 0, data, 128 * 3, Image.a4.Length);
                Array.Copy(Image.a5, 0, data, 128 * 4, Image.a5.Length);
                Array.Copy(Image.a6, 0, data, 128 * 5, Image.a5.Length);
                Array.Copy(Image.a7, 0, data, 128 * 6, Image.a5.Length);
                Array.Copy(Image.a8, 0, data, 128 * 7, Image.a5.Length);
                bleDevice.writeImage( 0x0B, index1, data);

            }
            else if (sUserId.Equals("11"))
            {
                String message = " cmd：0x0C , 清空图片组 ";
                ResultsListView.Items.Add(message);
                byte[] index = { 0x01 };
                bleDevice.write(index.Length, 0x0C, index);
            }
            else if (sUserId.Equals("12"))
            {
                String message = " cmd：0x0D , 图片文字设置 ";
                ResultsListView.Items.Add(message);
                String[] image_name = { "Home", "Baking", "Party", "Room", "Watching", "A", "B", "C" };
                int n1 = 1;
                bleDevice.writeImageName( 0x0D, n1, image_name);
            }
            else if (sUserId.Equals("13"))
            {
                //协议未实现
                byte[] indexNameClear = { 0x00 };
                String message = " cmd：0x0E , 清空图片文字组 ";
                ResultsListView.Items.Add(message);
                 bleDevice.write(indexNameClear.Length, 0x0E, indexNameClear);
            }
            else if (sUserId.Equals("14"))
            {
                byte[] data13 = new byte[128 * 6];
                Array.Copy(Utils.intArraysTobyteArrays(Image.b0), 0, data13, 128 * 0, Image.b0.Length);
                Array.Copy(Utils.intArraysTobyteArrays(Image.b1), 0, data13, 128 * 1, Image.b1.Length);
                Array.Copy(Utils.intArraysTobyteArrays(Image.b2), 0, data13, 128 * 2, Image.b2.Length);
                Array.Copy(Utils.intArraysTobyteArrays(Image.b3), 0, data13, 128 * 3, Image.b3.Length);
                Array.Copy(Utils.intArraysTobyteArrays(Image.b4), 0, data13, 128 * 4, Image.b4.Length);
                Array.Copy(Utils.intArraysTobyteArrays(Image.b5), 0, data13, 128 * 5, Image.b5.Length);
                int index0 = 0;
                String message = " cmd：0x0B , 图片0组 ";
                ResultsListView.Items.Add(message);
                bleDevice.writeImage(0x0B, index0, data13);
            }
            else if (sUserId.Equals("15"))
            {
                //协议未实现
                String message = " cmd：0x0C , 图片文字设置 ";
                ResultsListView.Items.Add(message);
                int nn = 0; //那一组图片的文字 0~9
                String[] image_name_0 = { "Base", "BBQ", "Camping", "Friends", "Music", "Self" };
                bleDevice.writeImageName(0x0D, nn, image_name_0);
            }

        }
        //消息相关命令
        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sUserId = comboBox2.SelectedIndex.ToString();
            System.Diagnostics.Debug.WriteLine("  2      " + sUserId);
            if (sUserId.Equals("1"))
            {
                String message = " cmd：0x11 , 手机请求设备报警 ";
                ResultsListView.Items.Add(message);
                byte[] a = { 0x02 };
              bleDevice.write(a.Length, 0x11, a);
            }
            else if (sUserId.Equals("2"))
            {
                String message = " cmd：0x14 , 来电提醒 ";
                ResultsListView.Items.Add(message);
                //byte[] st = { 0x3E, 0x14, 0, 0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0, 0, 0, 0, 0, 0, 0 };
                //            cc = write(st.Length, 0x14, st);
                String str = "+12345678910";
                byte[] st = Utils.strToByteArray(str);
                bleDevice.write(st.Length, 0x14, st);
            }
            else if (sUserId.Equals("3"))
            {
                String message = " cmd：0x15 , 来电已接听 ";
                ResultsListView.Items.Add(message);
                byte[] s = { 0x00 };
                 bleDevice.write(s.Length, 0x15, s);
            }
            else if (sUserId.Equals("4"))
            {
                String message = " cmd：0x16 , 来电已拒接 ";
                ResultsListView.Items.Add(message);
                byte[] j = { 0x00 };
                 bleDevice.write(j.Length, 0x16, j);
            }
            else if (sUserId.Equals("5"))
            {
                String message = " cmd：0x17 , 短信提醒 ";
                ResultsListView.Items.Add(message);
                String d = "+12345678910";
                byte[] dx = Utils.strToByteArray(d);
                //byte[] st = { 0x3E, 0x17, 0, 0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0, 0, 0, 0, 0, 0, 0 };
                bleDevice.write(dx.Length, 0x17, dx);
             }
        }
     
        //安全相关命令
        private void comboBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          byte[]  bluAddr = { 1,2,3};
        
            string sUserId = comboBox3.SelectedIndex.ToString();
            System.Diagnostics.Debug.WriteLine("  3      " + sUserId);
            if (sUserId.Equals("1"))
            {
                String message = " cmd：0x22 , 手机请求删除绑定 ";
                ResultsListView.Items.Add(message);
                bleDevice.write(16, 0x22, bluAddr);

            }
            else if (sUserId.Equals("2"))
            {
                byte[] SUPER_BOUND_DATA = { 0x01, 0x23, 0x45, 0X67, (byte)0x89, (byte)0xAB, (byte)0xCD, (byte)0xEF, (byte)0xFE, (byte)0xDC, (byte)0xBA, (byte)0x98, 0x76, 0x54, 0x32, 0x10 };
                String message = " cmd：0x24 , 超级绑定 ";
                ResultsListView.Items.Add(message);
                bleDevice.write(SUPER_BOUND_DATA.Length, 0x24, SUPER_BOUND_DATA);
               
            }
            else if (sUserId.Equals("3"))
            {
                String message = " cmd：0x23 , 手机请求删除绑定 ";
                ResultsListView.Items.Add(message);
                bleDevice.write( 16, 0x23, bluAddr);
            }
            else if (sUserId.Equals("4"))
            {
                String message = " cmd：0x26 , 手机请求蓝牙设备MAC地址 ";
                ResultsListView.Items.Add(message);
                byte[] d = { 0x00 };
                bleDevice.write( 1, 0x26, d);
            }
            else if (sUserId.Equals("5"))
            {
                String message = " cmd：0x25 , 手机请求蓝牙设备E号 ";
                ResultsListView.Items.Add(message);
                byte[] ee = { 0x00 };
                 bleDevice.write( 1, 0x25, ee);
            }
            else if (sUserId.Equals("6"))
            {
                String message = " cmd：0x21 , 手机请求绑定 ";
                ResultsListView.Items.Add(message);
                bleDevice.write(16, 0x21, bluAddr);
            }
            else if (sUserId.Equals("7"))
            {
                String message = " cmd：0x28 , 请求获得设备特性 ";
                ResultsListView.Items.Add(message);
                byte[] t = { 0x00 };
              bleDevice.write(t.Length, 0x28, t);
            }
            else if (sUserId.Equals("8"))
            {
                String message = " cmd：0x27 , 请求断开 ";
                ResultsListView.Items.Add(message);
                bleDevice.write(16, 0x27, bluAddr);
            }
            else if (sUserId.Equals("9"))
            {
                System.Diagnostics.Debug.WriteLine("  连接      " + sUserId);
                ConnectButton_Click();
            }
           
        }

      
        //运动相关命令
        private void comboBox4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sUserId = comboBox4.SelectedIndex.ToString();
            System.Diagnostics.Debug.WriteLine(" 4      " + sUserId);
            if (sUserId.Equals("1"))
            {
                String message = " cmd：0x31 , 请求实时运动数据 ";
                ResultsListView.Items.Add(message);
                byte[] s = { 0x01 };
                 bleDevice.write( s.Length, 0x31, s);
            }
            else if (sUserId.Equals("2"))
            {
                String message = " cmd：0x35 , 请求历史运动数据 ";
                ResultsListView.Items.Add(message);
                byte[] sh = { 0x01 };
                 bleDevice.write(sh.Length, 0x35, sh);
            }
            else if (sUserId.Equals("3"))
            {
                String message = " cmd：0x37 , 请求实时气压数据 ";
                ResultsListView.Items.Add(message);
                byte[] qy = { 0x01 };
                bleDevice.write(qy.Length, 0x37, qy);
            }
            else if (sUserId.Equals("4"))
            {
                String message = " cmd：0x32 , 请求挪动历史步数数据指针 ";
                ResultsListView.Items.Add(message);
                byte[] aa = Utils.record_date(2016, 12, 17, 0);
                bleDevice.write( aa.Length, 0x32, aa);
            }
            else if(sUserId.Equals("5"))
            {
                String message = " cmd：0x31 , 请求最近一天的总睡眠时间 ";
                ResultsListView.Items.Add(message);
                byte[] sleep = { 0x01 };
                bleDevice.write(sleep.Length, 0x33, sleep);
            }
            else if (sUserId.Equals("6"))
            {
                byte[] hisSleep = { 0x01 };
                String message = " cmd：0x34 , 请求详细历史睡眠时间 ";
                ResultsListView.Items.Add(message);
                bleDevice.write(hisSleep.Length, 0x34, hisSleep);
            }
            else if (sUserId.Equals("7"))
            {
                String message = " cmd：0x39 , 请求挪动历史睡眠数据指针 ";
                ResultsListView.Items.Add(message);
                byte[] sleepzhizhen = Utils.record_date(2016, 12, 17, 0); // 年，月，日，时
                bleDevice.write(sleepzhizhen.Length, 0x39, sleepzhizhen);
            }
            else if (sUserId.Equals("8"))
            {
                byte[] locaiton = { 0x01 };
                String message = " cmd：0x3A , 请求当前位置动作数据 ";
                ResultsListView.Items.Add(message);
                bleDevice.write(locaiton.Length, 0x3A, locaiton);
            }
            else if (sUserId.Equals("9"))
            {
                byte[] hislocaiton = { 0x01 };
                String message = " cmd：0x3B , 请求历史位置动作数据 ";
                ResultsListView.Items.Add(message);
                bleDevice.write(hislocaiton.Length, 0x3B, hislocaiton);
            }
            else if (sUserId.Equals("10"))
            {
                byte[] locaitonTime = Utils.record_date(2016, 12, 17, 0); // 年，月，日，时
                String message = " cmd：0x3C , 请求挪动历史位置动作数据 挪动时间：" + "2016-12-17 00:00";
                ResultsListView.Items.Add(message);
                bleDevice.write(locaitonTime.Length, 0x3C, locaitonTime);
            }

        }
        //水杯相关命令  暂时未写
        private void comboBox5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sUserId = comboBox5.SelectedIndex.ToString();
            System.Diagnostics.Debug.WriteLine("  5      " + sUserId);
            if (sUserId.Equals("1"))
            {
                byte[] zx = { 0x01 };
                String message = " cmd：0x59 , 请求药品剂量数据 ";
                ResultsListView.Items.Add(message);
                bleDevice.write(zx.Length, 0x59, zx);
            }
            else if (sUserId.Equals("2"))
            {
                /**由于蓝牙硬件的特殊性，需要断开连接才能删除，所以设备在收到 APP 指令后，会
//					迅速返回一个成功应答，此时 APP 必须主动断开蓝牙连接（Major Command 2, Minor
//					Command 7, 0x27） ，才进行删除工作，由于药品剂量数据总共有 10 个页面，每个页面删
//					除需要 400ms，APP 在断开连接后，请等待 4 秒以上才开始重新连接。*/
                byte[] del = { 0x00 };
                String message = " cmd：0x5A , 请求删除药品剂量数据 ";
                ResultsListView.Items.Add(message);
                bleDevice.write(del.Length, 0x5A, del);
            }
            else if (sUserId.Equals("3"))
            {
                byte[] set = { 0x05 }; // 代表要设置的药品类型，范围是  0～254
                String message = " cmd：0x5B , 请求设置药品类型 ";
                ResultsListView.Items.Add(message);
                bleDevice.write(set.Length, 0x5B, set);
            }
            //else if (sUserId.Equals("4"))
            //{
            //    System.Diagnostics.Debug.WriteLine("  请求挪动历史步数数据指针      " + sUserId);
            //    byte[] aa = record_date(2016, 12, 1, 0, 0);
            //    write(aa.Length, 0x32, aa);
            //}
        }
        //健康相关命令
        private void comboBox6_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sUserId = comboBox6.SelectedIndex.ToString();
            System.Diagnostics.Debug.WriteLine("  6      " + sUserId);
            if (sUserId.Equals("1"))
            {
                String message = " cmd：0x41 , 请求实时心率数据 ";
                ResultsListView.Items.Add(message);
                byte[] dx = { 0x01 };
             bleDevice.write(dx.Length, 0x41, dx);
            }
            else if (sUserId.Equals("2"))
            {
                String message = " cmd：0x43 , 请求历史心率数据 ";
                ResultsListView.Items.Add(message);
                byte[] hh = { 0x01 };
               bleDevice.write(hh.Length, 0x43, hh);
            }
            else if (sUserId.Equals("3"))
            {
                String message = " cmd：0x44 , 请求体温数据 ";
                ResultsListView.Items.Add(message);
                byte[] t = { 0x01 };
                 bleDevice.write(t.Length, 0x44, t);
            }
            else if (sUserId.Equals("4"))
            {
                String message = " cmd：0x46 , 请求历史温度数据 ";
                ResultsListView.Items.Add(message);
                byte[] th = { 0x01 };
                 bleDevice.write(th.Length, 0x46, th);
            }
            else if (sUserId.Equals("5"))
            {
                String message = " cmd：0x49 ,  请求挪动历史心率数据指针 ";
                ResultsListView.Items.Add(message);
                byte[] ah = Utils.record_date(2016, 12, 17, 0);
                bleDevice.write(ah.Length, 0x49, ah);
            }
            else if (sUserId.Equals("6"))
            {
                // 请求挪动历史体温数据指针
                String message = " cmd：0x4A , 请求挪动历史体温数据指针 " + "2016-12-17 00:00";
                ResultsListView.Items.Add(message);
                byte[] at = Utils.record_date(2016, 12, 17, 0);
                bleDevice.write(at.Length, 0x4A, at);
            }
        }
        public void Connected(string address)
        {
            try
            {
                if (address.Equals("连接成功")) {
                    ConnectButton.Visibility = Visibility.Collapsed;
                }
                ResultsListView.Items.Add(address);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public void onpaired(String result)
        {
            try
            {
                ResultsListView.Items.Add(result);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }


        public void onSendResult(string address)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("  address      " + address);
                ResultsListView.Items.Add(address);
            }
            catch
            {
                throw new NotImplementedException();
            }
            
           
        }
    }
}

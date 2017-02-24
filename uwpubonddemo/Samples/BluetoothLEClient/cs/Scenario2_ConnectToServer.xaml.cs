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
        //�뿪
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
        //�����������
        private void comboBox1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            e.AddedItems.Add(comboBox1.SelectedIndex);
            string sUserId = comboBox1.SelectedIndex.ToString();
            System.Diagnostics.Debug.WriteLine("   Convert.ToString(sender)      " + Convert.ToString(sender));
            System.Diagnostics.Debug.WriteLine("  e    " + Convert.ToString(e));
            System.Diagnostics.Debug.WriteLine("  1      " + sUserId);
            if (sUserId.Equals("1")) {
                String message = " cmd��0x01 , ʱ������ ";
                ResultsListView.Items.Add(message);
                byte[] time = Utils.nowTimeToBytes();
                bleDevice.write( time.Length, 0x01, time);

            }
            else if (sUserId.Equals("2"))
            {
                System.Diagnostics.Debug.WriteLine("  ��������      " + sUserId);
                String message = " cmd��0x02 , �������� ";
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
                String message = " cmd��0x03 , ��ȡ�豸�����б����� ";
                ResultsListView.Items.Add(message);
                byte[] alar = { 0x01 };
                 bleDevice.write(alar.Length, 0x03, alar);
            }
            else if (sUserId.Equals("4"))
            {
                String message = " cmd��0x04 , �Ʋ�Ŀ���趨 ";
                ResultsListView.Items.Add(message);
                byte[] b = Utils.intToByteArray(5000);
                 bleDevice.write(b.Length, 0x04, b);
            }
            else if (sUserId.Equals("5"))
            {
                String message = " cmd��0x05 , �û���Ϣ�������� ";
                ResultsListView.Items.Add(message);
                byte[] user = Utils.userToByte(1, 20, 180, 60);
                 bleDevice.write( user.Length, 0x05, user);
            }
            else if (sUserId.Equals("6"))
            {
                String message = " cmd��0x06 , �������� ";
                ResultsListView.Items.Add(message);
                byte[] d = { 0x01 };
                bleDevice.write(d.Length, 0x06, d);
            }
            else if (sUserId.Equals("7"))
            {
                String message = " cmd��0x07 , ������������ ";
                ResultsListView.Items.Add(message);
                byte[] lgSit = Utils.longSitByte();
                bleDevice.write(lgSit.Length, 0x07, lgSit);
            }
            else if (sUserId.Equals("8"))
            {
                String message = " cmd��0x09 , �������� ";
                ResultsListView.Items.Add(message);
                byte[] r = { 0x00 };
                 bleDevice.write( r.Length, 0x09, r);
            }
            if (sUserId.Equals("9"))
            {
                // ��(0) ʯ(1) ��(2) ��(3)
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
                String message = " cmd��0x0A , �������� ";
                ResultsListView.Items.Add(message);
                bleDevice.wirteUserName(0x0A, oneFonts, twoFonts, threeFonts,
                            fourFonts);
            }
            else if (sUserId.Equals("10"))
            {
                String message = " cmd��0x0B , ͼƬ���� ";
                ResultsListView.Items.Add(message);
                int index1 = 1; //����
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
                String message = " cmd��0x0C , ���ͼƬ�� ";
                ResultsListView.Items.Add(message);
                byte[] index = { 0x01 };
                bleDevice.write(index.Length, 0x0C, index);
            }
            else if (sUserId.Equals("12"))
            {
                String message = " cmd��0x0D , ͼƬ�������� ";
                ResultsListView.Items.Add(message);
                String[] image_name = { "Home", "Baking", "Party", "Room", "Watching", "A", "B", "C" };
                int n1 = 1;
                bleDevice.writeImageName( 0x0D, n1, image_name);
            }
            else if (sUserId.Equals("13"))
            {
                //Э��δʵ��
                byte[] indexNameClear = { 0x00 };
                String message = " cmd��0x0E , ���ͼƬ������ ";
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
                String message = " cmd��0x0B , ͼƬ0�� ";
                ResultsListView.Items.Add(message);
                bleDevice.writeImage(0x0B, index0, data13);
            }
            else if (sUserId.Equals("15"))
            {
                //Э��δʵ��
                String message = " cmd��0x0C , ͼƬ�������� ";
                ResultsListView.Items.Add(message);
                int nn = 0; //��һ��ͼƬ������ 0~9
                String[] image_name_0 = { "Base", "BBQ", "Camping", "Friends", "Music", "Self" };
                bleDevice.writeImageName(0x0D, nn, image_name_0);
            }

        }
        //��Ϣ�������
        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sUserId = comboBox2.SelectedIndex.ToString();
            System.Diagnostics.Debug.WriteLine("  2      " + sUserId);
            if (sUserId.Equals("1"))
            {
                String message = " cmd��0x11 , �ֻ������豸���� ";
                ResultsListView.Items.Add(message);
                byte[] a = { 0x02 };
              bleDevice.write(a.Length, 0x11, a);
            }
            else if (sUserId.Equals("2"))
            {
                String message = " cmd��0x14 , �������� ";
                ResultsListView.Items.Add(message);
                //byte[] st = { 0x3E, 0x14, 0, 0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0, 0, 0, 0, 0, 0, 0 };
                //            cc = write(st.Length, 0x14, st);
                String str = "+12345678910";
                byte[] st = Utils.strToByteArray(str);
                bleDevice.write(st.Length, 0x14, st);
            }
            else if (sUserId.Equals("3"))
            {
                String message = " cmd��0x15 , �����ѽ��� ";
                ResultsListView.Items.Add(message);
                byte[] s = { 0x00 };
                 bleDevice.write(s.Length, 0x15, s);
            }
            else if (sUserId.Equals("4"))
            {
                String message = " cmd��0x16 , �����Ѿܽ� ";
                ResultsListView.Items.Add(message);
                byte[] j = { 0x00 };
                 bleDevice.write(j.Length, 0x16, j);
            }
            else if (sUserId.Equals("5"))
            {
                String message = " cmd��0x17 , �������� ";
                ResultsListView.Items.Add(message);
                String d = "+12345678910";
                byte[] dx = Utils.strToByteArray(d);
                //byte[] st = { 0x3E, 0x17, 0, 0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0, 0, 0, 0, 0, 0, 0 };
                bleDevice.write(dx.Length, 0x17, dx);
             }
        }
     
        //��ȫ�������
        private void comboBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          byte[]  bluAddr = { 1,2,3};
        
            string sUserId = comboBox3.SelectedIndex.ToString();
            System.Diagnostics.Debug.WriteLine("  3      " + sUserId);
            if (sUserId.Equals("1"))
            {
                String message = " cmd��0x22 , �ֻ�����ɾ���� ";
                ResultsListView.Items.Add(message);
                bleDevice.write(16, 0x22, bluAddr);

            }
            else if (sUserId.Equals("2"))
            {
                byte[] SUPER_BOUND_DATA = { 0x01, 0x23, 0x45, 0X67, (byte)0x89, (byte)0xAB, (byte)0xCD, (byte)0xEF, (byte)0xFE, (byte)0xDC, (byte)0xBA, (byte)0x98, 0x76, 0x54, 0x32, 0x10 };
                String message = " cmd��0x24 , ������ ";
                ResultsListView.Items.Add(message);
                bleDevice.write(SUPER_BOUND_DATA.Length, 0x24, SUPER_BOUND_DATA);
               
            }
            else if (sUserId.Equals("3"))
            {
                String message = " cmd��0x23 , �ֻ�����ɾ���� ";
                ResultsListView.Items.Add(message);
                bleDevice.write( 16, 0x23, bluAddr);
            }
            else if (sUserId.Equals("4"))
            {
                String message = " cmd��0x26 , �ֻ����������豸MAC��ַ ";
                ResultsListView.Items.Add(message);
                byte[] d = { 0x00 };
                bleDevice.write( 1, 0x26, d);
            }
            else if (sUserId.Equals("5"))
            {
                String message = " cmd��0x25 , �ֻ����������豸E�� ";
                ResultsListView.Items.Add(message);
                byte[] ee = { 0x00 };
                 bleDevice.write( 1, 0x25, ee);
            }
            else if (sUserId.Equals("6"))
            {
                String message = " cmd��0x21 , �ֻ������ ";
                ResultsListView.Items.Add(message);
                bleDevice.write(16, 0x21, bluAddr);
            }
            else if (sUserId.Equals("7"))
            {
                String message = " cmd��0x28 , �������豸���� ";
                ResultsListView.Items.Add(message);
                byte[] t = { 0x00 };
              bleDevice.write(t.Length, 0x28, t);
            }
            else if (sUserId.Equals("8"))
            {
                String message = " cmd��0x27 , ����Ͽ� ";
                ResultsListView.Items.Add(message);
                bleDevice.write(16, 0x27, bluAddr);
            }
            else if (sUserId.Equals("9"))
            {
                System.Diagnostics.Debug.WriteLine("  ����      " + sUserId);
                ConnectButton_Click();
            }
           
        }

      
        //�˶��������
        private void comboBox4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sUserId = comboBox4.SelectedIndex.ToString();
            System.Diagnostics.Debug.WriteLine(" 4      " + sUserId);
            if (sUserId.Equals("1"))
            {
                String message = " cmd��0x31 , ����ʵʱ�˶����� ";
                ResultsListView.Items.Add(message);
                byte[] s = { 0x01 };
                 bleDevice.write( s.Length, 0x31, s);
            }
            else if (sUserId.Equals("2"))
            {
                String message = " cmd��0x35 , ������ʷ�˶����� ";
                ResultsListView.Items.Add(message);
                byte[] sh = { 0x01 };
                 bleDevice.write(sh.Length, 0x35, sh);
            }
            else if (sUserId.Equals("3"))
            {
                String message = " cmd��0x37 , ����ʵʱ��ѹ���� ";
                ResultsListView.Items.Add(message);
                byte[] qy = { 0x01 };
                bleDevice.write(qy.Length, 0x37, qy);
            }
            else if (sUserId.Equals("4"))
            {
                String message = " cmd��0x32 , ����Ų����ʷ��������ָ�� ";
                ResultsListView.Items.Add(message);
                byte[] aa = Utils.record_date(2016, 12, 17, 0);
                bleDevice.write( aa.Length, 0x32, aa);
            }
            else if(sUserId.Equals("5"))
            {
                String message = " cmd��0x31 , �������һ�����˯��ʱ�� ";
                ResultsListView.Items.Add(message);
                byte[] sleep = { 0x01 };
                bleDevice.write(sleep.Length, 0x33, sleep);
            }
            else if (sUserId.Equals("6"))
            {
                byte[] hisSleep = { 0x01 };
                String message = " cmd��0x34 , ������ϸ��ʷ˯��ʱ�� ";
                ResultsListView.Items.Add(message);
                bleDevice.write(hisSleep.Length, 0x34, hisSleep);
            }
            else if (sUserId.Equals("7"))
            {
                String message = " cmd��0x39 , ����Ų����ʷ˯������ָ�� ";
                ResultsListView.Items.Add(message);
                byte[] sleepzhizhen = Utils.record_date(2016, 12, 17, 0); // �꣬�£��գ�ʱ
                bleDevice.write(sleepzhizhen.Length, 0x39, sleepzhizhen);
            }
            else if (sUserId.Equals("8"))
            {
                byte[] locaiton = { 0x01 };
                String message = " cmd��0x3A , ����ǰλ�ö������� ";
                ResultsListView.Items.Add(message);
                bleDevice.write(locaiton.Length, 0x3A, locaiton);
            }
            else if (sUserId.Equals("9"))
            {
                byte[] hislocaiton = { 0x01 };
                String message = " cmd��0x3B , ������ʷλ�ö������� ";
                ResultsListView.Items.Add(message);
                bleDevice.write(hislocaiton.Length, 0x3B, hislocaiton);
            }
            else if (sUserId.Equals("10"))
            {
                byte[] locaitonTime = Utils.record_date(2016, 12, 17, 0); // �꣬�£��գ�ʱ
                String message = " cmd��0x3C , ����Ų����ʷλ�ö������� Ų��ʱ�䣺" + "2016-12-17 00:00";
                ResultsListView.Items.Add(message);
                bleDevice.write(locaitonTime.Length, 0x3C, locaitonTime);
            }

        }
        //ˮ���������  ��ʱδд
        private void comboBox5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sUserId = comboBox5.SelectedIndex.ToString();
            System.Diagnostics.Debug.WriteLine("  5      " + sUserId);
            if (sUserId.Equals("1"))
            {
                byte[] zx = { 0x01 };
                String message = " cmd��0x59 , ����ҩƷ�������� ";
                ResultsListView.Items.Add(message);
                bleDevice.write(zx.Length, 0x59, zx);
            }
            else if (sUserId.Equals("2"))
            {
                /**��������Ӳ���������ԣ���Ҫ�Ͽ����Ӳ���ɾ���������豸���յ� APP ָ��󣬻�
//					Ѹ�ٷ���һ���ɹ�Ӧ�𣬴�ʱ APP ���������Ͽ��������ӣ�Major Command 2, Minor
//					Command 7, 0x27�� ���Ž���ɾ������������ҩƷ���������ܹ��� 10 ��ҳ�棬ÿ��ҳ��ɾ
//					����Ҫ 400ms��APP �ڶϿ����Ӻ���ȴ� 4 �����ϲſ�ʼ�������ӡ�*/
                byte[] del = { 0x00 };
                String message = " cmd��0x5A , ����ɾ��ҩƷ�������� ";
                ResultsListView.Items.Add(message);
                bleDevice.write(del.Length, 0x5A, del);
            }
            else if (sUserId.Equals("3"))
            {
                byte[] set = { 0x05 }; // ����Ҫ���õ�ҩƷ���ͣ���Χ��  0��254
                String message = " cmd��0x5B , ��������ҩƷ���� ";
                ResultsListView.Items.Add(message);
                bleDevice.write(set.Length, 0x5B, set);
            }
            //else if (sUserId.Equals("4"))
            //{
            //    System.Diagnostics.Debug.WriteLine("  ����Ų����ʷ��������ָ��      " + sUserId);
            //    byte[] aa = record_date(2016, 12, 1, 0, 0);
            //    write(aa.Length, 0x32, aa);
            //}
        }
        //�����������
        private void comboBox6_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sUserId = comboBox6.SelectedIndex.ToString();
            System.Diagnostics.Debug.WriteLine("  6      " + sUserId);
            if (sUserId.Equals("1"))
            {
                String message = " cmd��0x41 , ����ʵʱ�������� ";
                ResultsListView.Items.Add(message);
                byte[] dx = { 0x01 };
             bleDevice.write(dx.Length, 0x41, dx);
            }
            else if (sUserId.Equals("2"))
            {
                String message = " cmd��0x43 , ������ʷ�������� ";
                ResultsListView.Items.Add(message);
                byte[] hh = { 0x01 };
               bleDevice.write(hh.Length, 0x43, hh);
            }
            else if (sUserId.Equals("3"))
            {
                String message = " cmd��0x44 , ������������ ";
                ResultsListView.Items.Add(message);
                byte[] t = { 0x01 };
                 bleDevice.write(t.Length, 0x44, t);
            }
            else if (sUserId.Equals("4"))
            {
                String message = " cmd��0x46 , ������ʷ�¶����� ";
                ResultsListView.Items.Add(message);
                byte[] th = { 0x01 };
                 bleDevice.write(th.Length, 0x46, th);
            }
            else if (sUserId.Equals("5"))
            {
                String message = " cmd��0x49 ,  ����Ų����ʷ��������ָ�� ";
                ResultsListView.Items.Add(message);
                byte[] ah = Utils.record_date(2016, 12, 17, 0);
                bleDevice.write(ah.Length, 0x49, ah);
            }
            else if (sUserId.Equals("6"))
            {
                // ����Ų����ʷ��������ָ��
                String message = " cmd��0x4A , ����Ų����ʷ��������ָ�� " + "2016-12-17 00:00";
                ResultsListView.Items.Add(message);
                byte[] at = Utils.record_date(2016, 12, 17, 0);
                bleDevice.write(at.Length, 0x4A, at);
            }
        }
        public void Connected(string address)
        {
            try
            {
                if (address.Equals("���ӳɹ�")) {
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

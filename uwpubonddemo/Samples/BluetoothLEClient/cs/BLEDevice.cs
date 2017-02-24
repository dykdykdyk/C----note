using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace SDKTemplate
{
    class BLEDevice
    {
        BLEDeviceListener mListener;
        public Guid guidService = new Guid("00001803-0000-1000-8000-00805f9b34fb");
        public Guid guidCharacteristic = new Guid("00002a06-0000-1000-8000-00805f9b34fb");
        public GattDeviceService gattdeviceService;
        private bool isValueChangedHandlerRegistered = false;
        private GattPresentationFormat presentationFormat ;
        IReadOnlyList<GattCharacteristic> mgattCharacteristic;
        private BluetoothLEDevice bluetoothLeDevice = null;
        public CoreDispatcher mdispatcher;
        byte[] image_name_data;
        String Message;
        IBuffer buffer;
        int mNumber;
        byte[] image_data;
        byte[] image_data_send = new byte[16];
        public static  String[] RESPONE_STATE = { 
		    "成功", "版本号不正确，此协议只接受1",
			"长度信息和命令要求不匹配", "类型信息和命令要求不匹配", "命令不存在", "序列号不正常", "设备已经被绑定",
			"绑定信息和设备内部不匹配，无法删除绑定", "登录信息和设备内部不匹配，无法登录", "还没有登录，先登录先",
			"指令不支持，很多指令是设备发出去的，并不能接收，参考具体指令介绍","指针移动失败，一般命令格式不对或者是指针已经移动到最末尾位置",
			"包数据不完整","Data 不正确","Param 不正确","内存不够" ,"指令内部返回，不走标准返回模式" };
        public BLEDevice(BLEDeviceListener listener)
        {
            this.mListener = listener;
            
        }
        public async void pair(BluetoothLEDeviceDisplay bleDeviceDisplay, CoreDispatcher dispatcher)
        {
            mdispatcher = dispatcher;
            DevicePairingResult result = await bleDeviceDisplay.DeviceInformation.Pairing.PairAsync();
            if (result.Status == DevicePairingResultStatus.Paired || result.Status == DevicePairingResultStatus.AlreadyPaired)
            {
 //               rootPage.NotifyUser($"配对成功", NotifyType.StatusMessage);
                System.Diagnostics.Debug.WriteLine("配对成功  ");
                Message = "配对成功";
            }
            else
            {
//                rootPage.NotifyUser($"配对失败", NotifyType.StatusMessage);
                System.Diagnostics.Debug.WriteLine("配对失败  ");
                Message = "配对失败";
            }
            await mdispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                mListener.onpaired(Message);
            });
        }
        private int sid = 0;
        public void write(int length, int cmd, byte[] data)
        {
            sid++;
            int v = 1;
            int t = 0;
            byte[] va = new byte[20];
            va[0] = (byte)((v << 5) | ((length - 1) << 1) | t);
            va[1] = (byte)(cmd & 0xFF);
            va[2] = (byte)(sid >> 4);
            va[3] = (byte)(sid & 0x0F);
            Array.Copy(data, 0, va, 4, data.Length);
            //return va;

            System.Diagnostics.Debug.WriteLine("写入数据：  ");
            for (int i = 0; i < va.Length; i++)
            {
                System.Diagnostics.Debug.Write(" " + va[i] + " ");
            }
            System.Diagnostics.Debug.WriteLine("  ");
            CharacteristicWrite(va);
        }



        public void wirteUserName(int cmd, byte[] oneFonts, byte[] twoFonts, byte[] threeFonts, byte[] fourFonts)
        {
            byte[] fontsData = new byte[32 * 4];
            Array.Copy(oneFonts, 0, fontsData, 0, oneFonts.Length);
            Array.Copy(twoFonts, 0, fontsData, 32, twoFonts.Length);
            Array.Copy(threeFonts, 0, fontsData, 64, threeFonts.Length);
            Array.Copy(fourFonts, 0, fontsData, 96, fourFonts.Length);
            writeValue(fontsData);
        }


        int fonts_sid = 0; //字体的第几个包
        byte[] fonts_data = new byte[32 * 4]; //要发送的字体字节数组
        int sendNumber = 0; //发送次数
        int fonts_param = 0;//第几个字体 
        byte[] fontsData = new byte[16];
        /**发送字体*/

        public void writeValue(byte[] data)
        {
            if (data != null & sendNumber < 8)
            {
                fonts_data = data;
                fonts_param = sendNumber / 2;
                System.Diagnostics.Debug.WriteLine("第几个字体：" + fonts_param);
                System.Diagnostics.Debug.WriteLine( "发送字体第几次：" + fonts_sid);
                System.Diagnostics.Debug.WriteLine("发送次数：" + sendNumber);
                Array.Copy(fonts_data, sendNumber * 16, fontsData, 0, 16);
                System.Diagnostics.Debug.WriteLine("总字节内容" + ( fonts_data).ToString());
                System.Diagnostics.Debug.WriteLine("发送的字体内容" + (fontsData).ToString());
                 write(16, 10, fonts_sid, fonts_param, fontsData);
                sendNumber++;
            }
            else
            {
                sendNumber = 0;
                fonts_sid = 0;
                fonts_param = 0;
            }
        }

        public void writeImage( int cmd,  int number, byte[] imageData)
        {
            if (imageData != null & imageData.Length >= 128 & imageData.Length <= 1024)
            {
                byte[] var10000 = new byte[] { (byte)(number & 255) };
                System.Diagnostics.Debug.WriteLine(" imageData.length = " + imageData.Length);
                byte[] data = new byte[imageData.Length];
                Array.Copy(imageData, 0, data, 0, imageData.Length);
                System.Diagnostics.Debug.WriteLine("先睡眠0.5s再发图片");
                PeripheralwriteImage(cmd, number, data);  
              }
        }

        public void PeripheralwriteImage(int cmd, int number, byte[] data)
        {
            if (image_data == null)
            {
                image_data = new byte[data.Length];
            }

            image_data = data;
            int index = sendNumber / 8;
            int index_sid = sendNumber % 8;
            mNumber = number;
            if (sendNumber < data.Length / 16)
            {
                int param = (number & 255) << 4 | index & 255;
                System.Diagnostics.Debug.WriteLine("发送次数：" + this.sendNumber + "，发送那组图片：" + number + " , 发送那张图片：" + index );
                System.Diagnostics.Debug.WriteLine("当前这张图片发送的次数：" + index_sid);
                Array.Copy(image_data, sendNumber * 16, image_data_send, 0, 16);
                write(16, cmd, index_sid, param, image_data_send);
                ++sendNumber;
            }
            else
            {
                //this.callbackContext.onSendImageAndFontsResult(cmd, 1, number);
                System.Diagnostics.Debug.WriteLine("Peripheral", "发送结束的图片");
                sendNumber = 0;
                image_data = null;
            }

        }
        private void write(int length, int cmd, int sid, int param, byte[] data)
        {
            byte v = 1;
            byte t = 0;
            byte[] value = new byte[20];
            value[0] = (byte)((v << 5) | ((length - 1) << 1) | t);
            value[1] = (byte)(cmd & 255);
            value[2] = (byte)sid;
            value[3] = (byte)param;
            Array.Copy(data, 0, value, 4, data.Length);

            System.Diagnostics.Debug.WriteLine("写入数据：  ");
            for (int i = 0; i < value.Length; i++)
            {
                System.Diagnostics.Debug.Write(" " + value[i] + " ");
            }
            System.Diagnostics.Debug.WriteLine("  ");
            CharacteristicWrite(value);

        }

        public void writeImageName(int cmd, int number, String[] name)
        {
            if (name.Length > 0 & name.Length < 9)
            {
                byte[] data = new byte[name.Length * 16];

                for (int i = 0; i < name.Length; ++i)
                {
                    if (name[i].Length <= 8 & name[i].Length > 0)
                    {
                        //这句代码可能有问题bug   编码问题 的
                        //Array.Copy(name[i].ToCharArray(), 0, data, i * 16, name[i].ToCharArray().Length);
                       Array.Copy(System.Text.UnicodeEncoding.UTF8.GetBytes(name[i]), 0, data, i * 16, System.Text.UnicodeEncoding.UTF8.GetBytes(name[i]).Length);
                    }
                }

                writeImageNamebyte(cmd, number, data);
            }

        }
        private void writeImageNamebyte(int cmd, int number, byte[] nameData)
        {
            if (nameData != null & nameData.Length >= 16 & nameData.Length <= 128)
            {
                byte[] data = new byte[nameData.Length];
                Array.Copy(nameData, 0, data, 0, nameData.Length);
                PeripheralwriteImageName(cmd, number, data);
            }

        }

        public void PeripheralwriteImageName(int cmd, int number, byte[] data)
        {
            if (image_name_data == null)
            {
                image_name_data = new byte[data.Length];
            }

            mNumber = number;
            image_name_data = data;
            int index_sid = sendNumber % 8;
            if (sendNumber < data.Length / 16)
            {
                int param = (number & 255) << 4 | index_sid & 255;
                System.Diagnostics.Debug.WriteLine("发送次数：" + sendNumber + "，发送那组图片文字：" + number);
                System.Diagnostics.Debug.WriteLine("发送那张图片名字：" + index_sid);
                Array.Copy(image_name_data, sendNumber * 16, image_data_send, 0, 16);
                write(16, cmd, 0, param, image_data_send);
                ++sendNumber;
            }
            else
            {
              //this.callbackContext.onSendImageAndFontsResult(cmd, 1, number);
                System.Diagnostics.Debug.WriteLine(" 发送结束的图片文字 ");
                sendNumber = 0;
                image_name_data = null;
            }

        }

        public async void connect(String deviceid, CoreDispatcher dispatcher)
        {
            mdispatcher = dispatcher;
            try
            {
                bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(deviceid);
            }
            catch (Exception ex) when ((uint)ex.HResult == 0x800710df)
            {

                // ERROR_DEVICE_NOT_AVAILABLE because the Bluetooth radio is not on.
                System.Diagnostics.Debug.WriteLine("请在  “设置”--“设备” 中打开蓝牙！ ");
                Message = "请检查蓝牙是否开启后重新连接.";
                //              sc.setUsername("请在  “设置”--“设备” 中打开蓝牙！");
            }
            if (bluetoothLeDevice != null)
            {
                Message = "连接成功";
                bluetoothLeDevice.ConnectionStatusChanged += bluetoothLeDevice_ConnectionStatusChanged;
            }
            else
            {
                ClearBluetoothLEDevice();
                System.Diagnostics.Debug.WriteLine("请检查蓝牙是否开启后重新连接. ");
                System.Diagnostics.Debug.WriteLine("bluetoothLeDevice == null ");
                Message ="请检查蓝牙是否开启后重新连接.";
            }
            gattdeviceService = bluetoothLeDevice.GetGattService(guidService);
            mgattCharacteristic = gattdeviceService.GetCharacteristics(guidCharacteristic);
            await mdispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                mListener.Connected(Message);
            });
        }

        public async void CharacteristicNotify()
        {
            // BT_Code: Must write the CCCD in order for server to send notifications.
            // We receive them in the ValueChanged event handler.
            // Note that this sample configures either Indicate or Notify, but not both.
            try
            {
                var result = await
                       mgattCharacteristic[0].WriteClientCharacteristicConfigurationDescriptorAsync(
                           GattClientCharacteristicConfigurationDescriptorValue.Notify);
                if (result == GattCommunicationStatus.Success)
                {
                    AddValueChangedHandler();
                    System.Diagnostics.Debug.WriteLine("Successfully registered for notifications ");
                    Message = "Successfully registered for notifications ";
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Error registering for notifications: "+result);
                    Message = "Error registering for notifications: " + result;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // This usually happens when a device reports that it support notify, but it actually doesn't.
                System.Diagnostics.Debug.WriteLine("   error： "+ ex.Message);
                Message = "   error： " + ex.Message;
            }
            await mdispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                mListener.onSendResult(Message);
            });

        }

        public async void CharacteristicWrite(byte[] array)
        {
            buffer = WindowsRuntimeBufferExtensions.AsBuffer(array, 0, array.Length);
            try
            {
                var result = await mgattCharacteristic[0].WriteValueAsync(buffer);
                if (result == GattCommunicationStatus.Success)
                {
                    System.Diagnostics.Debug.WriteLine("Successfully wrote value to device");
                    //Message = "Successfully wrote value to device";
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Write failed:  " + result);
                    Message = "Write failed:  " + result;
                }
            }
            catch (Exception ex) when ((uint)ex.HResult == 0x80650003 || (uint)ex.HResult == 0x80070005)
            {
                // E_BLUETOOTH_ATT_WRITE_NOT_PERMITTED or E_ACCESSDENIED
                // This usually happens when a device reports that it support writing, but it actually doesn't.
                System.Diagnostics.Debug.WriteLine("   error： " + ex.Message);
                Message = "   error： " + ex.Message;
            }
            //await mdispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    mListener.onSendResult(Message);
            //});

        }
        public void ClearBluetoothLEDevice()
        {
            bluetoothLeDevice?.Dispose();
            bluetoothLeDevice = null;
        }
        public  void ClearSelectedCharacteristic()
        {
            if (mgattCharacteristic != null)
            {
                if (isValueChangedHandlerRegistered)
                {
                    mgattCharacteristic[0].ValueChanged -= Characteristic_ValueChanged;
                    isValueChangedHandlerRegistered = false;
                }
                mgattCharacteristic = null;
            }
        }
        public void AddValueChangedHandler()
        {
            if (!isValueChangedHandlerRegistered)
            {
                mgattCharacteristic[0].ValueChanged += Characteristic_ValueChanged;
                isValueChangedHandlerRegistered = true;
            }
        }
        //连接状态

        public async void bluetoothLeDevice_ConnectionStatusChanged(BluetoothLEDevice bluetoothLeDevice, System.Object onConnectState)
        {
            await mdispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var statusArgs = bluetoothLeDevice.ConnectionStatus;
                String aa = Convert.ToString(statusArgs);
                System.Diagnostics.Debug.WriteLine(" statusArgs: " + statusArgs);
                System.Diagnostics.Debug.WriteLine(" aa: " + aa);
                if (aa.Equals("Connected"))
                {
                    Message = "连接状态：连接成功！";
                    mListener.Connected(Message);
                }
                else if (aa.Equals("Disconnected")) {
                    Message = "连接状态：断开连接！";
                    mListener.Connected(Message);
                }

            });
        }
        private async void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            // BT_Code: An Indicate or Notify reported that the value has changed.
            // Display the new value with a timestamp.
            byte[] test = Buffer2Bytes(args.CharacteristicValue);
            var newValue = FormatValueByPresentation(args.CharacteristicValue, presentationFormat);
            int result = test[4];
            String me;
            if (!showMessage(result).Equals("")) {
                if (Convert.ToString(test[1], 16).Length.Equals(1))
                {
                    me = " 写入设备：0x0" + Convert.ToString(test[1], 16);
                }
                else
                {
                    me = " 写入设备：0x" + Convert.ToString(test[1], 16);
                }
                Message = "设备返回：" + me + "，处理结果:  " + test[4] + " ,  " + showMessage(result);
                await mdispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    mListener.onSendResult(Message);
                });
            }
            System.Diagnostics.Debug.WriteLine("命令返回数据 ");
            for (int i = 0; i < test.Length; i++)
            {
                System.Diagnostics.Debug.Write(" " + test[i]);
            }
            System.Diagnostics.Debug.WriteLine(" ");
        }
        String showResult;
        public String showMessage(int str) {
            if (str > 17) {
                return "";
            }
            showResult = RESPONE_STATE[str];
            return showResult;
        }
        public static byte[] Buffer2Bytes(IBuffer buffer)
        {
            using (var dataReader = DataReader.FromBuffer(buffer))
            {
                var bytes = new byte[buffer.Length];
                dataReader.ReadBytes(bytes);
                return bytes;
            }
        }
        private string FormatValueByPresentation(IBuffer buffer, GattPresentationFormat format)
        {
            // BT_Code: For the purpose of this sample, this function converts only UInt32 and
            // UTF-8 buffers to readable text. It can be extended to support other formats if your app needs them.
            byte[] data;
            CryptographicBuffer.CopyToByteArray(buffer, out data);
            if (format != null)
            {
                if (format.FormatType == GattPresentationFormatTypes.UInt32 && data.Length >= 4)
                {
                    return BitConverter.ToInt32(data, 0).ToString();
                }
                else if (format.FormatType == GattPresentationFormatTypes.Utf8)
                {
                    try
                    {
                        return Encoding.UTF8.GetString(data);
                    }
                    catch (ArgumentException)
                    {
                        return "(error: Invalid UTF-8 string)";
                    }
                }
                else
                {
                    // Add support for other format types as needed.
                    return "Unsupported format: " + CryptographicBuffer.EncodeToHexString(buffer);
                }
            }
            else
            {
                // We don't know what format to use. Let's try a well-known profile, or default back to UTF-8.
                if (mgattCharacteristic[0].Uuid.Equals(GattCharacteristicUuids.HeartRateMeasurement))
                {
                    try
                    {
                        return "Heart Rate: " + ParseHeartRateValue(data).ToString();
                    }
                    catch (ArgumentException)
                    {
                        return "Heart Rate: (unable to parse)";
                    }
                }
                else if (mgattCharacteristic[0].Uuid.Equals(GattCharacteristicUuids.BatteryLevel))
                {
                    try
                    {
                        // battery level is encoded as a percentage value in the first byte according to
                        // https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.characteristic.battery_level.xml
                        return "Battery Level: " + data[0].ToString() + "%";
                    }
                    catch (ArgumentException)
                    {
                        return "Battery Level: (unable to parse)";
                    }
                }
                else
                {
                    try
                    {
                        return "Unknown format: " + Encoding.UTF8.GetString(data);
                    }
                    catch (ArgumentException)
                    {
                        return "Unknown format";
                    }
                }
            }
        }

        private static ushort ParseHeartRateValue(byte[] data)
        {
            // Heart Rate profile defined flag values
            const byte heartRateValueFormat = 0x01;

            byte flags = data[0];
            bool isHeartRateValueSizeLong = ((flags & heartRateValueFormat) != 0);

            if (isHeartRateValueSizeLong)
            {
                return BitConverter.ToUInt16(data, 1);
            }
            else
            {
                return data[1];
            }
        }


    }
}

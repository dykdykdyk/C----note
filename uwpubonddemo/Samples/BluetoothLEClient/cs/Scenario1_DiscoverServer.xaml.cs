using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SDKTemplate
{
    // This scenario uses a DeviceWatcher to enumerate nearby Bluetooth Low Energy devices,
    // displays them in a ListView, and lets the user select a device and pair it.
    // This device will be used by future scenarios.
    // For more information about device discovery and pairing, including examples of
    // customizing the pairing process, see the DeviceEnumerationAndPairing sample.
    public sealed partial class Scenario1_DiscoverServer : Page,ScanListener, BLEDeviceListener
    {
        private MainPage rootPage = MainPage.Current;

        public ObservableCollection<BluetoothLEDeviceDisplay> ResultCollection = new ObservableCollection<BluetoothLEDeviceDisplay>();
        public bool scan =true;
        BLEManager ble;
        BLEDevice bled;
       
        public Scenario1_DiscoverServer()
        {
            InitializeComponent();
            ble = new BLEManager(this);
            bled = new BLEDevice(this);
            String aa = "sdcsdfge";
            //byte[] byteArray = System.Text.Encoding.Unicode.GetBytes(aa);
            byte[] byteArray =System.Text.UnicodeEncoding.Unicode.GetBytes(aa);
            System.Diagnostics.Debug.WriteLine(" aa33  " + aa.ToCharArray());
            System.Diagnostics.Debug.WriteLine(" byteArray  " + byteArray);
        }
        //用户即将离开这个界面的时候需要调用的方法
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ble.StopBleDeviceWatcher();

            // Save the selected device's ID for use in other scenarios.
            //as 强制转换类型
            var bleDeviceDisplay = ResultsListView.SelectedItem as BluetoothLEDeviceDisplay;
            if (bleDeviceDisplay != null)
            {
                // rootPage.SelectedBleDeviceId 界面之间的通用值
                rootPage.SelectedBleDeviceId = bleDeviceDisplay.Id;
                rootPage.SelectedBleDeviceName = bleDeviceDisplay.Name;
            }
        }

        #region Device discovery

        //开始扫描方法
        private void EnumerateButton_Click()
        {

            if (scan)
            {

                EnumerateButton.Content = "Stop enumerating";
                ResultCollection.Clear();
                //开始扫描
                ble.startscanner(Dispatcher);
                scan = false;
                rootPage.NotifyUser($"Device watcher started.", NotifyType.StatusMessage);
            }
            else
            {
                scan = true;
                ble.StopBleDeviceWatcher();
                EnumerateButton.Content = "Start enumerating";
                rootPage.NotifyUser($"Device watcher stopped.", NotifyType.StatusMessage);
            }
        }

        public void onScanResult(string var1, DeviceInformation deviceInfo)
        {
            
            try
            {
                System.Diagnostics.Debug.WriteLine(" onScanResult  " + var1);
                rootPage.NotifyUser($""+ var1, NotifyType.StatusMessage);
                ResultCollection.Add(new BluetoothLEDeviceDisplay(deviceInfo));
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    
        #endregion

        #region Pairing

        private bool isBusy = false;

        private void PairButton_Click()
        {
            // Do not allow a new Pair operation to start if an existing one is in progress.
            if (isBusy)
            {
                return;
            }
            isBusy = true;
            var bleDeviceDisplay = ResultsListView.SelectedItem as BluetoothLEDeviceDisplay;
            bled.pair(bleDeviceDisplay, Dispatcher);
            isBusy = false;
        }

        public void Connected(string address)
        {
            try
            {
               
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public void onSendResult(String address)
        {
            try
            {
                
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
                if (result.Equals("pairsucess"))
                {
                    rootPage.NotifyUser($"配对成功", NotifyType.StatusMessage);
                }
                else {
                    rootPage.NotifyUser($"配对失败", NotifyType.StatusMessage);
                }
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace SDKTemplate
{
    interface BLEDeviceListener
    {
        /**
        * 设备连接函数
        * @param address
        */
        void Connected(String address);
        /**
         * 设备数据返回函数
         * @param address
         */
         void onSendResult(String address);
        /**
        * 设备配对函数
        * @param address
        */
        void onpaired(String result);
    }
}

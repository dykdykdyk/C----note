using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace SDKTemplate
{
   public  interface ScanListener
    {
        void onScanResult(String var1, DeviceInformation deviceInfo);
    }
}

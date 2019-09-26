using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SH.Communication;
using Windows.Devices.WiFi;

namespace SH.UWPToolKit
{
    public class AP : IAP
    {
        public AP(WiFiAvailableNetwork wiFiAvailableNetwork)
        {
            WiFiAvailableNetwork = wiFiAvailableNetwork;
        }

        public WiFiAvailableNetwork WiFiAvailableNetwork { get; }

        public string SSID => WiFiAvailableNetwork.Ssid;

        public string Password { get; private set; }

        public void SetPasswordToConnected(string password)
        {
            Password = password;
        }
    }
}

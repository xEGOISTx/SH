using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SH.Communication;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Security.Credentials;

namespace SH.UWPToolKit
{
    public class Connector : IConnector
    {
        private readonly List<AP> _availableAPs = new List<AP>();
        private WiFiAdapter _wifiAdapter;

        public int CountFoundAP => _availableAPs.Count;

        public IEnumerable<IAP> APs => _availableAPs;

        public void Clear()
        {
            _availableAPs.Clear();
        }

        public async Task ConnectTo(IAP aP)
        {
            await Task.Run(async () =>
            {
                WiFiAvailableNetwork availableNetwork = (aP as AP).WiFiAvailableNetwork;
                PasswordCredential credential = new PasswordCredential { Password = (aP as AP).Password };

                WiFiConnectionResult conResult;

                do
                {
                    conResult = await _wifiAdapter.ConnectAsync(availableNetwork, WiFiReconnectionKind.Manual, credential);
                }
                while (conResult.ConnectionStatus != WiFiConnectionStatus.Success);
            });
        }

        public void Disconnect()
        {
            if (_wifiAdapter != null)
            {
                _wifiAdapter.Disconnect();
            }
        }

        public async Task FindAPs(IAPSSIDs ssids)
        {
            bool initRes = await InitializeFirstAdapter();

            if (initRes)
            {
                await _wifiAdapter.ScanAsync();

                await Task.Run(() =>
                {
                    foreach(WiFiAvailableNetwork wiFiAvailableNetwork in _wifiAdapter.NetworkReport.AvailableNetworks)
                    {
                        if(ssids.Contains(wiFiAvailableNetwork.Ssid))
                        {
                            _availableAPs.Add(new AP(wiFiAvailableNetwork));
                        }
                    }
                });
            }
        }

        public IPAddress GetHostIP()
        {
            IPAddress ip = null;

            foreach (HostName localHostName in NetworkInformation.GetHostNames())
            {
                if (localHostName.IPInformation != null)
                {
                    if (localHostName.Type == HostNameType.Ipv4)
                    {
                        ip = IPAddress.Parse(localHostName.ToString());
                        break;
                    }
                }
            }

            return ip;
        }

        /// <summary>
        /// Инициализация адаптера
        /// </summary>
        /// <returns></returns>
        private async Task<bool> InitializeFirstAdapter()
        {
            return await Task.Run(async () =>
            {
                if (_wifiAdapter == null)
                {
                    bool res = false;
                    WiFiAccessStatus access = await WiFiAdapter.RequestAccessAsync();

                    if (access != WiFiAccessStatus.Allowed)
                    {
                        throw new Exception("WiFiAccessStatus not allowed");
                    }
                    else
                    {
                        var wifiAdapterResults = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());

                        if (wifiAdapterResults.Count >= 1)
                        {
                            _wifiAdapter = await WiFiAdapter.FromIdAsync(wifiAdapterResults[0].Id);
                            res = true;
                        }
                        else
                        {
                            throw new Exception("WiFi Adapter not found.");
                        }
                    }
                    return res;
                }
                else
                {
                    return true;
                }
            });
        }
    }
}

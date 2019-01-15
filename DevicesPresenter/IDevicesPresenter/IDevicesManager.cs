using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFi;

namespace DevicesPresenter
{
	public interface IDevicesManager
	{

		//IReadOnlyDictionary<ushort,IDevice> Devices { get; }

		IDeviceCommonList Devices { get; }

		IConnectionSettings ConnectionSettings { get; }

		IDeviceEditor GetDeviceEditor();

		Task<bool> FindAndConnectDevicesAsync();

		void SynchronizationWithDevicesAsync();

		event EventHandler LoadDevicesComplete;


		//Task<IEnumerable<WiFiAvailableNetwork>> GetAvailableDevices(string ssid);

		//Task<bool> ConnectAndSendConnectionSettingAsync(IEnumerable<WiFiAvailableNetwork> availableNetworks, 
		//	IConnectionParams deviceConnectionParams, IConnectionParams routerConnectionParams);

		//Task<bool> SendIdTo(IPAddress iPAddress, ushort id);

		//Task<ushort> GetId(IPAddress iPAddress);
		}
}

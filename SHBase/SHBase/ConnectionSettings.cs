using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase
{
	public class ConnectionSettings : IConnectionSettings
	{
		private ConnectionParams _connectionToDevice;
		private ConnectionParams _connectionToRouter;

		public IConnectionParams DeviceConnParams => _connectionToDevice;

		public IConnectionParams RouterConnParams => _connectionToRouter;

		public bool ChangeParamsForDevice(string ssid, string password)
		{
			_connectionToDevice.Ssid = ssid;
			_connectionToDevice.Password = password;

			return true;
		}

		public bool ChangeParamsForRouter(string ssid, string password)
		{
			_connectionToRouter.Ssid = ssid;
			_connectionToRouter.Password = password;

			return true;
		}

		public void Save()
		{
			throw new NotImplementedException();
		}

		public void Load()
		{
			//ILoader loader = new Loader();
			_connectionToDevice = new ConnectionParams() { Ssid = "Test", Password = "1234567890" };//loader.LoadDeviceConnectionParams() as ConnectionParams;
			_connectionToRouter = new ConnectionParams() { Ssid = "MGTS_GPON_2303", Password = "SSFR73N6" };//loader.LoadRouterConnectionParams() as ConnectionParams;

		}
	}
}

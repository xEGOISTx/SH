using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouterParser;
using SHBase.DevicesBaseComponents;
using Switches.SwitchesOutlets;

namespace DevicesPresenter
{
	public class DevicesManager : IDevicesManager
	{
		private readonly DeviceCommonList _deviceCommonList;
		private bool _scanDevicesFromRouterIsComplite;

		public DevicesManager(DeviceCommonList deviceCommonList)
		{
			_deviceCommonList = deviceCommonList;
		}


		public IConnectionSettings ConnectionSettings => throw new NotImplementedException();

		public Task<bool> FindAndConnectDevicesAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<bool> SynchronizationWithDevicesAsync()
		{
			throw new NotImplementedException();

			//if (_scanDevicesFromRouterIsComplite)
			//{
			//	_scanDevicesFromRouterIsComplite = false;
			//	Parser parser = new Parser("http://192.168.1.254/", "admin", "admin");

			//	parser.LoadDeviceInfosComplete += Parser_LoadDeviceInfosComplete;
			//	parser.LoadDeviceInfosAsync();
			//}



			//foreach (Devices devs in _deviceCommonList)
			//{
			//	await devs.Synchronization();
			//}
		}


		public ISwitches GetSwitches()
		{
			return _deviceCommonList.GetDeviceList<Switches.SwitchesOutlets.Switches>();
		}

		public async Task<bool> LoadDevicesAsync()
		{
			foreach(Devices devs in _deviceCommonList)
			{
				await devs.Load();
			}

			return true;
		}

	}
}

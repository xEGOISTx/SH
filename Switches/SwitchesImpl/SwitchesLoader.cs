using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;
using DataManager;

namespace Switches
{
	public class SwitchesLoader : ISwitchesLoader
	{
		private readonly DataManager.DataManager _dataManager = new DataManager.DataManager();
		private readonly DeviceType _deviceType;

		public SwitchesLoader(DeviceType deviceType)
		{
			_deviceType = deviceType;
		}

		public async Task<IResultOperationLoad> LoadDevices()
		{
			return await Task.Run(() =>
			{
				return _dataManager.LoadDevices((int)_deviceType);
			});
		}

		public async Task<IDBOperationResult> RenameDevice(IDeviceInfo device)
		{
			return await Task.Run(() =>
			{
				return _dataManager.RenameDevice(device);
			});
		}

		public async Task<IResultOperationSave> SaveDevices(IDeviceInfo[] devices)
		{
			return await Task.Run(() =>
			{
				return _dataManager.SaveDevices(devices);
			});
		}
	}
}

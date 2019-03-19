using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
	public class DevicesLoader
	{
		private readonly DataManager _dataManager = new DataManager();
		private readonly int _deviceType;

		public DevicesLoader(int deviceType)
		{
			_deviceType = deviceType;
		}

		public async Task<IResultOperationLoad> LoadDevices()
		{
			return await Task.Run(() =>
			{
				return _dataManager.LoadDevices(_deviceType);
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

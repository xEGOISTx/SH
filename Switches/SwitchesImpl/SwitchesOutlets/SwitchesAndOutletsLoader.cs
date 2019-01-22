using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;
using DataManager;

namespace Switches.SwitchesOutlets
{
	public class SwitchesAndOutletsLoader : ISwitchesAndOutletsLoader
	{
		private readonly DataManager.DataManager _dataManager = new DataManager.DataManager();

		public async Task<IResultOperationLoad> LoadDevices()
		{
			return await Task.Run(() =>
			{
				return _dataManager.Switches.LoadDevices();
			});
		}

		public async Task<IDBOperationResult> RenameDevice(IDeviceInfo device)
		{
			return await Task.Run(() =>
			{
				return _dataManager.Switches.RenameDevice(device);
			});
		}

		public async Task<IResultOperationSave> SaveDevices(IDeviceInfo[] devices)
		{
			return await Task.Run(() =>
			{
				return _dataManager.Switches.SaveDevices(devices);
			});
		}
	}
}

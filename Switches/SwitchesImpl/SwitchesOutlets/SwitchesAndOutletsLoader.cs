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
		public async Task<bool> Load()
		{
			throw new NotImplementedException();
		}

		public async Task<bool> RenameDevice(IDeviceBase device)
		{
			throw new NotImplementedException();
		}

		public async Task<int[]> SaveDevices(IEnumerable<ISwitchOutlet> devices)
		{
			//TODO: сделать интерфейсы для DataManag и передавать резултат операции дальше 
			return await Task.Run(() =>
			{
				//сохраняем
				DataManager.DataManager dataManager = new DataManager.DataManager();
				DBDeviceInfo[] deviceInfos = MakeInfos(devices);
				ResultOperationSave result = dataManager.SaveSwitches(deviceInfos);

				return result.IDs.ToArray();
			});
		}

		private DBDeviceInfo[] MakeInfos(IEnumerable<ISwitchOutlet> devices)
		{
			List<DBDeviceInfo> deviceInfos = new List<DBDeviceInfo>();

			foreach(ISwitchOutlet device in devices)
			{
				DBDeviceInfo deviceInfo = new DBDeviceInfo
				{
					Description = device.Description,
					DeviceType = (int)device.DeviceType,
					FirmwareType = (int)device.FirmwareType,
					MacAddress = device.Mac.ToString()
				};

				deviceInfos.Add(deviceInfo);
			}

			return deviceInfos.ToArray();
		}
	}
}

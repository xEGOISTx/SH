using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataManager;
using SHBase;

namespace DevicesPresenter
{
	public class Loader : ILoader
	{
		public IConnectionParams LoadDeviceConnectionParams()
		{
			return new ConnectionParams() {Ssid = "Test", Password = "1234567890" };
		}

		public IConnectionParams LoadRouterConnectionParams()
		{
			return new ConnectionParams() {Ssid ="MGTS_GPON_2214", Password ="4AMYNYKM" };
		}

		public async Task<bool> SaveDeviceAsync(IDevice device)
		{
			return await Task.Run(() =>
			{
				DBDeviceInfo dbDeviceInfo = new DBDeviceInfo
				{
					Id = device.ID,
					Description = device.Description,
					FirmwareType = (int)device.FirmwareType,					
				};

				DataManager.DataManager dB = new DataManager.DataManager();
				dB.SaveDevice(dbDeviceInfo);

				return true;
			});

		}

		public List<int> GetDevicesIDs()
		{
			DataManager.DataManager dB = new DataManager.DataManager();
			return dB.GetDevicesIDs();
		}

		/// <summary>
		/// Not implemented
		/// </summary>
		/// <param name="device"></param>
		/// <param name="description"></param>
		/// <returns></returns>
		public bool UpdateDeviceDescriptionInDB(IDevice device,string description)
		{
			return true;
		}
	}
}

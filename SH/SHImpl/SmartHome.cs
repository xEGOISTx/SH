using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHToolKit.DevicesManagement;
using SHBase.DevicesBaseComponents;

namespace SH
{
	public class SmartHome : ISmartHome
	{
		public SmartHome(IEnumerable<DeviceBaseList> devices)
		{
			foreach(DeviceBaseList deviceBaseList in devices)
			{
				(DevicesManager as DevicesManager).AddForManagement(deviceBaseList);
			}
		}

		public IDevicesManager DevicesManager { get; } = new DevicesManager();

		public async Task<bool> Start()
		{
			await (DevicesManager as DevicesManager).LoadDevicesAsync();

			return true;
		}
	}
}

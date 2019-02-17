using DataManager;
using SHBase;
using SHBase.DevicesBaseComponents;
using System.Collections.Generic;
using System.Linq;

namespace Switches
{
	internal class OutletsConvertor : DBConvertor
	{
		public override IEnumerable<IBaseSwitch> ConvertToDevices(IEnumerable<IDeviceInfo> infos)
		{
			List<IOutlet> baseSwitches = new List<IOutlet>(infos.Count());

			foreach (IDeviceInfo deviceInfo in infos)
			{
				var mac = new MacAddress(deviceInfo.MacAddress);
				var fType = (FirmwareType)deviceInfo.FirmwareType;
				var dType = (DeviceType)deviceInfo.DeviceType;

				Outlet ou = new Outlet(mac, fType, dType)
				{
					ID = deviceInfo.ID,
					Description = deviceInfo.Description
				};

				baseSwitches.Add(ou);
			}

			return baseSwitches;
		}
	}
}

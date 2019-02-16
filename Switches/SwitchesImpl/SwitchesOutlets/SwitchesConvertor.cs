using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataManager;
using SHBase;
using SHBase.DevicesBaseComponents;

namespace Switches
{
	internal class SwitchesConvertor : DBConvertor<ISwitch>
	{
		public override IEnumerable<ISwitch> ConvertToDevices(IEnumerable<IDeviceInfo> infos)
		{
			List<ISwitch> baseSwitches = new List<ISwitch>(infos.Count());

			foreach(IDeviceInfo deviceInfo in infos)
			{
				var mac = new MacAddress(deviceInfo.MacAddress);
				var fType = (FirmwareType)deviceInfo.FirmwareType;
				var dType = (DeviceType)deviceInfo.DeviceType;

				Switch sw = new Switch(mac, fType, dType)
				{
					Description = deviceInfo.Description
				};

				baseSwitches.Add(sw);
			}

			return baseSwitches;
		}
	}
}

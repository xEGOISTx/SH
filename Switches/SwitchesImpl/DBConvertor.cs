using DataManager;
using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switches
{
	internal abstract class DBConvertor<SwitchType>
	{
		public abstract IEnumerable<SwitchType> ConvertToDevices(IEnumerable<IDeviceInfo> infos);

		public IDeviceInfo[] ConvertToDeviceInfos(IEnumerable<IDeviceBase> switches)
		{
			List<DeviceInfo> deviceInfos = new List<DeviceInfo>();

			foreach (IDeviceBase device in switches)
			{
				DeviceInfo deviceInfo = new DeviceInfo
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

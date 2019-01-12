using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterParser
{
	public delegate void LoadDeviceInfosEventHandler(object sender, DeviceInfosEventArgs e);

	public class DeviceInfosEventArgs : EventArgs
	{
		public DeviceInfosEventArgs(IEnumerable<RDeviceInfo> deviceInfos)
		{
			DeviceInfos = deviceInfos;
		}

		public IEnumerable<RDeviceInfo> DeviceInfos { get; }
	}
}

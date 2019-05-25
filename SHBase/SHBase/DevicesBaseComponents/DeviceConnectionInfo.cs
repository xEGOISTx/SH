using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public class DeviceConnectionInfo
	{
		public DeviceConnectionInfo(IDeviceBase device, bool conState)
		{
			Device = device;
			ConnectionState = conState;
		}

		public IDeviceBase Device { get; }

		public bool ConnectionState { get; }
	}
}

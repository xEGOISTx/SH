using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase;
using SHBase.DevicesBaseComponents;

namespace SHToolKit.DevicesManagement
{
	internal class DeviceConnectionInfo : IDeviceConnectionInfo
	{
		public DeviceConnectionInfo(IDeviceBase device, bool conState)
		{
			Device = device;
			ConnectionState = conState;
		}

		public IDeviceBase Device { get;}

		public bool ConnectionState { get;}
	}
}

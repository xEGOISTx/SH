using SHBase;
using SHBase.DevicesBaseComponents;

namespace SH.DevicesManagement
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

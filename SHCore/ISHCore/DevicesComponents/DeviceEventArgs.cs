namespace SH.Core.DevicesComponents
{
	public delegate void DeviceEventHandler(object sender, DeviceEventArgs e);

	public class DeviceEventArgs
	{
		public DeviceEventArgs(IDevice device)
		{
			Device = device;
		}

		public IDevice Device { get; }

		public bool Cancel { get; }
	}
}

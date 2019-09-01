using System;
using System.Collections.Generic;
using System.Text;

namespace SH.Communication
{
	public delegate void DeviceRequestEventHandler(object sender, DeviceRequestEventArgs e);
	public class DeviceRequestEventArgs
	{
		public DeviceRequestEventArgs(string request)
		{
			Request = request;
		}

		public string Request { get; }
	}
}

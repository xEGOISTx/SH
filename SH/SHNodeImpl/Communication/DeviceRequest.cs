using SH.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SH.Communication
{
	internal class DeviceRequest : IDeviceRequest
	{
		public int RequestType { get; set; }

		public IPAddress DeviceIP { get; set; }

		public int DeviceType { get; set; }
	}
}

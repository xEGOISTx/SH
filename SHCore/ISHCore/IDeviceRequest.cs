using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SH.Core
{
	public interface IDeviceRequest
	{
		int DeviceID { get; }

		MacAddress Mac { get; }

		int RequestType { get; }

		IPAddress DeviceIP { get; }

		int DeviceType { get; }
	}
}

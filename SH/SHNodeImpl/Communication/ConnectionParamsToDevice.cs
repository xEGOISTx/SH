using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SH.Communication
{
	internal class ConnectionParamsToDevice
	{
		public IAPSSIDs APSSIDsForSearch { get; set; }

		public IPAddress DeviceDafaultIP { get; set; }

		public string DeviceAPPassword { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit.DataManagement
{
	public interface IConnectionSettings
	{
		IPAddress RouterIP { get; }

		string RouterLogin { get; }

		string RouterPassword { get; }

		string RouterSsid { get; }

		string RouterWiFiPassword { get; }

		IPAddress DeviceDafaultIP { get; }

		string DeviceWiFiPassword { get; }
	}
}

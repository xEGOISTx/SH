using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SHBase;

namespace SH
{
	internal class ConnectionSettings : SHToolKit.DataManagement.IConnectionSettings
	{
		internal ConnectionSettings() { }

		internal ConnectionSettings(SHToolKit.DataManagement.IConnectionSettings settings)
		{
			RouterIP = settings.RouterIP;
			RouterLogin = settings.RouterLogin;
			RouterPassword = settings.RouterPassword;
			RouterSsid = settings.RouterSsid;
			RouterWiFiPassword = settings.RouterWiFiPassword;
			DeviceDafaultIP = settings.DeviceDafaultIP;
			DeviceWiFiPassword = settings.DeviceWiFiPassword;
		}

		public IPAddress RouterIP { get; set; }

		public string RouterLogin { get; set; }

		public string RouterPassword { get; set; }

		public string RouterSsid { get; set; }

		public string RouterWiFiPassword { get; set; }

		public IPAddress DeviceDafaultIP { get; set; }

		public string DeviceWiFiPassword { get; set; }

		public SHBase.ICredentials GetRouterCredentials()
		{
			return new RouterCredentals(RouterIP)
			{
				Login = RouterLogin,
				Password = RouterPassword
			};
		}

		public IConnectionParamsToAP GetConnectionParamsToRouterAsAP()
		{
			return new ConnectionParamsToRouterAsAP
			{
				Ssid = RouterSsid,
				Password = RouterWiFiPassword
			};
		}
	}
}

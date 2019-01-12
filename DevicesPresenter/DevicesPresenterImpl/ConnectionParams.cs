using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase;

namespace DevicesPresenter
{
	public class ConnectionParams : IConnectionParams
	{

		public ConnectionParams()
		{
			Ssid = string.Empty;
			Password = string.Empty;
		}

		public ConnectionParams(string ssid,string password)
		{
			Ssid = ssid;
			Password = password ?? string.Empty;
		}

		public string Ssid { get; set; }

		public string Password { get; set; }

	}
}

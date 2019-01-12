using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RouterParser
{
	public class RDeviceInfo
	{
		internal RDeviceInfo(string name,string ip,string mac,bool isConnected)
		{
			Name = name;
			Ip = IPAddress.Parse(ip);
			Mac = mac;
			IsConnected = isConnected;
		}

		public string Name { get; }

		public IPAddress Ip { get; }

		public string Mac { get; }

		public bool IsConnected { get; }
	}
}

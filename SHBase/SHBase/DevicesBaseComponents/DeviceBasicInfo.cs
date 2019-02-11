using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public class DeviceBasicInfo
	{
		internal DeviceBasicInfo(IPAddress iP) { IP = iP; }

		public IPAddress IP { get; internal set; }

		public MacAddress Mac { get; internal set; }

		public int ID { get; internal set; }

		public string Name { get; internal set; }

		public DeviceType DeviceType { get; internal set; }

		public FirmwareType FirmwareType { get; internal set; }
	}
}

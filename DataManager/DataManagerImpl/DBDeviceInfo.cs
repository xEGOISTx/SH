using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
	public class DBDeviceInfo
	{
		public int Id { get; set; }

		public string MacAddress { get; set; }

		public int DeviceType { get; set; }

		public int FirmwareType { get; set; }

		public string Description { get; set; } = string.Empty;
	}
}

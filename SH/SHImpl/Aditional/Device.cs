using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SH
{
	internal class Device : SHToolKit.DataManagement.IDBDevice
	{
		public int ID { get; set; }

		public string MacAddress { get; set; }

		public int DeviceType { get; set; }

		public int FirmwareType { get; set; }

		public string Description { get; set; }
	}
}

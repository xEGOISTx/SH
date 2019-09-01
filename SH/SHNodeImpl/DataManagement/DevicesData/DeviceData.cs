using System;
using System.Collections.Generic;
using System.Text;

namespace SH.DataManagement
{
	public class DeviceData : IDeviceData
	{
		public int ID { get; set; }

		public string MacAddress { get; set; }

		public int DeviceType { get; set; }

		public string Description { get; set; }

        public IDeviceCommandData[] Commands { get; set; }
    }
}

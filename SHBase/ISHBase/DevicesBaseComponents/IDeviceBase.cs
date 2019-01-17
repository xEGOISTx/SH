using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public enum FirmwareType
	{
		Unknown,
		ESP_01,
		ESP_8266
	}

	public interface IDeviceBase
	{
		int ID { get; }

		string Name { get; }

		bool IsConnected { get; }

		IPAddress IP { get; }

		MacAddress Mac { get; }

		FirmwareType FirmwareType { get; }
	}
}

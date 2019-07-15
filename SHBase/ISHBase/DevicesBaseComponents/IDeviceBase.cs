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

		string Description { get; }

		bool IsConnected { get; }

		IPAddress IP { get; }

		MacAddress Mac { get; }

		int DeviceType { get; }

		FirmwareType FirmwareType { get; }
	}
}

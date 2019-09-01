using System.Net;

namespace SH.Core.DevicesComponents
{
	/// <summary>
	/// Класс базового устройсва
	/// </summary>
	public class Device : IDevice
	{
		public Device(int id, int deviceType, MacAddress mac)
		{
			ID = id;
			DeviceType = deviceType;
			Mac = mac;
		}

		public int ID { get; }

		//public string SystemName { get; set; }

		public string Description { get; set; }

		public bool IsConnected { get; set; }

		public IPAddress IP { get; set; }

		public MacAddress Mac { get; }

		public int DeviceType { get; }

		//public int FirmwareType { get; set; }

		public IDeviceCommandList Commands { get; set; }
	}
}

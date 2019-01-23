using SHBase.DevicesBaseComponents;
using System.Net;

namespace SHBase.DevicesBaseComponents
{
	/// <summary>
	/// Базовая инфа устройства
	/// </summary>
	public class DeviceBaseInfo
	{
		public DeviceBaseInfo(ushort id,string name, MacAddress mac,FirmwareType firmwareType)
		{
			ID = id;
			Name = name;
			Mac = mac;
			FirmwareType = firmwareType;
		}

		public ushort ID { get; }

		public string Name { get; }

		public MacAddress Mac { get; }

		public FirmwareType FirmwareType { get; }
	}
}

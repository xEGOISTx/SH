using SHBase.DevicesBaseComponents;
using System;
using System.Net;

namespace SHBase.DevicesBaseComponents
{
	/// <summary>
	/// Базовая инфа устройства
	/// </summary>
	public class DeviceBase : IDeviceBase
	{
		public DeviceBase(IPAddress iP)
		{
			IP = iP;
		}

		public IPAddress IP { get; set; }

		public MacAddress Mac { get; set; }

		public int ID { get; set; }

		public string Name { get; set; }

		public bool IsConnected { get; set; }

		public DeviceType DeviceType { get; set; }

		public FirmwareType FirmwareType { get; set; }

		public string Description { get; set; }

		public event EventHandler ConnectedStatysChange;
	}
}

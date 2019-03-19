/*======================
 * Основа всех устройств
 ======================*/

using System.Net;

namespace SHBase.DevicesBaseComponents
{
	/// <summary>
	/// Базовая инфа устройства
	/// </summary>
	internal class DeviceBase : IDeviceBase
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

		public int DeviceType { get; set; }

		public FirmwareType FirmwareType { get; set; }

		public string Description { get; set; }
	}
}

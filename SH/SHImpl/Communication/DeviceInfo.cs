/*======================
 * Основа всех устройств
 ======================*/

using System.Net;

namespace SH.Communication
{
	/// <summary>
	/// Базовая инфа устройства
	/// </summary>
	internal class DeviceInfo : SHBase.DevicesBaseComponents.IDeviceBase
	{
		public DeviceInfo(IPAddress iP)
		{
			IP = iP;
		}

		public IPAddress IP { get; set; }

		public SHBase.MacAddress Mac { get; set; }

		public int ID { get; set; }

		public string Name { get; set; }

		public bool IsConnected { get; set; }

		public int DeviceType { get; set; }

		public SHBase.DevicesBaseComponents.FirmwareType FirmwareType { get; set; }

		public string Description { get; set; }
	}
}

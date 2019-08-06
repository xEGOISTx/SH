/*======================
 * Основа всех устройств
 ======================*/

using SH.Core;
using System.Net;

namespace SH.Communication
{
	/// <summary>
	/// Базовая инфа устройства
	/// </summary>
	internal class DeviceInfo
	{
		public DeviceInfo(IPAddress iP)
		{
			IP = iP;
		}

		public IPAddress IP { get; set; }

		public MacAddress Mac { get; set; }

		public int ID { get; set; }

		public string Name { get; set; }

		public bool IsConnected { get; set; }

		public int DeviceType { get; set; }

		public int FirmwareType { get; set; }

		//public string Description { get; set; }
	}
}

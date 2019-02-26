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

	/// <summary>
	/// Тип устройства
	/// </summary>
	public enum DeviceType
	{
		/// <summary>
		/// Выключатель
		/// </summary>
		Switch = 1,

		/// <summary>
		/// Розетка
		/// </summary>
		Outlet
	}

	public interface IDeviceBase
	{
		int ID { get; }

		string Name { get; }

		string Description { get; }

		bool IsConnected { get; }

		IPAddress IP { get; }

		MacAddress Mac { get; }

		DeviceType DeviceType { get; }

		FirmwareType FirmwareType { get; }
	}
}

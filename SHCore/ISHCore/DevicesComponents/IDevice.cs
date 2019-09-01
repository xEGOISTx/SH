using System.Net;

namespace SH.Core.DevicesComponents
{
	/// <summary>
	/// Основа всех устройств
	/// </summary>
	public interface IDevice
	{
		/// <summary>
		/// Уникальный идентификатор
		/// </summary>
		int ID { get; }

		/// <summary>
		/// Системное имя устройства
		/// </summary>
		//string SystemName { get; }

		/// <summary>
		/// Назвние устройства
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Признак наличия соединения с устройством 
		/// </summary>
		bool IsConnected { get; }

		/// <summary>
		/// IP адрес устройства
		/// </summary>
		IPAddress IP { get; }

		/// <summary>
		/// Mac адрес устройства
		/// </summary>
		MacAddress Mac { get; }

		/// <summary>
		/// Тип устройства
		/// </summary>
		int DeviceType { get; }

		/// <summary>
		/// Тип прошивки. Характерезует физическую модель устройства
		/// </summary>
		//int FirmwareType { get; }

		/// <summary>
		/// Список исполняемых задачь устройством
		/// </summary>
		IDeviceCommandList Commands { get; }
	}
}

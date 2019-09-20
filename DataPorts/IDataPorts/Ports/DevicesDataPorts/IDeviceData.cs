namespace SH.DataPorts
{
	/// <summary>
	/// Сохраняемая информация об устройстве
	/// </summary>
	public interface IDeviceData
	{
		/// <summary>
		/// Уникальный идентификатор
		/// </summary>
		int ID { get; }

		/// <summary>
		/// Mac адрес
		/// </summary>
		string MacAddress { get; }

		/// <summary>
		/// Тип устройства
		/// </summary>
		int DeviceType { get;}

		/// <summary>
		/// Тип прошивки. Характерезует физическую модель устройства
		/// </summary>
		//int FirmwareType { get; }

		/// <summary>
		/// Название устройства
		/// </summary>
		string Description { get; }

        /// <summary>
        /// Команды устройства
        /// </summary>
        IDeviceCommandData[] Commands { get; }
	}
}

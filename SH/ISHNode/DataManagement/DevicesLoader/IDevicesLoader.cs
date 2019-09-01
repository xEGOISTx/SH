using SH.Core;

namespace SH.DataManagement
{
	/// <summary>
	/// Загрузчик устройств
	/// </summary>
	public interface IDevicesLoader
	{
		/// <summary>
		/// Возвращает устройства указанного типа
		/// </summary>
		/// <param name="devicesType"></param>
		/// <returns></returns>
		IOperationResultDevicesLoad LoadDevices(int devicesType);


		/// <summary>
		/// Сохранить устройство
		/// </summary>
		/// <param name="device"></param>
		/// <param name="commands"></param>
		/// <returns></returns>
		IOperationResult SaveDevice(IDeviceData device);

        /// <summary>
        /// Обновить параметры команд
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        IOperationResult UpdateDeviceCommands(IDeviceCommandData[] commands);

		/// <summary>
		/// Удалить устройство
		/// </summary>
		/// <param name="deviceID"></param>
		/// <returns></returns>
		IOperationResult RemoveDevice(int deviceID);
	}
}

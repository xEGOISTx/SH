using SHBase;
using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit.Communication
{
	public interface ICommunicator
	{
		/// <summary>
		/// Отправить устройству задачу действия с пинами
		/// </summary>
		/// <typeparam name="T">Тип действия</typeparam>
		/// <param name="task">Задача</param>
		/// <returns></returns>
		Task<bool> SendGPIOTask<T>(IBaseGPIOActionTask<T> task) where T : IBaseGPIOAction;

		/// <summary>
		/// Возвращает базовую инфу об устройстве
		/// </summary>
		/// <returns></returns>
		Task<IOperationGetBaseInfoResult> GetDeviceInfo(IPAddress deviceIP, bool asAP = false);

		/// <summary>
		/// Получить ID устройства
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		Task<int> GetDeviceID(IPAddress deviceIP);

		/// <summary>
		/// Отпрпавить id устройству
		/// </summary>
		/// <param name="id"></param>
		/// <param name="deviceIP"></param>
		/// <returns></returns>
		Task<IOperationResult> SendIdToDevice(int id, IPAddress deviceIP);

		/// <summary>
		/// Проверить соединение с устройством
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		Task<bool> CheckConnection(IDeviceBase device);
	}
}

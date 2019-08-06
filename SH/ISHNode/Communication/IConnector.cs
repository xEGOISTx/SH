using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SH.Communication
{
	/// <summary>
	/// Инструмент соединения с устройствами
	/// </summary>
	public interface IConnector
	{
		/// <summary>
		/// Кол-во найденных устройств
		/// </summary>
		int CountFoundAP { get; }

		/// <summary>
		/// Список найденных устройств
		/// </summary>
		IEnumerable<IAP> APs { get; }

		/// <summary>
		/// Найти точки с указанными именами
		/// </summary>
		/// <param name="ssids"></param>
		void FindAPs(IAPSSIDs ssids);

		/// <summary>
		/// Подключиться к указанной точке
		/// </summary>
		/// <param name="aP"></param>
		/// <returns></returns>
		bool ConnectTo(IAP aP);

		/// <summary>
		/// Отключится от указанной точки
		/// </summary>
		/// <param name="aP"></param>
		void Disconnect(IAP aP);

		/// <summary>
		/// Очистить список найденных точек
		/// </summary>
		void Clear();

		/// <summary>
		/// Возвращает IP хоста
		/// </summary>
		/// <returns></returns>
		IPAddress GetHostIP();
	}
}

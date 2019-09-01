using System;
using System.Net;

namespace SH.Communication
{
	/// <summary>
	/// Параметры подключения
	/// </summary>
	public interface IConnectionParams
	{

		Uri RouterUriToParse { get; }

		string RouterLogin { get; }

		string RouterPassword { get; }

		string RouterSsid { get; }

		string RouterAPPassword { get; }


		/// <summary>
		/// Список имён точек для поиска
		/// </summary>
		IAPSSIDs APSSIDsForSearch { get; }

		/// <summary>
		/// IP устройства по умолчанию
		/// </summary>
		IPAddress DeviceDafaultIP { get; }

		/// <summary>
		/// Пароль устройства как точки доступа
		/// </summary>
		string DeviceAPPassword { get; }


		/// <summary>
		/// Редактор параметров
		/// </summary>
		IConnectionParamsEditor Editor { get; }

	}
}

using System.Net;

namespace SH.Communication
{
	/// <summary>
	/// Параметры подключения
	/// </summary>
	public interface IConnectionParams
	{
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
		/// Параметры для подключения к роутеру
		/// </summary>
		IConnectionParamsToRouter ConnectionParamsToRouter { get; }

		/// <summary>
		/// Редактор параметров
		/// </summary>
		IConnectionParamsEditor Editor { get; }

	}
}

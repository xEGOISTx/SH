using System.Net;

namespace SH.Communication
{
	/// <summary>
	/// Редактор параметров подключения
	/// </summary>
	public interface IConnectionParamsEditor
	{
		/// <summary>
		/// Признак редактирования
		/// </summary>
		bool IsEditing { get; }

		/// <summary>
		/// Изменить IP роутера
		/// </summary>
		/// <param name="ip"></param>
		void ChangeRouterIP(IPAddress ip);

		/// <summary>
		/// Изменить логин подключения к роутеру
		/// </summary>
		/// <param name="routerLogin"></param>
		void ChangeRouterLogin(string routerLogin);

		/// <summary>
		/// Изменить пароль подключения к роутеру
		/// </summary>
		/// <param name="routerPassword"></param>
		void ChangeRouterPassword(string routerPassword);

		/// <summary>
		/// Изменить ssid
		/// </summary>
		/// <param name="routerSsid"></param>
		void ChangeRouterSsid(string routerSsid);

		/// <summary>
		/// Изменить список ssids для поиска
		/// </summary>
		/// <param name="aPSSIDsForSearch"></param>
		void ChangeAPSSIDsForSearch(string[] aPSSIDsForSearch);

		/// <summary>
		/// Изменить пароль подключения к роутеру как точки доступа
		/// </summary>
		/// <param name="routerAPPassword"></param>
		void ChangeRouterAPPassword(string routerAPPassword);

		/// <summary>
		/// Изменить дефолтный IP подсключения к устройству
		/// </summary>
		/// <param name="ip"></param>
		void ChangeDeviceDafaultIP(IPAddress ip);

		/// <summary>
		/// Изменить пароль подключения к устройству как точки доступа
		/// </summary>
		/// <param name="deviceAPPassword"></param>
		void ChangeDeviceAPPassword(string deviceAPPassword);

		/// <summary>
		/// Начать редактирование
		/// </summary>
		void StartEditing();

		/// <summary>
		/// Завершить редактирование
		/// </summary>
		/// <param name="applyCancelChanges"></param>
		void EndEditing(bool applyCancelChanges);
	}
}

using System.Net;
using SH.Core;

namespace SH.Communication
{
	/// <summary>
	/// Параметры для подключения к точке
	/// </summary>
	public interface IConnectionParamsToRouter
	{
		/// <summary>
		/// IP роутера
		/// </summary>
		IPAddress RouterIP { get; }

		/// <summary>
		/// Параметры для подключения к роутеру как к точке доступа
		/// </summary>
		IConnectionParamsToAP ConnectionParams { get; }

		/// <summary>
		/// Учётные данные роутера
		/// </summary>
		Core.ICredentials Credentials { get; }
	}
}

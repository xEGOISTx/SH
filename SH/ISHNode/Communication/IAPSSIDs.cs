using System.Collections.Generic;

namespace SH.Communication
{
	/// <summary>
	/// Список имён точек доступа
	/// </summary>
	public interface IAPSSIDs
	{
		IEnumerable<string> List { get; }

		/// <summary>
		/// Проверить наличие имени в списке
		/// </summary>
		/// <param name="ssid"></param>
		/// <returns></returns>
		bool Contains(string ssid);
	}
}

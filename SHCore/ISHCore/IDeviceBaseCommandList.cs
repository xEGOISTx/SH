using System.Collections.Generic;

namespace SH.Core
{
	/// <summary>
	/// Список команд
	/// </summary>
	public interface IDeviceBaseCommandList : IEnumerable<IDeviceCommand>
	{
		/// <summary>
		/// Возвращает задачу по id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		IDeviceCommand GetByID(int id);

		/// <summary>
		/// Возвращает задачу по названию голосовой команды
		/// </summary>
		/// <param name="voiceCommand"></param>
		/// <returns></returns>
		IDeviceCommand GetByVoiceCommand(string voiceCommand);
	}
}

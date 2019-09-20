using System.Collections.Generic;

namespace SH.Core.DevicesComponents
{
	/// <summary>
	/// Список команд
	/// </summary>
	public interface IDeviceCommandList : IEnumerable<IDeviceCommand>
	{
        int OwnerID { get; }

		/// <summary>
		/// Редактор команд
		/// </summary>
		IDeviceCommandEditor Editor { get; }

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
		//IDeviceCommand GetByVoiceCommand(string voiceCommand);

		/// <summary>
		/// Добавить команду
		/// </summary>
		/// <param name="command"></param>
		//void Add(IDeviceCommand command);
	}
}

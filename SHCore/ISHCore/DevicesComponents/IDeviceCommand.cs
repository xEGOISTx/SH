namespace SH.Core.DevicesComponents
{
	/// <summary>
	/// Команда для устройства
	/// </summary>
	public interface IDeviceCommand
	{
        IDevice Owner { get; }

        /// <summary>
        /// Идентификатор команды
        /// </summary>
        int ID { get; }

		string CommandName { get; }

		/// <summary>
		/// Описание
		/// </summary>
		string Description { get;}

		/// <summary>
		/// Голосовая команда
		/// </summary>
		string VoiceCommand { get; }

		/// <summary>
		/// Выполнить команду
		/// </summary>
		void Execute(string parameter = null);
	}
}

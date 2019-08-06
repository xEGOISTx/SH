using System;

namespace SH.Core
{
	/// <summary>
	/// Команда для устройства
	/// </summary>
	public class DeviceCommand : IDeviceCommand
	{
		private readonly string _commandName;
		private readonly IDeviceBase _owner;
		private readonly CommandSender _sender = new CommandSender();

		/// <summary>
		/// Инициализация команды
		/// </summary>
		/// <param name="owner">Устройство владелец команды</param>
		/// <param name="commandName">Название команды на устройстве</param>
		/// <param name="voiceCommand">Голосовая команда</param>
		public DeviceCommand(IDeviceBase owner,string commandName , string voiceCommand)
		{
			if(string.IsNullOrEmpty(voiceCommand))
			{
				throw new Exception("Голосовая команда не может быть null или empty");
			}

			VoiceCommand = voiceCommand;
			_commandName = commandName;
			_owner = owner;
		}

		/// <summary>
		/// Идентификатор команды
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Голосовая команда
		/// </summary>
		public string VoiceCommand { get; set; }

		/// <summary>
		/// Выполнить команду
		/// </summary>
		public void Execute()
		{
			_sender.SendCommandToDevice(_owner.IP, _commandName);
		}
	}
}

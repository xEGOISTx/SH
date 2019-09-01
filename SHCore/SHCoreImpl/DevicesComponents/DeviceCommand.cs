namespace SH.Core.DevicesComponents
{
	/// <summary>
	/// Команда для устройства
	/// </summary>
	public class DeviceCommand : IDeviceCommand
	{
		private readonly string _commandName;
		private readonly IDevice _owner;
		private readonly CommandSender _sender = new CommandSender();

		/// <summary>
		/// Инициализация команды
		/// </summary>
		/// <param name="owner">Устройство владелец команды</param>
		/// <param name="commandName">Название команды на устройстве</param>
		/// <param name="voiceCommand">Голосовая команда</param>
		public DeviceCommand(IDevice owner,int id, string commandName)
		{
			//VoiceCommand = voiceCommand;
			_commandName = commandName;
            ID = id;
			_owner = owner;
		}

        public IDevice Owner { get; }

		/// <summary>
		/// Идентификатор команды
		/// </summary>
		public int ID { get; }

		public string CommandName { get; }
		
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
		public void Execute(string parameter = null)
		{
			_sender.SendCommandToDevice(_owner.IP, _commandName, parameter);
		}

        public DeviceCommand GetCopy()
        {
            return new DeviceCommand(Owner, ID, CommandName)
            {
                Description = Description,
                VoiceCommand = VoiceCommand,
            };
        }
    }
}

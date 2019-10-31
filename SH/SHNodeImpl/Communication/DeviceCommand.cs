using SH.Core.DevicesComponents;
using System.Net;

namespace SH.Communication
{
	/// <summary>
	/// Команда для устройства
	/// </summary>
	internal class DeviceCommand : IDeviceCommand
	{
		private readonly CommandSender _sender = new CommandSender();

        /// <summary>
        /// Инициализация команды
        /// </summary>
        /// <param name="ownerIP"></param>
        /// <param name="id"></param>
        /// <param name="commandName"></param>
        public DeviceCommand(IPAddress ownerIP, int id)
		{
			//VoiceCommand = voiceCommand;
            ID = id;
            OwnerIP = ownerIP;
		}

        public IPAddress OwnerIP { get; internal set; }

		/// <summary>
		/// Идентификатор команды
		/// </summary>
		public int ID { get; }
	
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
			_sender.SendCommandToDevice(OwnerIP, ID, parameter);
		}

        public DeviceCommand GetCopy()
        {
            return new DeviceCommand(OwnerIP, ID)
            {
                Description = Description,
                VoiceCommand = VoiceCommand,
            };
        }
    }
}

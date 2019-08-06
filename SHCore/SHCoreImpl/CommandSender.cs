using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace SH.Core
{
	/// <summary>
	/// Отправитель команд устройству
	/// </summary>
	internal class CommandSender : RequestsSender
	{
		private bool _taskIsCompleted;

		/// <summary>
		/// Отправить команду устройству
		/// </summary>
		/// <param name="devIP"></param>
		/// <param name="commandName"></param>
		public async void SendCommandToDevice(IPAddress devIP, string commandName)
		{
			if (_taskIsCompleted)
			{
				_taskIsCompleted = false;
				IRequestOperationResult result = await SendToDevice(devIP, commandName);
				_taskIsCompleted = true;
			}

		}
	}
}

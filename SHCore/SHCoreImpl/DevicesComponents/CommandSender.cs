using System.Collections.Generic;
using System.Net;

namespace SH.Core.DevicesComponents
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
		public async void SendCommandToDevice(IPAddress devIP, string commandName, string parameter = null)
		{
			if (_taskIsCompleted)
			{
				_taskIsCompleted = false;
				if(parameter == null)
				{
					parameter = string.Empty;
				}

				List<RequestParameter> rParams = new List<RequestParameter>
				{
					new RequestParameter("p1", parameter)
				};


				IRequestOperationResult result = await SendToDeviceAsync(devIP, commandName, rParams);
				_taskIsCompleted = true;
			}

		}
	}
}

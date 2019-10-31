using SH.Core;
using System.Collections.Generic;
using System.Net;

namespace SH.Communication
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
		/// <param name="commandID"></param>
		public async void SendCommandToDevice(IPAddress devIP, int commandID, string parameter = null)
		{
			if (_taskIsCompleted)
			{
				_taskIsCompleted = false;
                List<RequestParameter> rParams = new List<RequestParameter>();

                if (parameter == null && parameter != string.Empty)
				{
                    rParams.Add(new RequestParameter("p1", parameter));
                }

				IRequestOperationResult result = await SendToDeviceAsync(devIP, commandID.ToString(), rParams);
				_taskIsCompleted = true;
			}

		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SH.Communication
{
	internal class GetDeviceCommandsOperationResult : Core.IOperationResult
	{
		public IEnumerable<DeviceCommandInfo> CommandsInfos{ get; set; }

		public bool Success { get; set; }

		public string ErrorMessage { get; set; }
	}
}

using System;

namespace SH.Core.DevicesComponents
{
	public interface IDeviceCommandEditor : IEditor
	{
		void ChangeDescription(IDeviceCommand command, string description);

		void ChangeVoiceCommand(IDeviceCommand command, string voiceCommand);
	}
}

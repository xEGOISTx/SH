using System;
using System.Collections.Generic;
using System.Text;

namespace SH.Core.DevicesComponents
{
    public interface IDefaultDeviceCommandParams
    {
        string VoiceCommand { get; }

        string Description { get; }
    }
}

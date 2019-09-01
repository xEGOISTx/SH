using SH.Core;
using SH.Core.DevicesComponents;
using System.Net;

namespace SH.Communication
{
    internal class DeviceConnectionState : IDeviceConnectionState
    {
        public IDevice Device { get; set; }

        public IPAddress IP { get; set; }

        public bool ConnectionState { get; set; }
    }
}

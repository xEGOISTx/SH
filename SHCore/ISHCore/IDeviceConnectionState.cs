using SH.Core.DevicesComponents;
using System.Net;

namespace SH.Core
{
	public interface IDeviceConnectionState
	{
		IDevice Device { get; }

        IPAddress IP { get; }

		bool ConnectionState { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SH.Communication
{
	public interface IDevicesRequestsListener
	{
		Task StartListening();

		void StopListening();

		event DeviceRequestEventHandler DeviceRequest;
	}
}

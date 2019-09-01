using System;
using System.Collections.Generic;
using System.Text;

namespace SH.Communication
{
	public interface IDevicesRequestsListener
	{
		void StartListening();

		void StopListening();

		event DeviceRequestEventHandler DeviceRequest;
	}
}

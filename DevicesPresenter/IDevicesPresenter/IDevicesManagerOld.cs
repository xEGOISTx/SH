using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFi;

namespace DevicesPresenter
{
	public interface IDevicesManagerOld
	{




		IDeviceCommonListOld Devices { get; }

		IConnectionSettings ConnectionSettings { get; }

		Task<bool> FindAndConnectDevicesAsync();

		void SynchronizationWithDevicesAsync();

		IDeviceEditor GetDeviceEditor();



		event EventHandler LoadDevicesComplete;

		}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFi;

namespace DevicesPresenter
{
	public interface IDevicesManager
	{




		IDeviceCommonList Devices { get; }

		IConnectionSettings ConnectionSettings { get; }

		IDeviceEditor GetDeviceEditor();

		Task<bool> FindAndConnectDevicesAsync();

		void SynchronizationWithDevicesAsync();

		event EventHandler LoadDevicesComplete;

		}
}

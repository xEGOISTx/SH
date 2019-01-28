using Switches;
using Switches.SwitchesOutlets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesPresenter
{
	public interface IDevicesManager
	{
		IConnectionSettings ConnectionSettings { get; }

		Task<bool> FindAndConnectDevicesAsync();

		Task<bool> SynchronizationWithDevicesAsync();

		ISwitches GetSwitches();
	}
}

using DataManager;
using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switches.SwitchesOutlets
{
	public interface ISwitchesAndOutletsLoader
	{
		Task<IResultOperationLoad> LoadDevices();

		Task<IResultOperationSave> SaveDevices(IDeviceInfo[] devices);

		Task<IDBOperationResult> RenameDevice(IDeviceInfo device);
	}
}

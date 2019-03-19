using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataManager;

namespace Switches
{
	public class SwitchEditor : ISwitchEditor
	{

		public void Rename(IBaseSwitch sw, string description)
		{
			if (sw.Description != description)
			{
				DataManager.DataManager dM = new DataManager.DataManager();


				IDeviceInfo deviceInfo = new DeviceInfo
				{
					ID = sw.ID,
					Description = description
				};

				IDBOperationResult result = dM.RenameDevice(deviceInfo);

				if (result.Success)
					(sw as BaseSwitch).Description = description;

			}
		}
	}
}

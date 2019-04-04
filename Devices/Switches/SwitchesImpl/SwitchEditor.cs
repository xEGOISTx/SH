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
		private readonly IDataManager _data;

		public SwitchEditor(IDataManager dataManager)
		{
			_data = dataManager;
		}

		public void Rename(IBaseSwitch sw, string description)
		{
			if (sw.Description != description)
			{
				IDeviceInfo deviceInfo = _data.CreateDeviceInfo(sw.Description, sw.DeviceType, (int)sw.FirmwareType, sw.Mac.ToString(), sw.ID);

				IDBOperationResult result = _data.RenameDevice(deviceInfo);

				if (result.Success)
					(sw as BaseSwitch).Description = description;

			}
		}
	}
}

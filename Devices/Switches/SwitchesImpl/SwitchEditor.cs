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
				IDBOperationResult result = _data.RenameDevice(sw.ID, description);

				if (result.Success)
					(sw as BaseSwitch).Description = description;

			}
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesPresenter
{
	public class SwitchesList : DeviceBaseList<ushort, ISwitchingDevice> , ISwitchesList
	{
		public void Add(ISwitchingDevice device)
		{
			if (!ContainsKey(device.ID))
			{
				_devices.Add(device.ID, device);
			}
		}
	}
}

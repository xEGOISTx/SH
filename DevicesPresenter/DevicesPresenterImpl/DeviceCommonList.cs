using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Switches.SwitchesOutlets;

namespace DevicesPresenter
{
	public class DeviceCommonList : DeviceCommonBaseList
	{
		public DeviceCommonList()
		{			
			Add(new Switches.Switches());
		}
	}
}

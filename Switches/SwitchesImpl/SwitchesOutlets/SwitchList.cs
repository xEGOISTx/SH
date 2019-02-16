using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace Switches
{
	public class SwitchList : SwitchesAndOutletsBaseList<ISwitch>, ISwitchList
	{
		public SwitchList() : base(DeviceType.Switch) { }

		internal override ISwitchesLoader GetLoader()
		{
			return new SwitchesLoader(DevicesType);
		}

		internal override DBConvertor<ISwitch> Convertor => new SwitchesConvertor();
	}
}

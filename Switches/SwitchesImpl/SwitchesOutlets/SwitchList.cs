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
		public SwitchList() : base(DeviceTypes.SWITCH) { }

		internal override DBConvertor Convertor => new SwitchesConvertor();
	}
}

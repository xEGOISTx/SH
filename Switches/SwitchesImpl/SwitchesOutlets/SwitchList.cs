using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace Switches.SwitchesOutlets
{
	public class SwitchList : SwitchesAndOutletsListBaseImpl, ISwitchesAndOutletsList
	{
		public SwitchList() : base(DeviceType.Switch) { }


		public override ISwitchesLoader GetLoader()
		{
			return new SwitchesLoader(DevicesType);
		}
	}
}

using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switches.SwitchesOutlets
{
	public class OutletList : SwitchesAndOutletsListBaseImpl, ISwitchesAndOutletsList
	{
		public OutletList() : base(DeviceType.Outlet) { }


		public override ISwitchesLoader GetLoader()
		{
			return new SwitchesLoader(DevicesType);
		}
	}
}

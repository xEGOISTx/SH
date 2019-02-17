using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switches.SwitchesOutlets
{
	public class OutletList : SwitchesAndOutletsBaseList<IOutlet>, IOutletList
	{
		public OutletList() : base(DeviceType.Outlet) { }

		internal override ISwitchesLoader Loader => new SwitchesLoader(DevicesType);


		internal override DBConvertor Convertor => new OutletsConvertor();

	}
}

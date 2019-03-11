using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switches
{
	public class OutletList : SwitchesAndOutletsBaseList<IOutlet>, IOutletList
	{
		public OutletList() : base(DeviceTypes.OUTLET) { }

		internal override DBConvertor Convertor => new OutletsConvertor();

	}
}

using Switches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchesControls.ViewModels
{
	public class OutletViewModel : SwitchOutletBaseViewModel
	{
		public readonly IOutlet _outlet;

		public OutletViewModel(IOutlet outlet, ISwitchEditor switchEditor) : base(outlet, switchEditor)
		{
			_outlet = outlet;
		}
	}
}

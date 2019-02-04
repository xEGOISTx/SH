using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Switches;
using Switches.SwitchesOutlets;

namespace SwitchesControls.ViewModels
{
	public class SwitchesViewModel
	{
		private readonly ISwitches _switches;

		public SwitchesViewModel(ISwitches switches)
		{
			_switches = switches;

			Switches = new SwitchsOutletsListViewModel(_switches.SwitchList);
			Outlets = new SwitchsOutletsListViewModel(_switches.OutletList);
		}

		public SwitchsOutletsListViewModel Switches { get; }

		public SwitchsOutletsListViewModel Outlets { get; }

		public void Refresh()
		{
			Switches.Refresh();
			Outlets.Refresh();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Switches;

namespace SwitchesControls.ViewModels
{
	public class SwitchesViewModel
	{
		private readonly ISwitches _switches;

		public SwitchesViewModel(ISwitches switches)
		{
			_switches = switches;
		}

		public ObservableCollection<SwitchOutletViewModel> Switches { get; }

		public ObservableCollection<SwitchOutletViewModel> Outlets { get; }
	}
}

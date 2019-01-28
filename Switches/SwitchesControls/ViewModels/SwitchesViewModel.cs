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

		//public ObservableCollection<SwitchOutletViewModel> Switches { get; } = new ObservableCollection<SwitchOutletViewModel>();

		//public ObservableCollection<SwitchOutletViewModel> Outlets { get; } = new ObservableCollection<SwitchOutletViewModel>();

		//private void InitLists()
		//{
		//	foreach(ISwitchOutlet device in _switches.SwitchList)
		//	{
		//		SwitchOutletViewModel deviceVM = new SwitchOutletViewModel(device);
		//		Switches.Add(deviceVM);
		//	}

		//	foreach (ISwitchOutlet device in _switches.OutletList)
		//	{
		//		SwitchOutletViewModel deviceVM = new SwitchOutletViewModel(device);
		//		Outlets.Add(deviceVM);
		//	}
		//}
	}
}

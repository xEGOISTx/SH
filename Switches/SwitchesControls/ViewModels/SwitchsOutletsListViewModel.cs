using System.Collections.Generic;
using System.Collections.ObjectModel;
using Switches.SwitchesOutlets;
using UWPHelper;

namespace SwitchesControls.ViewModels
{
	public class SwitchsOutletsListViewModel : BaseViewModel
	{
		private readonly ISwitchesAndOutletsList _devices;

		public SwitchsOutletsListViewModel(ISwitchesAndOutletsList switchesAndOutletsList)
		{
			_devices = switchesAndOutletsList;
			Refresh();
		}



		public IEnumerable<SwitchOutletViewModel> List { get; private set; }

		public void Refresh()
		{
			List<SwitchOutletViewModel> devs = new List<SwitchOutletViewModel>();
			foreach (ISwitchOutlet device in _devices)
			{
				SwitchOutletViewModel deviceVM = new SwitchOutletViewModel(device);
				devs.Add(deviceVM);
			}

			List = devs;
			OnPropertyChanged(nameof(List));
		}
	}
}

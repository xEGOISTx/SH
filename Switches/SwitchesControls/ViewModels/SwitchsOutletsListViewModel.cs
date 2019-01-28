using System.Collections.ObjectModel;
using Switches.SwitchesOutlets;

namespace SwitchesControls.ViewModels
{
	public class SwitchsOutletsListViewModel
	{
		ISwitchesAndOutletsList _devices;

		public SwitchsOutletsListViewModel(ISwitchesAndOutletsList switchesAndOutletsList)
		{
			_devices = switchesAndOutletsList;

			foreach (ISwitchOutlet device in switchesAndOutletsList)
			{
				SwitchOutletViewModel deviceVM = new SwitchOutletViewModel(device);
				List.Add(deviceVM);
			}
		}

		public ObservableCollection<SwitchOutletViewModel> List { get; } = new ObservableCollection<SwitchOutletViewModel>();
	}
}

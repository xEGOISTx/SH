using System.Collections.Generic;
using System.Collections.ObjectModel;
using Switches;
using UWPHelper;

namespace SwitchesControls.ViewModels
{
	public class SwitchListViewModel : BaseViewModel
	{
		private readonly ISwitchList _devices;

		public SwitchListViewModel(ISwitchList switchList)
		{
			_devices = switchList;
			Refresh();
		}



		public IEnumerable<SwitchViewModel> List { get; private set; }

		public void Refresh()
		{
			List<SwitchViewModel> devs = new List<SwitchViewModel>();
			foreach (ISwitch device in _devices)
			{
				SwitchViewModel deviceVM = new SwitchViewModel(device);
				devs.Add(deviceVM);
			}

			List = devs;
			OnPropertyChanged(nameof(List));
		}
	}
}

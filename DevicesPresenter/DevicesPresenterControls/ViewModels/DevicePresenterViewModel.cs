using DevicesPresenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwitchesControls.ViewModels;
using UWPHelper;

namespace DevicesPresenterControls.ViewModels
{
	public class DevicePresenterViewModel : BaseViewModel
	{
		private readonly IDevicesManager _manager;

		public DevicePresenterViewModel(IDevicesManager manager)
		{
			_manager = manager;
			FindDevices = new RelayCommand(ExecuteFindDevices);

			Switches = new SwitchesViewModel(_manager.GetSwitches());
		}

		public SwitchesViewModel Switches { get; }

		public RelayCommand FindDevices { get; private set; }
		private async void ExecuteFindDevices(object param)
		{
			await _manager.FindAndConnectDevicesAsync();
			Switches.Refresh();
		}

	}
}

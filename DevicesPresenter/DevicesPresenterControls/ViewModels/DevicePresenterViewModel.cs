using DevicesPresenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwitchesControls.ViewModels;

namespace DevicesPresenterControls.ViewModels
{
	public class DevicePresenterViewModel
	{
		private readonly IDevicesManager _manager;

		public DevicePresenterViewModel(IDevicesManager manager)
		{
			_manager = manager;

			Switches = new SwitchesViewModel(_manager.GetSwitches());
		}

		public SwitchesViewModel Switches { get; }
	}
}

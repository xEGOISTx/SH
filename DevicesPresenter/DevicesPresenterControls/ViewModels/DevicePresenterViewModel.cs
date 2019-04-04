using DevicesPresenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwitchesControls.ViewModels;
using UWPHelper;
using SHBase;

namespace DevicesPresenterControls.ViewModels
{
	public class DevicePresenterViewModel : BaseViewModel
	{
		private enum RefreshMode
		{
			Full,
			State
		}


		private readonly IDevicesManager _manager;
		private bool _devicePresenterVisibility;

		public DevicePresenterViewModel(IDevicesManager manager)
		{
			_manager = manager;
			FindDevices = new RelayCommand(ExecuteFindDevices);
			Update = new RelayCommand(ExecuteUpdate);

			Switches = new SwitchesViewModel(manager);
		}

		public SwitchesViewModel Switches { get; }

		public bool DevicePresenterVisibility
		{
			get { return _devicePresenterVisibility; }
			set
			{
				_devicePresenterVisibility = value;
				OnPropertyChanged(nameof(DevicePresenterVisibility));
				OnPropertyChanged(nameof(PBIsActive));
			}
		}

		public bool PBIsActive
		{
			get { return !_devicePresenterVisibility; }
		}


		public RelayCommand FindDevices { get; private set; }
		private async void ExecuteFindDevices(object param)
		{
			await ExecuteProcess(_manager.FindAndConnectDevicesAsync(), RefreshMode.Full);
		}

		public RelayCommand Update { get; private set; }
		private async void ExecuteUpdate(object param)
		{
			await ExecuteProcess(_manager.SynchronizationWithDevicesAsync(), RefreshMode.State);
		}

		public void FullRefresh()
		{
			Switches.FullRefresh();
		}

		public void RefreshState()
		{
			Switches.RefreshState();
		}

		private async Task<bool> ExecuteProcess(Task<bool> process, RefreshMode mode)
		{
			DevicePresenterVisibility = false;
			bool res = await process;

			if (mode == RefreshMode.Full || Switches.CommonCount == 0)
				FullRefresh();
			else
				RefreshState();
			
			DevicePresenterVisibility = true;
			return res;
		}

	}
}

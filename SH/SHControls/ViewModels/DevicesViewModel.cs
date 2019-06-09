using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwitchesControls.ViewModels;
using UWPHelper;
using SHToolKit.DevicesManagement;

namespace SHControls.ViewModels
{
	public class DevicesViewModel : BaseViewModel
	{
		private enum RefreshMode
		{
			Full,
			State
		}


		private readonly IDevicesManager _manager;
		private bool _isEnabled;

		public DevicesViewModel(IDevicesManager manager)
		{
			_manager = manager;
			FindDevices = new RelayCommand(ExecuteFindDevices);
			Refresh = new RelayCommand(ExecuteRefresh);

			Switches = new SwitchesViewModel(manager);
		}

		public SwitchesViewModel Switches { get; }

		public bool IsEnabled
		{
			get { return _isEnabled; }
			set
			{
				_isEnabled = value;
				OnPropertyChanged(nameof(IsEnabled));
				OnPropertyChanged(nameof(PBIsActive));
			}
		}

		public bool PBIsActive
		{
			get { return !_isEnabled; }
		}


		public RelayCommand FindDevices { get; private set; }
		private async void ExecuteFindDevices(object param)
		{
			await ExecuteProcess(_manager.FindAndConnectDevicesAsync(), RefreshMode.Full);
		}

		public RelayCommand Refresh { get; private set; }
		private async void ExecuteRefresh(object param)
		{
			await ExecuteProcess(_manager.SynchronizationWithDevicesAsync(), RefreshMode.State);
		}

		internal void FullRefresh()
		{
			Switches.FullRefresh();
		}

		internal void RefreshState()
		{
			Switches.RefreshState();
		}

		private async Task<bool> ExecuteProcess(Task<bool> process, RefreshMode mode)
		{
			IsEnabled = false;
			bool res = await process;

			if (mode == RefreshMode.Full || Switches.CommonCount == 0)
				FullRefresh();
			else
				RefreshState();
			
			IsEnabled = true;
			return res;
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwitchesControls.ViewModels;
using UWPHelper;
using SH;
using SHBase;
using SHToolKit;
using SHToolKit.DataManagement;
using SHToolKit.DevicesManagement;

namespace SHControls.ViewModels
{
	public class DevicesViewModel : BaseViewModel
	{
		private delegate Task<IOperationResult> Process();
		private readonly ITools _tools;
		private readonly IDataLoader _loader;
		private readonly IDevicesManager _manager;
		private bool _isEnabled;

		private enum RefreshMode
		{
			Full,
			State
		}


		public DevicesViewModel(IDevicesManager manager, IDataLoader loader, ITools tools)
		{
			_manager = manager;
			_tools = tools;
			_loader = loader;

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
			await ExecuteProcess(FindAndConnectDevicesAsync, RefreshMode.Full);
		}

		public RelayCommand Refresh { get; private set; }
		private async void ExecuteRefresh(object param)
		{
			await ExecuteProcess(RefreshDevicesAsync, RefreshMode.State);
		}

		internal void FullRefresh()
		{
			Switches.FullRefresh();
		}

		internal void RefreshState()
		{
			Switches.RefreshState();
		}

		internal async Task<IOperationResult> PreInit()
		{
			return await ExecuteProcess(Init, RefreshMode.Full);
		}

		private async Task<IOperationResult> Init()
		{
			IOperationResult loadRes = await _manager.LoadDataFromRepository(_loader);
			if (!loadRes.Success) { return loadRes; }

			IOperationResult refreshRes = await RefreshDevicesAsync();

			return refreshRes;
		}

		private async Task<IOperationResult> FindAndConnectDevicesAsync()
		{
			IFindDevicesOperationResult foundDevsRes = await _manager.FindAndConnectToRouterNewDevicesAsync(_tools.GetDevicesFinder());
			if (!foundDevsRes.Success) { return foundDevsRes; }

			IOperationResult saveResult = await _manager.SaveAndDistributeNewDevices(foundDevsRes, _loader.GetDevicesLoader(), _tools.GetCommunicator());

			return saveResult;
		}

		private async Task<IOperationResult> RefreshDevicesAsync()
		{
			IDevicesConnectionInfo connectionInfo = await _manager.GetDevicesConnectionInfo(_tools.GetDevicesFinder());

			_manager.RefreshDevicesConnectionState(connectionInfo);

			IFindDevicesOperationResult foundDevsRes = await _manager.FindDevicesAtRouterIfItsConn(_tools.GetDevicesFinder(), connectionInfo);
			if (!foundDevsRes.Success) { return foundDevsRes; }

			IOperationResult result = await _manager.DevicesSynchronization(foundDevsRes);

			return result;
		}

		private async Task<IOperationResult> ExecuteProcess(Process process, RefreshMode mode)
		{
			IsEnabled = false;
			IOperationResult res = await process();

			if (mode == RefreshMode.Full )
				FullRefresh();
			else
				RefreshState();
			
			IsEnabled = true;
			return res;
		}

	}
}

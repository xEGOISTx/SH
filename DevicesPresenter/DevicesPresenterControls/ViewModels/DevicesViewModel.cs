using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevicesPresenter;
using UWPHelper;
using Windows.Devices.WiFi;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DevicesPresenterControls.ViewModels
{
	public class DevicesViewModel : BaseViewModel
	{
		private readonly IDevicesManager _devicesManager;
		private ConnectionParamsViewModel _connectionParamsVM;
		private DeviceViewModel _selectedDevice;

		public List<DeviceViewModel> Devices { get; private set; } = new List<DeviceViewModel>();

		public DevicesViewModel(IDevicesManager devicesManager)
		{
			_devicesManager = devicesManager;
			_devicesManager.LoadDevicesComplete += DevicesManager_LoadDevicesComplete;
			_devicesManager.SynchronizationWithDevicesAsync();


			FindAndConnect = new RelayCommand(ExecuteFindAndConnect);
			SendId = new RelayCommand(ExecuteSendId);
			GetId = new RelayCommand(ExecuteGetId);
		}

		private void DevicesManager_LoadDevicesComplete(object sender, EventArgs e)
		{
			foreach (ISwitchingDevice device in _devicesManager.Devices.GetDevices<ISwitchesList>().Values)
			{
				DeviceViewModel deviceVM = new DeviceViewModel(device);
				Devices.Add(deviceVM);
			}
			OnPropertyChanged(nameof(Devices));
		}



		#region Properties
		public ConnectionParamsViewModel ConnectionParamsVM
		{
			get
			{
				if(_connectionParamsVM == null)
				{
					_connectionParamsVM = new ConnectionParamsViewModel(_devicesManager.ConnectionSettings);
				}

				return _connectionParamsVM;
			}
		}
		
		public DeviceViewModel SelectedDevice
		{
			get { return _selectedDevice; }
			set
			{
				DevicePresenterViewModel devicePresenterVM = new DevicePresenterViewModel(value, _devicesManager.GetDeviceEditor());
				(Window.Current.Content as Frame).Navigate(typeof(Views.DeviceView), devicePresenterVM);
				_selectedDevice = null;
				//DevicePresenterVM.SetDevice(value);
				OnPropertyChanged(nameof(SelectedDevice));

			}
		}

		#endregion Properties



		#region Commands
		public RelayCommand FindAndConnect { get;  set; }
		private async void ExecuteFindAndConnect(object param)
		{
			ushort selectedDeviceID = SelectedDevice != null ? SelectedDevice.ID : ushort.MinValue;
			DeviceViewModel forSelect = null;

			await _devicesManager.FindAndConnectDevicesAsync();
			Devices.Clear();
			List<DeviceViewModel> devicesVM = new List<DeviceViewModel>();

			foreach (ISwitchingDevice device in _devicesManager.Devices.GetDevices<ISwitchesList>().Values)
			{
				DeviceViewModel deviceVM = new DeviceViewModel(device);
				devicesVM.Add(deviceVM);

				if(deviceVM.ID == selectedDeviceID)
				{
					forSelect = deviceVM;
				}
			}
			Devices = devicesVM;
			OnPropertyChanged(nameof(Devices));

			if(forSelect != null)
			{
				SelectedDevice = forSelect;
			}

		}

		public RelayCommand SendId { get; set; }
		private async void ExecuteSendId(object param)
		{
			//IEnumerable<IDevice> devices = await _devicePresenter.TestDevices();
			//await _devicePresenter.TestSendIdTo(devices.First(), 1);

		}

		public RelayCommand GetId { get; set; }
		private async void ExecuteGetId(object param)
		{
			//IEnumerable<IDevice> devices = await _devicePresenter.GetConnectedDevices();
			//IDevicesManager networkWorker = _devicePresenter.DevicesManager;
			//ushort id = await networkWorker.GetId(devices.First().Ip);
		}

		#endregion Commands
	}
}

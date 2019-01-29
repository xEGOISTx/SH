using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;
using DevicesPresenterControls.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using DevicesPresenter;


namespace SHMain.Main.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private bool _devicePresenterVisibility;
		private bool _pBIsActive;

		public MainViewModel()
		{
			DataManager.DataManager dataManager = new DataManager.DataManager();
			dataManager.InitializeDatabase();
			Init();
		}


		public DevicePresenterViewModel DevicePresenterVM { get; private set; }

		public bool DevicePresenterVisibility
		{
			get { return _devicePresenterVisibility; }
			set
			{
				_devicePresenterVisibility = value;
				OnPropertyChanged(nameof(DevicePresenterVisibility));
			}
		}

		public bool PBIsActive
		{
			get { return _pBIsActive; }
			set
			{
				_pBIsActive = value;
				OnPropertyChanged(nameof(PBIsActive));
			}
		}



		private async void Init()
		{
			PBIsActive = true;
			DevicesManager devicesManager = new DevicesManager(new DeviceCommonList());

			bool loadResult = await devicesManager.LoadDevicesAsync();

			if(loadResult)
			{
				bool synchronizationResult =  await devicesManager.SynchronizationWithDevicesAsync();

				if (synchronizationResult)
				{
					DevicePresenterVM = new DevicePresenterViewModel(devicesManager);
					OnPropertyChanged(nameof(DevicePresenterVM));
					PBIsActive = false;
					DevicePresenterVisibility = true;
				}
			}
		}



		//public DevicesViewModel DevicesVM { get; private set; }
		//public RelayCommand GoToDevices { get; private set; }
		//private void ExecuteGoToDevices(object param)
		//{
		//	(Window.Current.Content as Frame).Navigate(typeof(DevicesPresenterControls.Views.DevicesView), DevicesVM);
		//}
	}
}

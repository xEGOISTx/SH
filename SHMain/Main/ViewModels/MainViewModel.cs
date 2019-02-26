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
using SHBase;


namespace SHMain.Main.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		public MainViewModel()
		{
			DataManager.DataManager dataManager = new DataManager.DataManager();
			dataManager.InitializeDatabase();
			Server server = new Server();
			Init();
		}


		public DevicePresenterViewModel DevicePresenterVM { get; private set; }

		private async void Init()
		{
			DevicesManager devicesManager = new DevicesManager(new DeviceCommonList());
			DevicePresenterVM = new DevicePresenterViewModel(devicesManager);
			OnPropertyChanged(nameof(DevicePresenterVM));

			await devicesManager.LoadDevicesAsync();
			await devicesManager.SynchronizationWithDevicesAsync();

			DevicePresenterVM.RefreshPresenter();
			DevicePresenterVM.DevicePresenterVisibility = true;
		}
	}
}


		//public DevicesViewModel DevicesVM { get; private set; }
		//public RelayCommand GoToDevices { get; private set; }
		//private void ExecuteGoToDevices(object param)
		//{
		//	(Window.Current.Content as Frame).Navigate(typeof(DevicesPresenterControls.Views.DevicesView), DevicesVM);
		//}
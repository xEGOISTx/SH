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
using Windows.Storage;

namespace SHMain.Main.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		public MainViewModel()
		{
			DataManager.DataManager dataManager = new DataManager.DataManager();
			dataManager.InitializeDatabase();
			Server server = new Server();

			StorageFolder installedLocation = ApplicationData.Current.LocalFolder;

			Init();
		}


		public DevicePresenterViewModel DevicePresenterVM { get; private set; }

		private async void Init()
		{
			DevicesManager devicesManager = new DevicesManager();
			devicesManager.AddForManagement(new Switches.SwitchList());
			devicesManager.AddForManagement(new Switches.OutletList());

			DevicePresenterVM = new DevicePresenterViewModel(devicesManager);
			OnPropertyChanged(nameof(DevicePresenterVM));

			await devicesManager.LoadDevicesAsync();
			DevicePresenterVM.Update.Execute(null);
		}
	}
}


		//public DevicesViewModel DevicesVM { get; private set; }
		//public RelayCommand GoToDevices { get; private set; }
		//private void ExecuteGoToDevices(object param)
		//{
		//	(Window.Current.Content as Frame).Navigate(typeof(DevicesPresenterControls.Views.DevicesView), DevicesVM);
		//}
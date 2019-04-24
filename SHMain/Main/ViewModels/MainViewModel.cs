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
		}


		public DevicePresenterViewModel DevicePresenterVM { get; private set; }

		public async void Init()
		{
			DataManager.DataManager dataManager = new DataManager.DataManager();
			dataManager.InitializeDatabase();

			DevicesManager devicesManager = new DevicesManager();
			devicesManager.AddForManagement(new Switches.SwitchList(dataManager));
			devicesManager.AddForManagement(new Switches.OutletList(dataManager));

			DevicePresenterVM = new DevicePresenterViewModel(devicesManager, new RouterParser.Parser());
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
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
		public MainViewModel()
		{
			DataManager.DataManager dataManager = new DataManager.DataManager();
			dataManager.InitializeDatabase();
			Init();
			//DeviceCommonList deviceCommonList = new DeviceCommonList();
			//DevicesManager devicesManager = new DevicesManager(deviceCommonList);


			//DevicesVM = new DevicesViewModel(new DevicesPresenter.DevicesManagerOld(new DevicesPresenter.DeviceCommonListOld()));

			//GoToDevices = new RelayCommand(ExecuteGoToDevices);
		}
 

		private async void Init()
		{
			DeviceCommonList deviceCommonList = new DeviceCommonList();
			DevicesManager devicesManager = new DevicesManager(deviceCommonList);

			bool loadResult = await devicesManager.LoadDevicesAsync();

			if(loadResult)
			{
				bool synchronizationResult =  await devicesManager.SynchronizationWithDevicesAsync();

				if (synchronizationResult)
				{
					//DevicesViewModel devicesVM = new DevicesViewModel();
					//(Window.Current.Content as Frame).Navigate(typeof(DevicesPresenterControls.Views.DevicesView), DevicesVM);
				}
			}
		}



		public DevicesViewModel DevicesVM { get; private set; }
		public RelayCommand GoToDevices { get; private set; }
		private void ExecuteGoToDevices(object param)
		{
			(Window.Current.Content as Frame).Navigate(typeof(DevicesPresenterControls.Views.DevicesView), DevicesVM);
		}
	}
}

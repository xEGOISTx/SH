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
using SHToolKit.DevicesManagement;
using Windows.Storage;
using SH;
using SHControls.ViewModels;
using SHBase.DevicesBaseComponents;

namespace SHMain.Main.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		public MainViewModel()
		{
			
		}

		public SHViewModel SHVM { get; private set; }

		public async void Init()
		{
			DataManager.DataManager dataManager = new DataManager.DataManager();
			dataManager.InitializeDatabase();

			List<DeviceBaseList> devices = new List<DeviceBaseList>
			{
				new Switches.SwitchList(dataManager),
				new Switches.OutletList(dataManager)
			};

			SmartHome sH = new SmartHome(devices);
			SHVM = new SHViewModel(sH);
			OnPropertyChanged(nameof(SHVM));

			await sH.Start();
			SHVM.Refresh();
		}
	}
}


		//public DevicesViewModel DevicesVM { get; private set; }
		//public RelayCommand GoToDevices { get; private set; }
		//private void ExecuteGoToDevices(object param)
		//{
		//	(Window.Current.Content as Frame).Navigate(typeof(DevicesPresenterControls.Views.DevicesView), DevicesVM);
		//}
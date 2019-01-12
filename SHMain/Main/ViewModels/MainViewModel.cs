using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;
using DevicesPresenterControls.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace SHMain.Main.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		public MainViewModel()
		{
			DataManager.DataManager dataManager = new DataManager.DataManager();
			dataManager.InitializeDatabase();
			
			DevicesVM = new DevicesViewModel(new DevicesPresenter.DevicesManager());
			GoToDevices = new RelayCommand(ExecuteGoToDevices);
		}
 
		public DevicesViewModel DevicesVM { get;private set; }


		public RelayCommand GoToDevices { get; private set; }
		private void ExecuteGoToDevices(object param)
		{
			(Window.Current.Content as Frame).Navigate(typeof(DevicesPresenterControls.Views.DevicesView), DevicesVM);
		}
	}
}

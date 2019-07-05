using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Storage;
using SH;
using SHControls.ViewModels;
using SHBase.DevicesBaseComponents;
using DataManager;
using SHToolKit;
using SHBase;

namespace SHMain.Main.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		public MainViewModel()
		{
			
		}

		public NodeViewModel NodeVM { get; private set; }

		public async void Init()
		{		
			Data.InitializeDatabase();

			List<DeviceBaseList> devices = new List<DeviceBaseList>
			{
				new Switches.SwitchList(),
				new Switches.OutletList()
			};

			Node node = new Node();
			node.AddDevices(devices);
			NodeVM = new NodeViewModel(node, Data.DataLoader, new Tools());
			OnPropertyChanged(nameof(NodeVM));

			IOperationResult res = await NodeVM.Init();
		}
	}
}


		//public DevicesViewModel DevicesVM { get; private set; }
		//public RelayCommand GoToDevices { get; private set; }
		//private void ExecuteGoToDevices(object param)
		//{
		//	(Window.Current.Content as Frame).Navigate(typeof(DevicesPresenterControls.Views.DevicesView), DevicesVM);
		//}
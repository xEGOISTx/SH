using UWPHelper;
using SH.DataRepository;

namespace SHMain.Main.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		public MainViewModel()
		{
        }

		//public NodeViewModel NodeVM { get; private set; }

		public void Init()
		{
            Repository.InitializeDatabase();

			//Data.InitializeDatabase();

			//List<DeviceBaseList> devices = new List<DeviceBaseList>
			//{
			//	new Switches.SwitchList(),
			//	new Switches.OutletList()
			//};


			//Node node = new Node(devices);
			//NodeVM = new NodeViewModel(node, Data.DataLoader);
			//OnPropertyChanged(nameof(NodeVM));

			//IOperationResult res = await NodeVM.Init();
		}
	}
}


		//public DevicesViewModel DevicesVM { get; private set; }
		//public RelayCommand GoToDevices { get; private set; }
		//private void ExecuteGoToDevices(object param)
		//{
		//	(Window.Current.Content as Frame).Navigate(typeof(DevicesPresenterControls.Views.DevicesView), DevicesVM);
		//}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.SpeechRecognition;
using Windows.Devices.Gpio;
using Windows.Devices.WiFi;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using System.Net;
using Windows.UI.Popups;
using System.Net.Sockets;
using System.Text;

using System.Net.Http;

namespace SHMain.Main.Views
{
    public sealed partial class MainPage : Page
    {
		public MainPage()
		{
			this.InitializeComponent();
			DataContext = new Main.ViewModels.MainViewModel();
		}

		private void ThisControl_Loaded(object sender, RoutedEventArgs e)
		{
			(DataContext as ViewModels.MainViewModel).Init();
		}
	}
}

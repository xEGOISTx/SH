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
using RouterParser;
using DevicesPresenter;
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

			HttpClientHandler clientHandler = new HttpClientHandler { AllowAutoRedirect = true };
			HttpClient httpClient = new HttpClient(clientHandler);
		}



		private  void Button_Click(object sender, RoutedEventArgs e)
		{

		}


		private async void Parserrrrr()
		{
			Parser parser = new Parser("http://192.168.1.254", "admin", "admin");
			parser.LoadDeviceInfosComplete += Parser_LoadDeviceInfosComplete;
			parser.LoadDeviceInfosAsync();

			//Parser parser2 = new Parser("http://192.168.1.254", "admin", "admin");
		}

		private void Parser_LoadDeviceInfosComplete(object sender, DeviceInfosEventArgs e)
		{
			
		}

		private async void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Parserrrrr();
		}

	}
}

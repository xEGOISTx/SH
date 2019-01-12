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


namespace DevicesPresenterControls.Views
{
	public sealed partial class DeviceView : Page
	{
		public DeviceView()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			DataContext = e.Parameter;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if(Frame.CanGoBack)
			{
				Frame.GoBack();
			}
		}

		private void TextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			TextBox textBlox = sender as TextBox;
			Grid item = textBlox.Parent as Grid;

			if (taskList.SelectedItem != item.DataContext)
				taskList.SelectedItem = item.DataContext;
		}
	}
}

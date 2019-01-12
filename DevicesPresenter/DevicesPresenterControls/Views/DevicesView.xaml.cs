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
    public sealed partial class DevicesView : Page
    {
        public DevicesView()
        {
            this.InitializeComponent();
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			spV.IsPaneOpen = !spV.IsPaneOpen;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if(DataContext == null)
			{
				DataContext = e.Parameter;
			}
		}
 
	}
}

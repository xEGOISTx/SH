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

namespace SHBaseControls
{
	public partial class DevicesGroup : UserControl
	{
		public DevicesGroup()
		{
			this.InitializeComponent();

		}



		public IEnumerable<object> ItemsSource
		{
			get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(DevicesGroup), new PropertyMetadata(null, ItemsSourceChaged));


		private static void ItemsSourceChaged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			DevicesGroup group = obj as DevicesGroup;
			group.devList.ItemsSource = e.NewValue as IEnumerable<object>;
		}


		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}


		public static readonly DependencyProperty ItemTemplateProperty =
			DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(DevicesGroup), new PropertyMetadata(null, ItemTemplateChanged));


		private static void ItemTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			DevicesGroup group = obj as DevicesGroup;
			group.devList.ItemTemplate = e.NewValue as DataTemplate;
		}


	}
}

using Switches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;

namespace SwitchesControls.ViewModels
{
	public class SwitchViewModel : SwitchOutletBaseViewModel
	{
		private readonly ISwitch _device;
		//private string _description;
		//private bool _editsDescription;
		//private bool _isNotBlockOnOff = true;

		public SwitchViewModel(ISwitch sw) : base(sw)
		{
			_device = sw;
			//Description = _device.Description;		
			//InitCommands();
		}

		//public bool IsNotBlockOnOff
		//{
		//	get { return _isNotBlockOnOff; }
		//	private set
		//	{
		//		_isNotBlockOnOff = value;
		//		OnPropertyChanged(nameof(IsNotBlockOnOff));
		//	}
		//}


		//public bool TurnOnOff
		//{
		//	get { return _device.State == CurrentState.TurnedOn; }
		//	set
		//	{
		//		if (IsNotBlockOnOff)
		//			OnOff(value);
		//	}
		//}


		//public string Description
		//{
		//	get { return _description; }
		//	set
		//	{
		//		_description = value;				
		//	}
		//}

		//public bool EditsDescription
		//{
		//	get { return _editsDescription; }
		//}

		//public Image Img { get { return GetImg(); } }


		//public RelayCommand EditDescription { get; private set; }
		//private void ExecuteEditDescription(object param)
		//{
		//	if(EditsDescription)
		//	{
		//		OnPropertyChanged(nameof(Description));
		//	}

		//	_editsDescription = !_editsDescription;
		//	OnPropertyChanged(nameof(EditsDescription));
		//}

		//private async void OnOff(bool value)
		//{
		//	IsNotBlockOnOff = false;
		//	if (value)
		//	{
		//		await _device.TurnOn();
		//	}
		//	else
		//	{
		//		await _device.TurnOff();
		//	}

		//	await Task.Delay(200);
		//	OnPropertyChanged(nameof(TurnOnOff));

		//	IsNotBlockOnOff = true;
		//}

		//private void InitCommands()
		//{
		//	EditDescription = new RelayCommand(ExecuteEditDescription);
		//}

		//private Image GetImg()
		//{
		//	Image image = new Image();
		//	BitmapImage img = new BitmapImage(new Uri($"{AppContext.BaseDirectory}/SwitchesControls/Resources/Imgs/NoImage.jpg"));
		//	image.Source = img;
		//	image.Stretch = Windows.UI.Xaml.Media.Stretch.Fill;
		//	return image;
		//}
	}
}

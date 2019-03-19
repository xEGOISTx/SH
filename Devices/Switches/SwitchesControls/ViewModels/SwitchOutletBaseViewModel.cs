using Switches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace SwitchesControls.ViewModels
{
	public abstract class SwitchOutletBaseViewModel : BaseViewModel
	{
		private readonly IBaseSwitch _device;
		private string _description;
		private bool _editsDescription;
		private bool _isNotBlockOnOff = true;
		private readonly ISwitchEditor _editor;

		public SwitchOutletBaseViewModel(IBaseSwitch baseSwitch, ISwitchEditor switchEditor)
		{
			_device = baseSwitch;
			_editor = switchEditor;

			Description = _device.Description;
			InitCommands();
		}

		public bool IsNotBlockOnOff
		{
			get { return _isNotBlockOnOff; }
			private set
			{
				_isNotBlockOnOff = value;
				OnPropertyChanged(nameof(IsNotBlockOnOff));
			}
		}

		public bool IsConnected { get { return _device.IsConnected; } }



		public bool TurnOnOff
		{
			get { return _device.State == CurrentState.TurnedOn; }
			set
			{
				if (IsNotBlockOnOff)
					OnOff(value);
			}
		}


		public string Description
		{
			get { return _device.Description; }
			set
			{
				_description = value;
			}
		}

		public bool EditsDescription
		{
			get { return _editsDescription; }
		}

		public Image Img { get { return GetImg(); } }

		public RelayCommand EditDescription { get; private set; }
		private void ExecuteEditDescription(object param)
		{
			if (EditsDescription)
			{
				_editor.Rename(_device, _description);				
			}

			OnPropertyChanged(nameof(Description));
			_editsDescription = !_editsDescription;
			OnPropertyChanged(nameof(EditsDescription));
		}


		public virtual void RefreshState()
		{
			OnPropertyChanged(nameof(IsConnected));
			OnPropertyChanged(nameof(TurnOnOff));
		}

		private async void OnOff(bool value)
		{
			IsNotBlockOnOff = false;
			if (value)
			{
				await _device.TurnOn();
			}
			else
			{
				await _device.TurnOff();
			}

			//задержка, чтобы успела отработать анимация 
			await Task.Delay(200);
			OnPropertyChanged(nameof(TurnOnOff));

			IsNotBlockOnOff = true;
		}

		private void InitCommands()
		{
			EditDescription = new RelayCommand(ExecuteEditDescription);
		}

		protected Image GetImg()
		{
			Image image = new Image();
			BitmapImage img = new BitmapImage(new Uri($"{AppContext.BaseDirectory}/SwitchesControls/Resources/Imgs/NoImage.jpg"));
			image.Source = img;
			image.Stretch = Windows.UI.Xaml.Media.Stretch.Fill;
			return image;
		}
	}
}

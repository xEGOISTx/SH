using Switches.SwitchesOutlets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;

namespace SwitchesControls.ViewModels
{
	public class SwitchOutletViewModel : BaseViewModel
	{
		private readonly ISwitchOutlet _device;
		private string _description;
		private bool _editsDescription;

		public SwitchOutletViewModel(ISwitchOutlet switchOutlet)
		{
			_device = switchOutlet;
			Description = _device.Description;
			InitCommands();
		}

		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;				
			}
		}

		public bool EditsDescription
		{
			get { return _editsDescription; }
		}


		public RelayCommand TurnOnOff { get; private set; }
		private void ExecuteTurnOnOff(object param)
		{
			if(_device.State == CurrentState.TurnedOff)
			{
				_device.TurnOn();
			}
			else
			{
				_device.TurnOff();
			}
		}

		public RelayCommand EditDescription { get; private set; }
		private void ExecuteEditDescription(object param)
		{
			if(EditsDescription)
			{
				OnPropertyChanged(nameof(Description));
			}

			_editsDescription = !_editsDescription;
			OnPropertyChanged(nameof(EditsDescription));
		}

		private void InitCommands()
		{
			TurnOnOff = new RelayCommand(ExecuteTurnOnOff);
			EditDescription = new RelayCommand(ExecuteEditDescription);
		}
	}
}

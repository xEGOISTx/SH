using DevicesPresenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;
using SHBase.DeviceBase;

namespace DevicesPresenterControls.ViewModels
{
    public class ActionGPIOViewModel : BaseViewModel
    {
		private readonly IActionGPIO _action;
		private GPIOMode _mode;
		private GPIOLevel _level;

		public ActionGPIOViewModel(IActionGPIO action)
		{
			_action = action;
			_mode = action.Mode;
			_level = action.Level;

			Levels = new List<GPIOLevel>
			{
				GPIOLevel.High,
				GPIOLevel.Low,
				GPIOLevel.NotDefined
			};

			Modes = new List<GPIOMode>
			{
				GPIOMode.Input,
				GPIOMode.Output,
				GPIOMode.NotDefined
			};
		}

		public IEnumerable<GPIOLevel> Levels { get; }

		public IEnumerable<GPIOMode> Modes { get; }

		public byte PinNumber => _action.PinNumber;

		//public bool IsPresentsAction => _action.IsPresentsAction;

		public bool TaskIsSelected { get; set; }

		public GPIOMode Mode
		{
			get { return _mode; }
			set
			{
				if(_mode != value && TaskIsSelected)
				{
					_mode = value;
					OnPropertyChanged(nameof(Mode));
				}
			}
		}

		public GPIOLevel Level
		{
			get { return _level; }
			set
			{
				if(_level != value && TaskIsSelected)
				{
					_level = value;
					OnPropertyChanged(nameof(Level));
				}
			}
		}

		public bool IsChanged
		{
			get
			{
				return _mode != _action.Mode || _level != _action.Level;
			}
		}


		public void ApplyChanges()
		{
			_action.ChangeAction(_mode, _level);
			//_action.Mode = _mode;
			//_action.Level = _level;
		}

		//public void Refresh()
		//{
		//	_mode = _action.Mode;
		//	_lavel = _action.Lavel;
		//}

    }
}

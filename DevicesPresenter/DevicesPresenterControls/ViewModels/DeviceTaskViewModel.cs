using DevicesPresenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;

namespace DevicesPresenterControls.ViewModels
{
	public class DeviceTaskViewModel : BaseViewModel
	{
		private string _description;
		private string _voiceCommand;
		private readonly List<ActionGPIOViewModel> _actions = new List<ActionGPIOViewModel>();
		private IDeviceTask _task;
		private bool _isSelected;

		public DeviceTaskViewModel(IDeviceTask task)
		{
			_task = task;
			_description = task.Description;
			_voiceCommand = task.VoiceCommand;
			FillActions();
			
			Execute = new RelayCommand(ExecuteTask);
		}

		public IEnumerable<ActionGPIOViewModel> Actions => _actions;

		public int ID => _task.ID;

		public bool IsNew => _task.IsNew;

		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				_isSelected = value;
				foreach(ActionGPIOViewModel action in Actions)
				{
					action.TaskIsSelected = value;
				}
			}
		}

		public bool IsChanged
		{
			get
			{
				return _description != _task.Description || _voiceCommand != _task.VoiceCommand || _actions.Any(a => a.IsChanged);
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				if(_description != value)
				{
					_description = value;
					OnPropertyChanged(nameof(Description));
				}
			}
		}

		public string VoiceCommand
		{
			get { return _voiceCommand; }
			set
			{
				if(_voiceCommand != value)
				{
					_voiceCommand = value;
					OnPropertyChanged(nameof(VoiceCommand));
				}
			}
		}

		public RelayCommand Execute { get; private set; }
		private void ExecuteTask(object param)
		{
				_task.Execute();
		}

		public void ApplyChanges()
		{
			_task.Description = _description;
			_task.VoiceCommand = _voiceCommand;

			foreach(ActionGPIOViewModel actionVM in _actions)
			{
				if(actionVM.IsChanged)
				{
					actionVM.ApplyChanges();
				}
			}
		}

		private void FillActions()
		{
			foreach(IActionGPIO action in _task.Actions)
			{
				ActionGPIOViewModel actionVM = new ActionGPIOViewModel(action);
				_actions.Add(actionVM);
			}
		}

	}
}

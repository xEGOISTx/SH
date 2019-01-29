using DevicesPresenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using SC = DevicesPresenterControls.Resources.StringConsts;

namespace DevicesPresenterControls.ViewModels
{
	public class DevicePresenterViewModelOld : BaseViewModel
	{
		private readonly IDeviceEditor _deviceEditor;
		private DeviceViewModel _originalDeviceVM;
		private DeviceViewModel _currentDeviceVM;
		private ISwitchingDevice _deviceCopy;
		private bool _isEditing;
		private DeviceTaskViewModel _selectedTask;

		public DevicePresenterViewModelOld(DeviceViewModel device, IDeviceEditor deviceEditor)
		{			
			_deviceEditor = deviceEditor;
			_currentDeviceVM = device;
			Description = device.Description;
			Edit = new RelayCommand(ExecuteEdit);
			AddTask = new RelayCommand(ExecuteAddTask);
			RemoveTask = new RelayCommand(ExecuteRemoveTask);

			RefreshTaskList();
		}


		//public SymbolIcon Icon
		//{
		//	get
		//	{
		//		if(IsEditing)
		//		{
		//			return new SymbolIcon(Symbol.Accept);
		//		}
		//		else
		//		{
		//			return new SymbolIcon(Symbol.Edit);
		//		}
		//	}
		//}

		public ObservableCollection<DeviceTaskViewModel> Tasks => _currentDeviceVM.Tasks;

		public IEnumerable<ActionGPIOViewModel> Actions { get; private set; }

		public DeviceTaskViewModel SelectedTask
		{
			get { return _selectedTask; }
			set
			{
				if (_selectedTask != null)
					_selectedTask.IsSelected = false;

				_selectedTask = value;

				if (_selectedTask != null)
					_selectedTask.IsSelected = true;
				OnPropertyChanged(nameof(SelectedTask));
			}
		}

		public string IsConnected
		{
			get { return _currentDeviceVM != null ? _currentDeviceVM.IsConnected.ToString() : string.Empty; }
		}


		public bool IsEditing
		{
			get { return _isEditing; }
			set
			{
				_isEditing = value;
				OnPropertyChanged(nameof(IsEditing));				
			}
		}

		public string Description
		{
			get { return _currentDeviceVM.Description; }
			set
			{
				_currentDeviceVM.Description = value;
				OnPropertyChanged(nameof(Description));
			}
		}

		#region Commands
		public RelayCommand AddTask { get; private set; }
		private void ExecuteAddTask(object param)
		{
			if (_deviceCopy != null && _isEditing)
			{
				IDeviceTask newTask = _deviceEditor.CreateNewTaskFor(_deviceCopy);
				_currentDeviceVM.Tasks.Add(new DeviceTaskViewModel(newTask));
			}
		}

		public RelayCommand Edit { get; private set; }
		private void ExecuteEdit(object param)
		{
			if (_currentDeviceVM != null)
			{
				if (IsEditing)
				{
					if (_currentDeviceVM.Tasks.Any(t => t.IsChanged) || _currentDeviceVM.Description != _originalDeviceVM.Description)
					{
						ShowMessageDialog();
					}		
					else
					{
						_currentDeviceVM = _originalDeviceVM;
						OnPropertyChanged(nameof(Description));
						RefreshTaskList();

						_deviceCopy = null;
						IsEditing = false;
					}
				}
				else
				{
					PreparingDeviceCopy();
					IsEditing = true;
				}
			}
		}

		public RelayCommand RemoveTask { get; private set; }
		private void ExecuteRemoveTask(object param)
		{
			_deviceEditor.MarkTaskForDelete(_deviceCopy, _selectedTask.ID);
			_currentDeviceVM.Tasks.Remove(_selectedTask);
		}
		#endregion Commands

		private async void  ShowMessageDialog()
		{
			MessageDialog messageDialog = new MessageDialog(SC.SaveChanges);
			messageDialog.Commands.Add(new UICommand(SC.Yes, new UICommandInvokedHandler(CommandInvokedHandler)));
			messageDialog.Commands.Add(new UICommand(SC.No, new UICommandInvokedHandler(CommandInvokedHandler)));
			messageDialog.Commands.Add(new UICommand(SC.Cancel, new UICommandInvokedHandler(CommandInvokedHandler)));
			await messageDialog.ShowAsync();
		}

		private void CommandInvokedHandler(IUICommand command)
		{
			if(command.Label == SC.Yes)
			{
				_currentDeviceVM.ApplyChanges();
				_deviceEditor.ApplyAndSaveChanges(_deviceCopy);
				_originalDeviceVM.FullRefresh();
				_currentDeviceVM = _originalDeviceVM;
				OnPropertyChanged(nameof(Description));
				RefreshTaskList();

				_deviceCopy = null;
				IsEditing = false;
			}

			if (command.Label == SC.No)
			{
				_deviceCopy = null;
				_currentDeviceVM = _originalDeviceVM;
				OnPropertyChanged(nameof(Description));
				RefreshTaskList();

				IsEditing = false;
			}
		}

		private void RefreshTaskList()
		{
			int taskIDForSelect = int.MinValue;
			if (SelectedTask != null)
			{
				taskIDForSelect = SelectedTask.ID;
			}

			OnPropertyChanged(nameof(Tasks));

			if (taskIDForSelect > int.MinValue)
			{
				DeviceTaskViewModel task = Tasks.Where(t => t.ID == taskIDForSelect).FirstOrDefault();
				SelectedTask = task;
			}
		}


		private void PreparingDeviceCopy()
		{
			_originalDeviceVM = _currentDeviceVM;
			_deviceCopy = _deviceEditor.GetDeviceCopy(_originalDeviceVM.ID);
			_currentDeviceVM = new DeviceViewModel(_deviceCopy);
			RefreshTaskList();
		}

	}
}

using DevicesPresenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;

namespace DevicesPresenterControls.ViewModels
{
	public class DeviceViewModel : BaseViewModel
	{
		private readonly IDevice _device;
		private string _description;

		public DeviceViewModel(IDevice device)
		{
			_device = device;
			Description = _device.Description;
			RefreshTasks();			
		}

		public ushort ID => _device.ID;
		public string Name => _device.Name;
		public string FirmwareType => _device.FirmwareType.ToString();
		public string Mac => _device.Mac.ToString();
		public string IP => _device.IP.ToString();
		public bool IsConnected => _device.IsConnected;

		public string Description
		{
			get { return _description; }
			set
			{
				if (_description != value)
				{
					_description = value;
					OnPropertyChanged(nameof(Description));
				}
			}
		}

		public ObservableCollection<DeviceTaskViewModel> Tasks { get; } = new ObservableCollection<DeviceTaskViewModel>();

		public void FullRefresh()
		{
			RefreshProperties();
			RefreshTasks();
		}

		public void RefreshProperties()
		{
			Description = _device.Description;
			OnPropertyChanged(nameof(IsConnected));
		}

		public void RefreshConnectionStatus()
		{
			OnPropertyChanged(nameof(IsConnected));
		}

		public void RefreshTasks()
		{
			Tasks.Clear();
			foreach (IDeviceTask task in _device.Tasks)
			{
				if (!task.IsDelete)
				{
					DeviceTaskViewModel taskVM = new DeviceTaskViewModel(task);
					Tasks.Add(taskVM);
				}
			}
		}

		public void ApplyChanges()
		{
			_device.Description = _description;

			foreach(DeviceTaskViewModel task in Tasks)
			{
				task.ApplyChanges();
			}
		}

	}
}

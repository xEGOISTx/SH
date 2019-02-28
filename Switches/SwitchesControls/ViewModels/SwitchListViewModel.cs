using System.Collections.Generic;
using System.Collections.ObjectModel;
using Switches;
using UWPHelper;

namespace SwitchesControls.ViewModels
{
	public class SwitchListViewModel : BaseViewModel
	{
		private readonly ISwitchList _devices;
		private readonly ISwitchEditor _editor;

		public SwitchListViewModel(ISwitchList switchList)
		{
			_devices = switchList;
			_editor = switchList.SwitchEditor;
			FullRefresh();
		}



		public IEnumerable<SwitchViewModel> List { get; private set; }

		public void FullRefresh()
		{
			List<SwitchViewModel> devs = new List<SwitchViewModel>();
			foreach (ISwitch device in _devices)
			{
				SwitchViewModel deviceVM = new SwitchViewModel(device, _editor);
				devs.Add(deviceVM);
			}

			List = devs;
			OnPropertyChanged(nameof(List));
		}

		public void RefreshState()
		{
			foreach (SwitchViewModel device in List)
			{
				device.RefreshState();
			}
		}

	}
}

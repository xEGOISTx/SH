using SHToolKit.DevicesManagement;
using Switches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;

namespace SwitchesControls.ViewModels
{
	public class OutletListViewModel : BaseViewModel
	{
		private readonly IOutletList _devices;
		private readonly ISwitchEditor _editor;

		public OutletListViewModel(IDevicesGetter devicesGetter)
		{
			_devices = (IOutletList)devicesGetter.GetDevices<IOutletList>();
			_editor = _devices.SwitchEditor;
			FullRefresh();
		}

		public IEnumerable<OutletViewModel> List { get; private set; }

		public void FullRefresh()
		{
			List<OutletViewModel> devs = new List<OutletViewModel>();
			foreach (IOutlet device in _devices)
			{
				OutletViewModel deviceVM = new OutletViewModel(device, _editor);
				devs.Add(deviceVM);
			}

			List = devs;
			OnPropertyChanged(nameof(List));
		}

		public void RefreshState()
		{
			foreach (OutletViewModel device in List)
			{
				device.RefreshState();
			}
		}
	}
}

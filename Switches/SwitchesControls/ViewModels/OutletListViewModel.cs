﻿using Switches;
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

		public OutletListViewModel(IOutletList outletList)
		{
			_devices = outletList;
			Refresh();
		}



		public IEnumerable<OutletViewModel> List { get; private set; }

		public void Refresh()
		{
			List<OutletViewModel> devs = new List<OutletViewModel>();
			foreach (IOutlet device in _devices)
			{
				OutletViewModel deviceVM = new OutletViewModel(device);
				devs.Add(deviceVM);
			}

			List = devs;
			OnPropertyChanged(nameof(List));
		}
	}
}

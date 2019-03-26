﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Switches;
using SHBase.DevicesBaseComponents;

namespace SwitchesControls.ViewModels
{
	public class SwitchesViewModel
	{
		public SwitchesViewModel(IDevicesGetter devicesGetter)
		{
			Switches = new SwitchListViewModel(devicesGetter);
			Outlets = new OutletListViewModel(devicesGetter);
		}

		public SwitchListViewModel Switches { get; }

		public OutletListViewModel Outlets { get; }

		public int CommonCount => Switches.List.Count() + Outlets.List.Count();

		public void FullRefresh()
		{
			Switches.FullRefresh();
			Outlets.FullRefresh();
		}

		public void RefreshState()
		{
			Switches.RefreshState();
			Outlets.RefreshState();
		}
	}
}
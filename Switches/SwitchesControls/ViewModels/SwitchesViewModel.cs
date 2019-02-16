﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Switches;

namespace SwitchesControls.ViewModels
{
	public class SwitchesViewModel
	{
		private readonly ISwitches _switches;

		public SwitchesViewModel(ISwitches switches)
		{
			_switches = switches;

			Switches = new SwitchListViewModel(_switches.SwitchList);
			Outlets = new OutletListViewModel(_switches.OutletList);
		}

		public SwitchListViewModel Switches { get; }

		public OutletListViewModel Outlets { get; }

		public void Refresh()
		{
			Switches.Refresh();
			Outlets.Refresh();
		}
	}
}

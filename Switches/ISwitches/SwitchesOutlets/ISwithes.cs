﻿using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switches.SwitchesOutlets
{
	public interface ISwithes
	{
		ISwitchesAndOutletsList SwitchesAndOutlets { get; }
	}
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DeviceBase
{
	public interface IGPIOActionTask<ActionType> : IDeviceBaseTask
		where ActionType: IGPIOAction
	{
		IEnumerable<ActionType> Actions { get; }
	}
}

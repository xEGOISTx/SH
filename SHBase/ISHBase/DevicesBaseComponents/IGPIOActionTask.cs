using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public interface IGPIOActionTask<ActionType> : IDeviceBaseTask
		where ActionType: IBaseGPIOAction
	{
		IEnumerable<ActionType> Actions { get; }
	}
}

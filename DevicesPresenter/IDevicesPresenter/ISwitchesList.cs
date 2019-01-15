using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesPresenter
{
	public interface ISwitchesList : IDeviceBaseList , IReadOnlyDictionary<ushort, ISwitchingDevice>
	{
	}
}

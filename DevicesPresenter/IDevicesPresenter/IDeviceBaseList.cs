using SHBase.DevicesBaseComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesPresenter
{
	public interface IDeviceBaseList<DeviceType>  : IReadOnlyDictionary<int, DeviceType>
		where DeviceType : IDeviceBase
	{

	}
}

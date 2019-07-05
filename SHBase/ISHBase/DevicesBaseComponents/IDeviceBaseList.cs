using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public interface IDeviceBaseList<out DeviceType> : BaseContainers.IContainer<int, DeviceType>
		where DeviceType : IDeviceBase
	{
		int DevicesType { get; }
	}
}

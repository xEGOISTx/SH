using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public interface IDeviceBaseList<DeviceType> : BaseContainers.IContainer<int,DeviceType>
		where DeviceType : IDeviceBase
	{
		DevicesBaseComponents.DeviceType DevicesType { get; }
	}
}

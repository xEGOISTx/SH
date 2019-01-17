using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesPresenter
{
	public interface IDeviceCommonList
	{
		DeviceListType GetDevices<DeviceListType>();
	}
}

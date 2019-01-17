using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace DevicesPresenter
{
	public class DeviceCommonList : IDeviceCommonList
	{
		private readonly List<object> _devices = new List<object>();

		public DeviceCommonList()
		{
			Add(new SwitchesList());

		}

		public DeviceListType GetDevices<DeviceListType>()
		{
			return (DeviceListType)_devices.Where(d => d is DeviceListType).FirstOrDefault();
		}

		public void Add<T>(IDeviceBaseList<T> deviceList )
			where T : IDeviceBase
		{
			_devices.Add(deviceList);
		}
	}
}

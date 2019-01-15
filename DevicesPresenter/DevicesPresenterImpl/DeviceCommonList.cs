using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DeviceBase;

namespace DevicesPresenter
{
	public class DeviceCommonList : IDeviceCommonList
	{
		private readonly List<IDeviceBaseList> _devices = new List<IDeviceBaseList>();

		public DeviceCommonList()
		{
			_devices.Add(new SwitchesList());
		}

		public DeviceListType GetDevices<DeviceListType>()
		{
			return (DeviceListType)_devices.Where(d => d is DeviceListType).FirstOrDefault();
		}

		public Task<bool> Load()
		{
			throw new NotImplementedException();
		}
	}
}

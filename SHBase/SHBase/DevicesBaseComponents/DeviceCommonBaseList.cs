using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	/// <summary>
	/// Хранит списки устройств
	/// </summary>
	public abstract class DeviceCommonBaseList : IEnumerable
	{
		private readonly List<Devices> _list = new List<Devices>();

		protected IEnumerable<Devices> List => _list;

		public int Count => _list.Count;

		public DeviceListType GetDeviceList<DeviceListType>()
			where DeviceListType : Devices
		{
			return (DeviceListType)_list.Where(dL => dL is DeviceListType).FirstOrDefault();
		}

		public bool IsPresent<DeviceListType>()
		{
			Devices devList = _list.Where(dL => dL is DeviceListType).FirstOrDefault();
			return devList != null;
		}

		protected void Add<DeviceListType>(DeviceListType deviceList)
			where DeviceListType : Devices
		{
			if(!IsPresent<DeviceListType>())
			{
				_list.Add(deviceList);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return _list.GetEnumerator();
		}
	}
}

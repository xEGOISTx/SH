using SHBase.DevicesBaseComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesPresenter
{
	public abstract class DeviceBaseList<DeviceType> : IDeviceBaseList<DeviceType>
		where DeviceType : IDeviceBase
	{
		protected readonly Dictionary<int, DeviceType> _devices = new Dictionary<int, DeviceType>();

		public DeviceType this[int id] => _devices[id];

		public IEnumerable<int> Keys => _devices.Keys;

		public IEnumerable<DeviceType> Values => _devices.Values;

		public int Count => _devices.Count;

		public bool ContainsKey(int id)
		{
			return _devices.ContainsKey(id);
		}

		IEnumerator<KeyValuePair<int, DeviceType>> IEnumerable<KeyValuePair<int, DeviceType>>.GetEnumerator()
		{
			return _devices.GetEnumerator();
		}

		public bool TryGetValue(int id, out DeviceType value)
		{
			return _devices.TryGetValue(id, out value);
		}

		public IEnumerator GetEnumerator()
		{
			return _devices.Values.GetEnumerator();
		}

		public void Add(DeviceType device)
		{
			if (!ContainsKey(device.ID))
			{
				_devices.Add(device.ID, device);
			}
		}

	}
}

using SHBase.DevicesBaseComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Switches
{
	public abstract class SwitchesAndOutletsBaseList<TValue>
		where TValue : IBaseSwitch
	{
		protected readonly Dictionary<int, TValue> _devices = new Dictionary<int, TValue>();

		public SwitchesAndOutletsBaseList(DeviceType devicesType)
		{
			DevicesType = devicesType;
		}


		public DeviceType DevicesType { get;}

		public int Count => _devices.Count;


		public bool ContainsKey(int key)
		{
			return _devices.ContainsKey(key);
		}

		public TValue GetByKey(int key)
		{
			if (ContainsKey(key))
			{
				return _devices[key];
			}

			return default(TValue);
		}

		public IEnumerator GetEnumerator()
		{
			return _devices.Values.GetEnumerator();
		}

		public void Add(TValue device)
		{
			if (!ContainsKey(device.ID))
			{
				_devices.Add(device.ID, device);
			}
		}

		public void AddRange(IEnumerable<TValue> devices)
		{
			foreach (TValue device in devices.ToArray())
			{
				Add(device);
			}
		}

		internal abstract ISwitchesLoader GetLoader();

		internal abstract DBConvertor<TValue> Convertor { get; }
	}
}

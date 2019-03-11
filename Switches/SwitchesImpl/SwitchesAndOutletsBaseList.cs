using SHBase.DevicesBaseComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Switches
{
	public abstract class SwitchesAndOutletsBaseList<TValue> : LoadableList
		where TValue : IBaseSwitch
	{
		public SwitchesAndOutletsBaseList(int devicesType) : base(devicesType)
		{
		}

	
		public new TValue GetByKey(int key)
		{
			if (ContainsKey(key))
			{
				return (TValue)_devices[key];
			}

			return default(TValue);
		}

		public new IEnumerator GetEnumerator()
		{
			return _devices.Values.Cast<TValue>().GetEnumerator();
		}

		public void Add(TValue device)
		{
			base.Add(device);
		}

		public void AddRange(IEnumerable<TValue> devices)
		{
			foreach (TValue device in devices.ToArray())
			{
				Add(device);
			}
		}

		public ISwitchEditor SwitchEditor { get; } = new SwitchEditor();

	}
}

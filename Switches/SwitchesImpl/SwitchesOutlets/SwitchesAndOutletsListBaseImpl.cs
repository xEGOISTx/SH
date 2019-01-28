using SHBase.DevicesBaseComponents;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Switches.SwitchesOutlets
{
	public abstract class SwitchesAndOutletsListBaseImpl : ISwitchesAndOutletsList
	{
		protected readonly Dictionary<int, ISwitchOutlet> _devices = new Dictionary<int, ISwitchOutlet>();

		public SwitchesAndOutletsListBaseImpl(DeviceType devicesType)
		{
			DevicesType = devicesType;
		}


		public DeviceType DevicesType { get;}

		public int Count => _devices.Count;


		public bool ContainsKey(int key)
		{
			return _devices.ContainsKey(key);
		}

		public ISwitchOutlet GetByKey(int key)
		{
			if (ContainsKey(key))
			{
				return _devices[key];
			}

			return null;
		}

		public IEnumerator GetEnumerator()
		{
			return _devices.Values.GetEnumerator();
		}

		public void Add(ISwitchOutlet device)
		{
			if (!ContainsKey(device.ID))
			{
				_devices.Add(device.ID, device);
			}
		}

		public void AddRange(IEnumerable<ISwitchOutlet> devices)
		{
			foreach (ISwitchOutlet device in devices.ToArray())
			{
				Add(device);
			}
		}

		public abstract ISwitchesLoader GetLoader();

	}
}

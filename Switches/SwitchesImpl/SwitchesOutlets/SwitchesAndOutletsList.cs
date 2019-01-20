using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace Switches.SwitchesOutlets
{
	public class SwitchesAndOutletsList : ISwitchesAndOutletsList
	{
		private readonly Dictionary<int, ISwitchOutlet> _devices = new Dictionary<int, ISwitchOutlet>();

		public int Count => _devices.Count;

		public bool ContainsKey(int key)
		{
			return _devices.ContainsKey(key);
		}

		public ISwitchOutlet GetByKey(int key)
		{
			if(ContainsKey(key))
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
			if(!ContainsKey(device.ID))
			{
				_devices.Add(device.ID, device);
			}
		}

		public void AddRange(IEnumerable<ISwitchOutlet> devices)
		{
			foreach(ISwitchOutlet device in devices.ToArray())
			{
				Add(device);
			}
		}

		public ISwitchesAndOutletsLoader GetLoader()
		{
			return new SwitchesAndOutletsLoader();
		}
	}
}

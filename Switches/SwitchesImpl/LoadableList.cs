using SHBase.BaseContainers;
using SHBase.DevicesBaseComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switches
{
	public abstract class LoadableList : IContainer<int, IBaseSwitch>
	{
		protected readonly Dictionary<int, IBaseSwitch> _devices = new Dictionary<int, IBaseSwitch>();


		public LoadableList(DeviceType deviceType)
		{
			DevicesType = deviceType;
		}


		internal abstract ISwitchesLoader Loader { get; }

		internal abstract DBConvertor Convertor { get; }

		public int Count => _devices.Count;

		public DeviceType DevicesType { get;}


		public void Add(IBaseSwitch device)
		{
			if (!ContainsKey(device.ID) && device.DeviceType == DevicesType)
			{
				_devices.Add(device.ID, device);
			}
		}

		public void AddRange(IEnumerable<IBaseSwitch> devices)
		{
			foreach (IBaseSwitch device in devices.ToArray())
			{
				Add(device);
			}
		}

		public IBaseSwitch GetByKey(int key)
		{
			if(ContainsKey(key))
			{
				return _devices[key];
			}

			return null;
		}

		public bool ContainsKey(int key)
		{
			return _devices.ContainsKey(key);
		}

		public IEnumerator GetEnumerator()
		{
			return _devices.Values.GetEnumerator();
		}
	}
}

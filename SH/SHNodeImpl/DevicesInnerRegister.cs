using SH.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using SH.Core.DevicesComponents;
using System.Collections;

namespace SH.Node
{
	internal static class DevicesInnerRegister
	{
		private static readonly SortedDictionary<int, IDevice> _devices = new SortedDictionary<int, IDevice>();

        public static IEnumerable<IDevice> List => _devices.Values;

		public static void Add(IDevice device)
		{
			if(device.ID > 0 && !_devices.ContainsKey(device.ID))
			{
				_devices.Add(device.ID, device);
			}
		}

		public static void AddRange(IEnumerable<IDevice> devices)
		{
			foreach(IDevice device in devices)
			{
				Add(device);
			}
		}

		public static void Remove(IDevice device)
		{
			if(_devices.ContainsKey(device.ID))
			{
				_devices.Remove(device.ID);
			}
		}

        public static bool IsPresent(int id)
        {
            return _devices.ContainsKey(id);
        }

        public static IDevice GetByID(int id)
        {
            if(_devices.ContainsKey(id))
            {
                return _devices[id];
            }

            return null;
        }

		public static int GetFreeID()
		{
			if(!_devices.Any())
			{
				return 1;
			}
			else
			{
				return _devices.Last().Key + 1;
			}		
		}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace SH
{
	public class Node : INode
	{
		private List<DeviceBaseList> _devices = new List<DeviceBaseList>();
		//private bool _isInit;

		public Node(IEnumerable<DeviceBaseList> devices)
		{
			foreach (DeviceBaseList devsList in devices)
			{
				(DevicesManager as DevicesManager).AddForManagement(devsList);
				_devices.Add(devsList);
			}
		}

		public IDevicesManager DevicesManager { get;} = new DevicesManager();

	}
}

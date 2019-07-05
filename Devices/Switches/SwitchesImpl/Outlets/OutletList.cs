using SHBase;
using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Switches
{
	public class OutletList : SwitchesBaseList, IOutletList
	{
		//private readonly IDataManager _data;

		public OutletList() : base(DeviceTypes.OUTLET)
		{
			//_data = dataManager;
			//SwitchEditor = new SwitchEditor(_data);
		}

		public ISwitchEditor SwitchEditor { get; }

		public override IDeviceBase CreateDevice(int id, string name, string description, IPAddress iP, FirmwareType firmwareType, MacAddress mac)
		{
			return new Outlet(iP, mac, firmwareType)
			{
				Name = name,
				ID = id,
				Description = name
			};
		}

		public IOutlet GetByKey(int key)
		{
			return _devices.ContainsKey(key) ? (IOutlet)_devices[key] : null;
		}
	}
}

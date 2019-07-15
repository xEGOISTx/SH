using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SHBase;
using SHBase.DevicesBaseComponents;

namespace Switches
{
	public class SwitchList : SwitchesBaseList<Switch> , ISwitchList
	{
		//private readonly IDataManager _data;

		public SwitchList() : base(DeviceTypes.SWITCH)
		{
			//_data = dataManager;
			//SwitchEditor = new SwitchEditor(_data);
		}

		public ISwitchEditor SwitchEditor { get; }

		public override IDeviceBase CreateDevice(int id, string name, string description, IPAddress iP, FirmwareType firmwareType, MacAddress mac)
		{
			return new Switch(iP, mac, firmwareType)
			{
				Name = name,
				ID = id,
				Description = description
			};
		}

		public ISwitch GetByKey(int key)
		{
			return _devices.ContainsKey(key) ? (ISwitch)_devices[key] : null;
		}
	}
}

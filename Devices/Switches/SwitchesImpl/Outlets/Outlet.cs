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
	public class Outlet : BaseSwitch, IOutlet
	{
		private SwitchOutletTaskList _tasks;

		public Outlet(IPAddress ip, MacAddress mac, FirmwareType firmwareType) : base(DeviceTypes.OUTLET)
		{
			Mac = mac;
			FirmwareType = firmwareType;

			_tasks = new SwitchOutletTaskList(this);
		}

		internal Outlet(MacAddress mac, FirmwareType firmwareType) :
			this(SHToolKit.Consts.ZERO_IP, mac, firmwareType)
		{ }


		public ISwitchOutletTaskList Tasks => _tasks;


		/// <summary>
		/// Включить
		/// </summary>
		public async Task<bool> TurnOn()
		{
			ISwitchOutletTask turnOn = _tasks.GetByKey(SwitchOutletTaskType.TurnOn);
			return await TurnOn(turnOn);
		}

		/// <summary>
		/// Выключить
		/// </summary>
		public async Task<bool> TurnOff()
		{
			ISwitchOutletTask turnOff = _tasks.GetByKey(SwitchOutletTaskType.TurnOff);
			return await TurnOff(turnOff);
		}
	}
}

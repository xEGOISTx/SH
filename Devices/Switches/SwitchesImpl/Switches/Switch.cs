using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SHBase;
using SHBase.DevicesBaseComponents;

namespace Switches
{
	public class Switch : BaseSwitch, ISwitch
	{
		private SwitchOutletTaskList _tasks;

		public Switch(IPAddress ip, MacAddress mac, FirmwareType firmwareType)
		{
			IP = ip;
			Mac = mac;
			FirmwareType = firmwareType;
			DeviceType = DeviceTypes.SWITCH;

			_tasks = new SwitchOutletTaskList(this);
		}

		internal Switch(MacAddress mac, FirmwareType firmwareType) :
			this(Consts.ZERO_IP, mac, firmwareType)
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

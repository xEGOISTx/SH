using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SHBase;
using SHBase.DevicesBaseComponents;

namespace Switches.SwitchesOutlets
{
	public class SwitchOutlet : DeviceBase, ISwitchOutlet
	{
		private SwitchOutletTaskList _tasks;

		public SwitchOutlet(IPAddress ip, MacAddress mac, FirmwareType firmwareType, DeviceType deviceType) : base(ip)
		{
			Mac = mac;
			FirmwareType = firmwareType;
			DeviceType = deviceType;

			_tasks = new SwitchOutletTaskList(this);
		}

		public SwitchOutlet(MacAddress mac, FirmwareType firmwareType, DeviceType deviceType) :
			this(Consts.ZERO_IP, mac, firmwareType, deviceType) { }


		public CurrentState State { get; set; }

		public ISwitchOutletTaskList Tasks => _tasks;


		/// <summary>
		/// Включить
		/// </summary>
		public async Task<bool> TurnOn()
		{
			ISwitchOutletTask turnOn = _tasks.GetByKey(TaskType.TurnOn);
			bool res = await turnOn?.Execute();

			if (res)
			{
				State = CurrentState.TurnedOn;
			}

			return res;

		}

		/// <summary>
		/// Выключить
		/// </summary>
		public async Task<bool> TurnOff()
		{
			ISwitchOutletTask turnOff = _tasks.GetByKey(TaskType.TurnOff);
			bool res = await turnOff?.Execute();

			if (res)
			{
				State = CurrentState.TurnedOff;
			}

			return res;
		}
	}
}

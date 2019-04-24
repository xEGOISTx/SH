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
	public abstract class BaseSwitch : IDeviceBase
	{
		public BaseSwitch(int deviceType) { DeviceType = deviceType; }

		public CurrentState State { get; internal set;}

		public int ID { get; internal set; }

		public string Name { get; internal set; }

		public string Description { get; internal set; }

		public bool IsConnected { get; internal set; }

		public IPAddress IP { get; internal set; }

		public MacAddress Mac { get; internal set; }

		public int DeviceType { get;}

		public FirmwareType FirmwareType { get; internal set; }


		protected async Task<bool> TurnOff(IDeviceBaseTask task)
		{
			bool res = await task?.Execute();

			if (res)
			{
				State = CurrentState.TurnedOff;
			}

			return res;
		}

		protected async Task<bool> TurnOn(IDeviceBaseTask task)
		{
			bool res = await task?.Execute();

			if (res)
			{
				State = CurrentState.TurnedOn;
			}

			return res;
		}

		//TODO: добавить абстрактный метод GetOwnPrams
	}
}

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
	public class SwitchOutlet : ISwitchOutlet
	{
		private SwitchOutletTaskList _tasks;
		private IPAddress _IP;
		private bool _isConnected;

		public SwitchOutlet(MacAddress mac, FirmwareType firmwareType, DeviceType deviceType)
		{
			Mac = mac;
			FirmwareType = firmwareType;
			DeviceType = deviceType;

			_tasks = new SwitchOutletTaskList(firmwareType);
		}


		public CurrentState State { get; set; }

		public string Description { get; set; }

		public ISwitchOutletTaskList Tasks => _tasks;

		public int ID { get; set; }

		public string Name { get; set; }

		public bool IsConnected
		{
			get { return _isConnected; }
			set
			{
				if(_isConnected != value)
				{
					_isConnected = value;
					OnConnectedStatysChange();
				}
			}
		}


		public IPAddress IP
		{
			get { return _IP; }
			set
			{
				_IP = value;

				foreach(ISwitchOutletTask task in _tasks)
				{
					(task as SwitchOutletTask).OwnerIP = value;
				}
			}
		}


		public MacAddress Mac { get; set; }

		public DeviceType DeviceType { get; }

		public FirmwareType FirmwareType { get; }

		/// <summary>
		/// Включить
		/// </summary>
		public async void TurnOn()
		{
			ISwitchOutletTask turnOn = _tasks.GetByKey(TaskType.TurnOn);
			bool res = await turnOn?.Execute();

			if (res)
			{
				State = CurrentState.TurnedOn;
			}

		}

		/// <summary>
		/// Выключить
		/// </summary>
		public async void TurnOff()
		{
			ISwitchOutletTask turnOff = _tasks.GetByKey(TaskType.TurnOff);
			bool res = await turnOff?.Execute();

			if (res)
			{
				State = CurrentState.TurnedOff;
			}
		}

		private void OnConnectedStatysChange()
		{
			ConnectedStatysChange?.Invoke(this, new EventArgs());
		}

		public event EventHandler ConnectedStatysChange;
	}
}

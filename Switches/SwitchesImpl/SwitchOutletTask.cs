using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SHBase.Communication;
using SHBase.DevicesBaseComponents;

namespace Switches
{
	public class SwitchOutletTask : ISwitchOutletTask
	{
		private readonly List<IGPIOAction> _actions = new List<IGPIOAction>();


		public SwitchOutletTask(IDeviceBase owner, byte pinNumber, SwitchOutletTaskType taskType)
		{
			Owner = owner;
			TaskType = taskType;
			GPIOAction action;

			if (taskType == SwitchOutletTaskType.TurnOn)
			{
				action = new GPIOAction(pinNumber, GPIOMode.Output, GPIOLevel.High);
			}
			else
			{
				action = new GPIOAction(pinNumber, GPIOMode.Output, GPIOLevel.Low);
			}

			_actions.Add(action);
		}


		public IEnumerable<IGPIOAction> Actions => _actions;

		public SwitchOutletTaskType TaskType { get; }

		public int ID { get; }

		public string Description { get; set; }

		public string VoiceCommand { get; set; }

		public IDeviceBase Owner { get; set; }

		public async Task<bool> Execute()
		{
			Communicator communicator = new Communicator();
			return await communicator.SendGPIOTask(this);
		}
	}
}

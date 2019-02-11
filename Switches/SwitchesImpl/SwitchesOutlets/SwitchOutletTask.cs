using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SHBase.Communication;
using SHBase.DevicesBaseComponents;

namespace Switches.SwitchesOutlets
{
	public class SwitchOutletTask : ISwitchOutletTask
	{
		private readonly List<IGPIOAction> _actions = new List<IGPIOAction>();


		public SwitchOutletTask(IDeviceBase owner, byte pinNumber, TaskType taskType)
		{
			Owner = owner;
			TaskType = taskType;
			GPIOAction action;

			if (taskType == TaskType.TurnOn)
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

		public TaskType TaskType { get; }

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

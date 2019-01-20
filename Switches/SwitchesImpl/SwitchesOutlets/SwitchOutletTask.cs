using System.Collections.Generic;
using System.Net;
using SHBase.Communication;
using SHBase.DevicesBaseComponents;

namespace Switches.SwitchesOutlets
{
	public class SwitchOutletTask : ISwitchOutletTask
	{
		private readonly List<IGPIOAction> _actions = new List<IGPIOAction>();


		public SwitchOutletTask(byte pinNumber, TaskType taskType)
		{
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

		public IPAddress OwnerIP { get; set; }

		public async void Execute()
		{
			Communicator communicator = new Communicator();
			await communicator.SendGPIOTask(this);
		}
	}
}

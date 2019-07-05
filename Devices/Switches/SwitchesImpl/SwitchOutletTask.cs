using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SHToolKit.Communication;
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

			GPIOLevel gPIOLevel = taskType == SwitchOutletTaskType.TurnOn ? GPIOLevel.High : GPIOLevel.Low;
			GPIOAction action = new GPIOAction(pinNumber, GPIOMode.Output, gPIOLevel);

			_actions.Add(action);
		}


		public IEnumerable<IGPIOAction> Actions => _actions;

		public SwitchOutletTaskType TaskType { get; }

		public int ID { get; }

		public string Description { get; set; }

		public string VoiceCommand { get; set; }

		public IDeviceBase Owner { get;}

		//public async Task<bool> Execute()
		//{
		//	Communicator communicator = new Communicator();
		//	return await communicator.SendGPIOTask(this);
		//}
	}
}

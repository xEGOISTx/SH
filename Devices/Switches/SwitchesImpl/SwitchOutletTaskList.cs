using SHBase.DevicesBaseComponents;
using System.Collections;
using System.Collections.Generic;


namespace Switches
{
	public class SwitchOutletTaskList : ISwitchOutletTaskList
	{
		private readonly Dictionary<SwitchOutletTaskType, ISwitchOutletTask> _tasks = new Dictionary<SwitchOutletTaskType, ISwitchOutletTask>();

		public SwitchOutletTaskList(IDeviceBase owner)
		{
			if (owner.FirmwareType == FirmwareType.ESP_8266)
			{
				_tasks.Add(SwitchOutletTaskType.TurnOn, new SwitchOutletTask(owner,5, SwitchOutletTaskType.TurnOn));
				_tasks.Add(SwitchOutletTaskType.TurnOff, new SwitchOutletTask(owner,5, SwitchOutletTaskType.TurnOff));
			}

		}

		public int Count => _tasks.Count;

		public bool ContainsKey(SwitchOutletTaskType key)
		{
			return _tasks.ContainsKey(key);
		}

		public ISwitchOutletTask GetByKey(SwitchOutletTaskType key)
		{
			if(ContainsKey(key))
			{
				return _tasks[key];
			}

			return null;
		}

		public IEnumerator GetEnumerator()
		{
			return _tasks.Values.GetEnumerator();
		}
	}
}

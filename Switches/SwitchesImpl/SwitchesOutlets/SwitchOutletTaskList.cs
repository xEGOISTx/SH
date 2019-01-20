using SHBase.DevicesBaseComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Switches.SwitchesOutlets
{
	public class SwitchOutletTaskList : ISwitchOutletTaskList
	{
		private readonly Dictionary<TaskType, ISwitchOutletTask> _tasks = new Dictionary<TaskType, ISwitchOutletTask>();

		public SwitchOutletTaskList(FirmwareType firmwareType)
		{
			if (firmwareType == FirmwareType.ESP_8266)
			{
				_tasks.Add(TaskType.TurnOn, new SwitchOutletTask(5, TaskType.TurnOn));
				_tasks.Add(TaskType.TurnOff, new SwitchOutletTask(5, TaskType.TurnOff));
			}

		}

		public int Count => _tasks.Count;

		public bool ContainsKey(TaskType key)
		{
			return _tasks.ContainsKey(key);
		}

		public ISwitchOutletTask GetByKey(TaskType key)
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

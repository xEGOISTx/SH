using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public interface IDeviceBaseTaskType<TaskType>
		where TaskType: IDeviceBaseTask
	{
		bool IsPresentTask(int id);

		IEnumerable<TaskType> Tasks { get; }

		TaskType GetTaskByID(int id);
	}
}

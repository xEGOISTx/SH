using SHBase.BaseContainers;

namespace Switches.SwitchesOutlets
{
	/// <summary>
	/// Список задач для обычного выулючателя или розетки
	/// </summary>
	public interface ISwitchOutletTaskList : IContainer<TaskType, ISwitchOutletTask>
	{		
	}
}

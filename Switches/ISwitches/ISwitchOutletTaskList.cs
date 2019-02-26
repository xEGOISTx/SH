using SHBase.BaseContainers;

namespace Switches
{
	/// <summary>
	/// Список задач для обычного выключателя или розетки
	/// </summary>
	public interface ISwitchOutletTaskList : IContainer<SwitchOutletTaskType, ISwitchOutletTask>
	{		
	}
}

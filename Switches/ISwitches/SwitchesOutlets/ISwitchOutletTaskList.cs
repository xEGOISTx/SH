using SHBase.BaseContainers;

namespace ISwitches.SwitchesOutlets
{
	/// <summary>
	/// Список задач для обычного выулючателя или розетки
	/// </summary>
	public interface ISwitchOutletTaskList : IContainer<int, ISwitchOutletTask>
	{		
	}
}

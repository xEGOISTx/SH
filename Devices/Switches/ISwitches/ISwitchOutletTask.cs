using SHBase.DevicesBaseComponents;

namespace Switches
{
	public enum SwitchOutletTaskType
	{
		TurnOn,
		TurnOff
	}

	/// <summary>
	/// Задача для обычного выключателя или розетки
	/// </summary>
	public interface ISwitchOutletTask : IBaseGPIOActionTask<IGPIOAction>
	{
		SwitchOutletTaskType TaskType { get; }
	}
}

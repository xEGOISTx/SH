using SHBase.DevicesBaseComponents;

namespace Switches.SwitchesOutlets
{
	public enum TaskType
	{
		TurnOn,
		TurnOff
	}

	/// <summary>
	/// Задача для обычного выключателя или розетки
	/// </summary>
	public interface ISwitchOutletTask : IBaseGPIOActionTask<IGPIOAction>
	{
		TaskType TaskType { get; }
	}
}

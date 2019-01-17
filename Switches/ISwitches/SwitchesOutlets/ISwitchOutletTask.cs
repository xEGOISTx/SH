using SHBase.DevicesBaseComponents;

namespace ISwitches.SwitchesOutlets
{
	public enum CurrentState
	{
		TurnedOn,
		TurnedOff
	}

	/// <summary>
	/// Задача для обычного выключателя или розетки
	/// </summary>
	public interface ISwitchOutletTask : IGPIOActionTask<IGPIOAction>
	{
		/// <summary>
		/// Текущее состояние 
		/// </summary>
		CurrentState State { get; }
	}
}

using SHBase.DevicesBaseComponents;
using System.Threading.Tasks;

namespace Switches.SwitchesOutlets
{
	public enum CurrentState
	{
		TurnedOn,
		TurnedOff
	}

	/// <summary>
	/// Обычный переключатель или розетка
	/// </summary>
	public interface ISwitchOutlet : IDeviceBase
	{
		/// <summary>
		/// Текущее состояние 
		/// </summary>
		CurrentState State { get; }

		/// <summary>
		/// Задачи
		/// </summary>
		ISwitchOutletTaskList Tasks { get; }

		/// <summary>
		/// Включить
		/// </summary>
		Task<bool> TurnOn();

		/// <summary>
		/// Выключить
		/// </summary>
		Task<bool> TurnOff();
	}
}

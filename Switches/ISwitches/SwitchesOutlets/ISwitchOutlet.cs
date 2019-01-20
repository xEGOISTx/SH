using SHBase.DevicesBaseComponents;

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
		/// Описание
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Задачи
		/// </summary>
		ISwitchOutletTaskList Tasks { get; }

		/// <summary>
		/// Включить
		/// </summary>
		void TurnOn();

		/// <summary>
		/// Выключить
		/// </summary>
		void TurnOff();
	}
}

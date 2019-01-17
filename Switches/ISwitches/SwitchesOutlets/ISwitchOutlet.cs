using SHBase.DevicesBaseComponents;

namespace ISwitches.SwitchesOutlets
{
	public enum SwitchOutletType
	{
		Switch,
		Outlet
	}


	/// <summary>
	/// Обычный переключатель или розетка
	/// </summary>
	public interface ISwitchOutlet : IDeviceBase
	{
		/// <summary>
		/// Тип. Выключатель или розетка
		/// </summary>
		SwitchOutletType Type { get; }

		/// <summary>
		/// Описание
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Задачи
		/// </summary>
		ISwitchOutletTaskList Tasks { get; }
	}
}

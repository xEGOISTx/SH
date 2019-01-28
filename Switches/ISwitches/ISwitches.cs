using Switches.SwitchesOutlets;

namespace Switches
{
	public interface ISwitches
	{
		/// <summary>
		/// Список обычных выключателей
		/// </summary>
		ISwitchesAndOutletsList SwitchList { get; }

		/// <summary>
		/// Список розеток
		/// </summary>
		ISwitchesAndOutletsList OutletList { get; }
	}
}

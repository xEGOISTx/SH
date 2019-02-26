
namespace Switches
{
	public interface ISwitches
	{
		/// <summary>
		/// Список обычных выключателей
		/// </summary>
		ISwitchList SwitchList { get; }

		/// <summary>
		/// Список розеток
		/// </summary>
		IOutletList OutletList { get; }
	}
}

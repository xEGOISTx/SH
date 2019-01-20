using SHBase.DevicesBaseComponents;

namespace Switches.SwitchesOutlets
{
	/// <summary>
	/// Действие пина
	/// </summary>
	public class GPIOAction : IGPIOAction
	{
		public GPIOAction(byte pinNumber, GPIOMode mode, GPIOLevel level)
		{
			PinNumber = pinNumber;
			Mode = mode;
			Level = level;
		}

		/// <summary>
		/// Номер пина
		/// </summary>
		public byte PinNumber { get; set; }

		/// <summary>
		/// Режим
		/// </summary>
		public GPIOMode Mode { get; set; }

		/// <summary>
		/// Уровень напряжения
		/// </summary>
		public GPIOLevel Level { get; set; }
	}
}

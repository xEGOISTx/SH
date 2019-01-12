using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DeviceBase;

namespace DevicesPresenter
{
	/// <summary>
	/// Действие с GPIO
	/// </summary>
	public class ActionGPIO : IActionGPIO
	{
		private GPIOMode _mode;
		private GPIOLevel _level;

		public ActionGPIO(byte pinNumber) : this(pinNumber, GPIOMode.NotDefined, GPIOLevel.NotDefined)
		{
			PinNumber = pinNumber;
		}

		public ActionGPIO(byte pinNumber, GPIOMode mode, GPIOLevel level)
		{
			PinNumber = pinNumber;
			Mode = mode;
			Level = level;

			_mode = mode;
			_level = level;
		}

		/// <summary>
		/// Признак действие изменено
		/// </summary>
		public bool IsChanged { get; private set; }
		
		/// <summary>
		/// Номер пина
		/// </summary>
		public byte PinNumber { get; }

		/// <summary>
		/// Режим
		/// </summary>
		public GPIOMode Mode { get; private set; }

		/// <summary>
		/// Уровень напряжения
		/// </summary>
		public GPIOLevel Level { get; private set; }

		/// <summary>
		/// Изменить действие для пина
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="level"></param>
		public void ChangeAction(GPIOMode mode, GPIOLevel level)
		{
			IsChanged = mode != _mode || level != _level;

			Mode = mode;
			Level = level;
		}

		public void ResetChanged()
		{
			_mode = Mode;
			_level = Level;
			IsChanged = false;
		}

		/// <summary>
		/// Возвращает копию действия
		/// </summary>
		/// <returns></returns>
		public IActionGPIO Copy()
		{
			return new ActionGPIO(PinNumber)
			{
				Level = Level,
				Mode = Mode,
				IsChanged = IsChanged
			};
		}

	}
}
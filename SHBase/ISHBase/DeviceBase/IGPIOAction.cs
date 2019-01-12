using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DeviceBase
{
	/// <summary>
	/// Режим GPIO
	/// </summary>
	public enum GPIOMode
	{
		Input,
		Output,
		NotDefined
	}

	/// <summary>
	/// Уровень на GPIO
	/// </summary>
	public enum GPIOLevel
	{
		High,
		Low,
		NotDefined
	}

	public interface IGPIOAction
	{
		byte PinNumber { get; }

		GPIOMode Mode { get; }

		GPIOLevel Level { get; }
	}
}

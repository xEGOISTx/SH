using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switches
{
	public enum CurrentState
	{
		TurnedOff,
		TurnedOn
	}

	public interface IBaseSwitch : IDeviceBase
	{
		/// <summary>
		/// Текущее состояние 
		/// </summary>
		CurrentState State { get; }

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

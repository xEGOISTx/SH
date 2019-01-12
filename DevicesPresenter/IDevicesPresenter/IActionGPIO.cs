using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DeviceBase;

namespace DevicesPresenter
{

	/// <summary>
	/// Интерфейс действия с GPIO
	/// </summary>
	public interface IActionGPIO : IGPIOAction
	{
		bool IsChanged { get; }

		void ChangeAction(GPIOMode mode, GPIOLevel level);
	}
}

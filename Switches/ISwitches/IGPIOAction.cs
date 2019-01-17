using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace ISwitches
{
	/// <summary>
	/// Интерфейс действия с GPIO
	/// </summary>
	public interface IGPIOAction : IBaseGPIOAction
    {
		bool IsChanged { get; }

		void ChangeAction(GPIOMode mode, GPIOLevel level);
	}
}

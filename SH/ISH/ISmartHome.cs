using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SH
{
	public interface ISmartHome
	{
		SHToolKit.DevicesManagement.IDevicesManager DevicesManager { get; }

		Task<bool> Start();
	}
}

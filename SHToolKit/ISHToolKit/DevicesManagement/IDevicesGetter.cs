using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit.DevicesManagement
{
	public interface IDevicesGetter
	{
		object GetDevices<DevsType>();
	}
}

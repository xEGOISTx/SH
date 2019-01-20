using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public interface IBaseDevicesLoader
	{
		Task<bool> Load();

		Task<bool> RenameDevice(IDeviceBase device);
	}
}

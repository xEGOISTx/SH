using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public interface IDevicesLoader
	{
		IEnumerable<IDeviceBase> LoadDevices();

		void SaveDevices(IEnumerable<IDeviceBase> devices);
	}
}

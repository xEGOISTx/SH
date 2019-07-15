using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit.DataManagement
{
	public interface IDevicesLoader
	{
		IOperationResultDevicesLoad LoadDevices(int devicesType);

		IOperationResultSaveDevices SaveDevices(IDevice[] devices);
	}
}

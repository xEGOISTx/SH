using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
	public interface IDataManager
	{
		IResultOperationLoad LoadDevices(int deviceType);

		IResultOperationSave SaveDevices(IDeviceInfo[] devices);

		IDBOperationResult RenameDevice(IDeviceInfo device);


		IDeviceInfo CreateDeviceInfo(string description, int deviceType, int firmwareType, string macAddres, int id = 0);
	}
}

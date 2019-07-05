using SHToolKit.DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
	internal class DataLoader : IDataLoader
	{
		private IDevicesLoader _devicesLoader = new DevicesLoader();
		private ISettingsLoader _settingsLoader;

		public IDevicesLoader GetDevicesLoader()
		{
			return _devicesLoader;
		}

		public ISettingsLoader GetSettingsLoader()
		{
			throw new NotImplementedException();
		}
	}
}

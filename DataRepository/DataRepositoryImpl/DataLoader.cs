using System;
using SH.DataPorts;

namespace SH.DataRepository
{
    internal class DataLoader : IDataLoader
    {
        public IDevicesLoader GetDevicesLoader()
        {
            return new DevicesLoader();
        }

        public ISettingsLoader GetSettingsLoader()
        {
			return new SettingsLoader();
        }
    }
}

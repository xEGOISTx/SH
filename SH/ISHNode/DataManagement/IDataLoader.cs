using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SH.DataManagement
{
	public interface IDataLoader
	{
		IDevicesLoader GetDevicesLoader();

		ISettingsLoader GetSettingsLoader();
	}
}

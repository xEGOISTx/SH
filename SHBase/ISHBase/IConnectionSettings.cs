using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase
{
	public interface IConnectionSettings
	{
		IConnectionParams DeviceConnParams { get; }

		IConnectionParams RouterConnParams { get; }		

		bool ChangeParamsForDevice(string ssid, string password);

		bool ChangeParamsForRouter(string ssid, string password);

		void Save();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SH.DataManagement
{
	public interface IConnectionSettings
	{
		IParameter[] Parameters { get; }


		//string RouterIP { get; }

		//string RouterLogin { get; }

		//string RouterPassword { get; }

		//string RouterSsid { get; }

		//string[] APSSIDsForSearch { get; }

		//string RouterAPPassword { get; }

		//string DeviceDafaultIP { get; }

		//string DeviceAPPassword { get; }
	}
}

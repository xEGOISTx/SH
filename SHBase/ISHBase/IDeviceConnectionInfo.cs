using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace SHBase
{
	public interface IDeviceConnectionInfo
	{
		IDeviceBase Device { get; }

		bool ConnectionState { get; }
	}
}

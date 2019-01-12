using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DeviceBase
{
	public interface IDeviceBaseTask
	{
		IPAddress OwnerIP { get; }

		void Execute();
	}
}

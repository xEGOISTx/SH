using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit
{
	public interface ITools
	{
		DevicesManagement.IDevicesFinder GetDevicesFinder(IRouterParser routerParser = null);

		Communication.ICommunicator GetCommunicator();

		IRouterParser GetRouterParser();
	}
}

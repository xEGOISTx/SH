using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit
{
	public interface IRouterParser
	{
		Task<IParseOperationResult> GetDevicesIPs(IPAddress routerIP, string login, string password);
	}
}

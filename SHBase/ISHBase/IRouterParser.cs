using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHBase
{
	public interface IRouterParser
	{
		Task<IEnumerable<IPAddress>> GetDevicesIPs(IPAddress routerIP, string login, string password);
	}
}

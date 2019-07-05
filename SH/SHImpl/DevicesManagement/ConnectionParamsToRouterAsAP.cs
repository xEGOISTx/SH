using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SH
{
	internal class ConnectionParamsToRouterAsAP : SHBase.IConnectionParamsToAP
	{
		public string Ssid { get; set; }

		public string Password { get; set; }
	}
}

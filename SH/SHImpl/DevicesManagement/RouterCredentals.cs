using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SHBase;

namespace SH
{
	internal class RouterCredentals : SHBase.ICredentials
	{
		public RouterCredentals(IPAddress routerIP)
		{
			Uri = new Uri($"http://{ routerIP }/");
		}

		public Uri Uri { get; set; }

		public string Login { get; set; }

		public string Password { get; set; }
	}
}

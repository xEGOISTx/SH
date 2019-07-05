using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SHBase
{
	public interface ICredentials
	{
		Uri Uri { get; }

		string Login { get; }

		string Password { get; }
	}
}

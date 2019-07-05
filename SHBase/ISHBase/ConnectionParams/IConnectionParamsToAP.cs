using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase
{
	public interface IConnectionParamsToAP
	{
		string Ssid { get; }

		string Password { get; }
	}
}

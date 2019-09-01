using System.Threading.Tasks;
using SH.Core;
using System;

namespace SH.Communication
{
	public interface IRouterParser
	{
		IParseOperationResult GetActiveIPs(Uri uriToParse, ICredentials routerCredentials);
	}
}

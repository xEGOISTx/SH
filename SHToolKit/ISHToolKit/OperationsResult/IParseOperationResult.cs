using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit
{
	public interface IParseOperationResult //: SHBase.IOperationResult
	{
		IEnumerable<string> IPs { get; }
	}
}

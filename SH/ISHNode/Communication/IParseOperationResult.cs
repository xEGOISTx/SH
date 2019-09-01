using SH.Core;
using System.Collections.Generic;

namespace SH.Communication
{
	public interface IParseOperationResult : IOperationResult
	{
		IEnumerable<string> IPs { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit
{
	internal class ParseOperationResult : OperationResult, IParseOperationResult
	{
		public ParseOperationResult() { }

		public ParseOperationResult(OperationResult operationResult) : base(operationResult){ }

		public IEnumerable<string> IPs { get; set; }
	}
}

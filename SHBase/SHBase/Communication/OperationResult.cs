using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.Communication
{
	public class OperationResult
	{
		internal OperationResult() { }

		public bool Success { get; internal set; }

		internal string ResponseMessage { get; set; } = string.Empty;

		public string ErrorMessage { get; internal set; } = string.Empty;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.Communication
{
	public class OperationResult
	{
		public bool Success { get; set; }

		internal string ResponseMessage { get; set; } = string.Empty;

		public string ErrorMessage { get; set; } = string.Empty;
	}
}

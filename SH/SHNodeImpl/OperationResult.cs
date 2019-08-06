using System;
using System.Collections.Generic;
using System.Text;
using SH.Core;

namespace SH.Node
{
	public class OperationResult : IOperationResult
	{
		public bool Success { get; set; }

		public string ErrorMessage { get; set; }
	}
}

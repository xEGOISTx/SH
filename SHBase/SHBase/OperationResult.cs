using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase
{
	public class OperationResult : IOperationResult
	{
		internal OperationResult() { }

		public OperationResult(OperationResult operationResult)
		{
			Success = operationResult.Success;
			ResponseMessage = operationResult.ResponseMessage;
			ErrorMessage = operationResult.ErrorMessage;
		}

		public bool Success { get; internal set; }

		public string ResponseMessage { get; internal set; } = string.Empty;

		public string ErrorMessage { get; internal set; } = string.Empty;
	}
}

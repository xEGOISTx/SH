using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit
{
	internal class OperationResult : IOperationResult
	{
		public OperationResult() { }

		public OperationResult(OperationResult operationResult)
		{
			Success = operationResult.Success;
			ResponseMessage = operationResult.ResponseMessage;
			ErrorMessage = operationResult.ErrorMessage;
		}

		public bool Success { get; internal set; }

		public string ResponseMessage { get; set; } = string.Empty;

		public string ErrorMessage { get; internal set; } = string.Empty;
	}
}

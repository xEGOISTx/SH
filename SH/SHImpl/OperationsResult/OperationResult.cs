using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SH
{
	internal class OperationResult : SHBase.IOperationResult
	{
		public OperationResult(SHBase.IOperationResult operationResult = null)
		{
			if (operationResult != null)
			{
				Success = operationResult.Success;
				ErrorMessage = operationResult.ErrorMessage;
			}
		}

		public bool Success { get; set; }

		public string ErrorMessage { get; set; }
	}
}

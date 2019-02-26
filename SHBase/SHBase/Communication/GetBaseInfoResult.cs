using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.Communication
{
	public class GetBaseInfoResult : OperationResult
	{
		internal GetBaseInfoResult() { }

		internal GetBaseInfoResult(OperationResult operationResult)
		{
			Success = operationResult.Success;
			ResponseMessage = operationResult.ResponseMessage;
			ErrorMessage = operationResult.ErrorMessage;
		}

		public DevicesBaseComponents.IDeviceBase BasicInfo { get; internal set; }
	}
}

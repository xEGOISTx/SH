namespace SH
{
	internal class GetBaseInfoResult : OperationResult , IOperationGetBaseInfoResult
	{
		public GetBaseInfoResult() { }

		public GetBaseInfoResult(SHBase.IOperationResult operationResult) : base (operationResult){ }

		public SHBase.DevicesBaseComponents.IDeviceBase BasicInfo { get; set; }
	}
}

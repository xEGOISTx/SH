namespace SHToolKit
{
	internal class GetBaseInfoResult : OperationResult , IOperationGetBaseInfoResult
	{
		public GetBaseInfoResult() { }

		public GetBaseInfoResult(OperationResult operationResult) : base (operationResult){ }

		public SHBase.DevicesBaseComponents.IDeviceBase BasicInfo { get; set; }
	}
}

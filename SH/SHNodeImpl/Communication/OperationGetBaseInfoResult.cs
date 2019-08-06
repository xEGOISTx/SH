namespace SH.Communication
{
	internal class OperationGetBaseInfoResult : Core.IOperationResult
	{

		public DeviceInfo BasicInfo { get; set; }

		public bool Success { get; set; }

		public string ErrorMessage { get; set; }
	}
}

namespace SH.Communication
{
	internal class GetBaseInfoOperationResult : Core.IOperationResult
	{

		public DeviceInfo DeviceBasicInfo { get; set; }

		public bool Success { get; set; }

		public string ErrorMessage { get; set; }
	}
}

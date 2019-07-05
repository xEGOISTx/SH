
namespace DataManager
{
	internal class OperationResultSaveDevices : SHToolKit.IOperationResultSaveDevices
	{
		public int[] DevicesIDs { get; set; }

		public bool Success { get; set; }

		public string ErrorMessage { get; set; }
	}
}

using SHToolKit;
using SHToolKit.DataManagement;

namespace DataManager
{
	internal class OperationResultDevicesLoad : IOperationResultDevicesLoad
	{
		public IDBDevice[] Devices { get; set; }

		public bool Success { get; set; }

		public string ErrorMessage { get; set; }
	}
}

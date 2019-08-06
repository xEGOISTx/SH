using SH.Core;

namespace SH.DataManagement
{
	public interface IOperationResultSaveDevices : IOperationResult
	{
		int[] DevicesIDs { get; }
	}
}

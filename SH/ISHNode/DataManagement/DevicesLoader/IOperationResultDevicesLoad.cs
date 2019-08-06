using SH.Core;

namespace SH.DataManagement
{
	public interface IOperationResultDevicesLoad : IOperationResult
	{
		IDevice[] Devices { get; }
	}
}

using SH.Core;

namespace SH.DataManagement
{
	public interface IOperationResultDevicesLoad : IOperationResult
	{
		IDeviceData[] Devices { get; }
	}
}

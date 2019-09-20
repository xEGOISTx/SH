using SH.Core;

namespace SH.DataPorts
{
	public interface IOperationResultDevicesLoad : IOperationResult
	{
		IDeviceData[] Devices { get; }
	}
}

using System.Collections.Generic;
using SHBase.DevicesBaseComponents;

namespace SH
{
	public interface IFindDevicesOperationResult : SHBase.IOperationResult
	{
		IReadOnlyDictionary<int, IEnumerable<IDeviceBase>> FoundDevices { get; }
	}
}

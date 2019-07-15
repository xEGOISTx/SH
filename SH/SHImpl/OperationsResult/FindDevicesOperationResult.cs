using System.Collections.Generic;
using SHBase.DevicesBaseComponents;

namespace SH
{
	internal class FindDevicesOperationResult : OperationResult, IFindDevicesOperationResult
	{

		public FindDevicesOperationResult() { }

		public FindDevicesOperationResult(OperationResult result): base(result) { }

		public IReadOnlyDictionary<int, IEnumerable<IDeviceBase>> FoundDevices { get; set; }
	}
}

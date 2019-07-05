using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace SHToolKit
{
	internal class FindDevicesOperationResult : OperationResult, IFindDevicesOperationResult
	{

		public FindDevicesOperationResult() { }

		public FindDevicesOperationResult(OperationResult result): base(result) { }

		public IReadOnlyDictionary<int, IEnumerable<IDeviceBase>> FoundDevices { get; set; }
	}
}

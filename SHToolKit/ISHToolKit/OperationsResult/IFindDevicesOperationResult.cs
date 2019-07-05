using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace SHToolKit
{
	public interface IFindDevicesOperationResult : SHBase.IOperationResult
	{
		IReadOnlyDictionary<int, IEnumerable<IDeviceBase>> FoundDevices { get; }
	}
}

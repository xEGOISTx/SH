using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit
{
	public interface IFindNotConnectedDevicesOperationResult : SHBase.IOperationResult
	{
		DevicesManagement.IDevicesConnectionInfo ConnectionInfo { get; }
	}
}

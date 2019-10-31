using SHToolKit.DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit
{
	public interface IOperationResultDevicesLoad //: SHBase.IOperationResult
	{
		IDevice[] Devices { get; }
	}
}

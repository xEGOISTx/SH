using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit
{
	public interface IOperationResultSaveDevices //: SHBase.IOperationResult
	{
		int[] DevicesIDs { get; }
	}
}

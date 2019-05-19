using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit
{
	public interface IOperationGetBaseInfoResult : IOperationResult
	{
		SHBase.DevicesBaseComponents.IDeviceBase BasicInfo { get; }
	}
}

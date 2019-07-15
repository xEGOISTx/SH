using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SH
{
	public interface IOperationGetBaseInfoResult : SHBase.IOperationResult
	{
		SHBase.DevicesBaseComponents.IDeviceBase BasicInfo { get; }
	}
}

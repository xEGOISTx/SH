using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHToolKit.DataManagement;

namespace SHToolKit
{
	public interface ILoadSettingOperationResult //: SHBase.IOperationResult
	{
		IConnectionSettings ConnectionSettings { get; }
	}
}

using SH.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SH.DataManagement
{
	public interface ILoadSettingOperationResult : IOperationResult
	{
		IConnectionSettings ConnectionSettings { get; }
	}
}

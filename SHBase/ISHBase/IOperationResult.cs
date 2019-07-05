using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase
{
	public interface IOperationResult
	{
		bool Success { get; }

		string ErrorMessage { get; }
	}
}

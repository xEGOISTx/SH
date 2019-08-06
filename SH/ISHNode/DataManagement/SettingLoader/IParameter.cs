using System;
using System.Collections.Generic;
using System.Text;

namespace SH.DataManagement
{
	public interface IParameter
	{
		int Index { get; }

		string Value { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public interface ICommandParameter
	{
		string Name { get; }

		string Value { get; }
	}
}

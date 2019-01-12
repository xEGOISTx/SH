using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DeviceBase
{
	public interface ICommandParameter
	{
		string Name { get; }

		string Value { get; }
	}
}

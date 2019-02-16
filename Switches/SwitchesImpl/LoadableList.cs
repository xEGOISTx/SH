using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switches
{
	internal abstract class LoadableList
	{
		internal abstract ISwitchesLoader GetLoader();

		internal abstract DBConvertor<IBaseSwitch> Convertor { get; }
	}
}

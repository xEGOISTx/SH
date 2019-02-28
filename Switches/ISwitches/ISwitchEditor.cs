using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switches
{
	public interface ISwitchEditor
	{
		void Rename(IBaseSwitch sw, string description);
	}
}

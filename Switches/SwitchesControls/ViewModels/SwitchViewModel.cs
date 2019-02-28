using Switches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;

namespace SwitchesControls.ViewModels
{
	public class SwitchViewModel : SwitchOutletBaseViewModel
	{
		private readonly ISwitch _device;

		public SwitchViewModel(ISwitch sw, ISwitchEditor switchEditor) : base(sw, switchEditor)
		{
			_device = sw;
		}
	}
}

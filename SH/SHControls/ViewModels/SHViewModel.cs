using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SH;

namespace SHControls.ViewModels
{
	public class SHViewModel
	{
		private readonly ISmartHome _sHome;

		public SHViewModel(ISmartHome smartHome)
		{
			_sHome = smartHome;
			DevicesVM = new DevicesViewModel(_sHome.DevicesManager);
		}


		public DevicesViewModel DevicesVM { get; }

		public void Refresh()
		{
			DevicesVM.Refresh.Execute(null);
		}
	}
}

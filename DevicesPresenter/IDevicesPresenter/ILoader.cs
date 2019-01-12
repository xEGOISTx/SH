using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase;

namespace DevicesPresenter
{
	public interface ILoader
	{
		IConnectionParams LoadDeviceConnectionParams();

		IConnectionParams LoadRouterConnectionParams();
	}
}

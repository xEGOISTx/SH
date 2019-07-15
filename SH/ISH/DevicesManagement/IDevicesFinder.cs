using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SHBase;
using SHBase.DevicesBaseComponents;

namespace SH.DevicesManagement
{
	public interface IDevicesFinder
	{
		Task<IFindDevicesOperationResult> FindAndConnectNewDevicesToRouterAsync(IConnectionParamsToAP connParamsToRouter, IPAddress devDefaultIP, string devPassAsAP);

		Task<IFindDevicesOperationResult> FindDevicesAtRouterIfItsConn(IDevicesConnectionInfo devsConnInfo, SHBase.ICredentials routerCredentials);

		Task<IDevicesConnectionInfo> FindNotConnectedDevices(IEnumerable<IDeviceBase> devices);
	}
}

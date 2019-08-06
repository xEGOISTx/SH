using SH.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SH.Node
{
	public interface IDeviceEditor
	{
		void ChangeID(IDeviceBase device, int id);

		void ChangeName(IDeviceBase device, string name);

		void ChangeDescription(IDeviceBase device, string description);

		void ChangeConnectionState(IDeviceBase device, bool isConnected);

		void ChangeIPAddress(IDeviceBase device, IPAddress ip);

		void ChangeMacAddress(IDeviceBase device, MacAddress mac);

		void ChangeDeviceType(IDeviceBase device, int dType);

		void ChangeFirmwareType(IDeviceBase device, int fType);

		IDeviceBase CreateDevice();
	}
}

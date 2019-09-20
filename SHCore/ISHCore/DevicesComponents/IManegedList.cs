using System.Collections.Generic;
using System.Net;

namespace SH.Core.DevicesComponents
{
	public interface IManegedList<out DType> : IEnumerable<DType>
		where DType : IDevice
	{
		int Count { get; }

		int DevicesType { get; }

		DType GetByID(int id);

		bool IsPresent(int id);

		void RefreshDeviceConnectionState(IDeviceConnectionState connectionInfo);

        DType AddNewDevice(int id, int deviceType, IPAddress iP, MacAddress mac, IDeviceCommandList commands, string description = null);

		void HandleDeviceRequest(IDeviceRequest request);

        IDefaultDeviceCommandParams GetDefaultParamsForCommand(int commandID);

		event DeviceEventHandler RemovingDevice;
	}
}

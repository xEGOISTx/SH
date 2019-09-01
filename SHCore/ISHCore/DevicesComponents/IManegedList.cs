using System.Collections.Generic;
using System.Net;

namespace SH.Core.DevicesComponents
{
	public interface IManegedList<DType> : IEnumerable<DType>
		where DType : IDevice
	{
		int Count { get; }

		int DevicesType { get; }

		DType GetByID(int id);

		bool Contains(int id);

		void Add(DType device);

		void AddRange(IEnumerable<DType> devices);

		void Remove(DType device);

		void RefreshDeviceConnectionState(IDeviceConnectionState connectionInfo);

		DType CreateDevice(int id, int deviceType, IPAddress iP, MacAddress mac, string description = null);

		void SetCommandsToDevice(IDevice device, IDeviceCommandList commands);

		void HandleDeviceRequest(IDeviceRequest request);

        IDefaultDeviceCommandParams GetDefaultParamsForCommand(int commandID);

		event DeviceEventHandler RemoveDevice;
	}
}

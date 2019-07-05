using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase;
using SHBase.DevicesBaseComponents;

namespace SHToolKit.DevicesManagement
{
	internal class DevicesConnectionInfo : IDevicesConnectionInfo
	{
		private readonly Dictionary<int, List<IDeviceConnectionInfo>> _connectionChangesInfo = new Dictionary<int, List<IDeviceConnectionInfo>>();

		private readonly Dictionary<int, IDeviceBase> _nonConnDevs = new Dictionary<int, IDeviceBase>();

		public IReadOnlyDictionary<int, IDeviceBase> NotConnectedDevices => _nonConnDevs;



		public IReadOnlyDictionary<int, IEnumerable<IDeviceConnectionInfo>> GetСonnectionChangesInfo()
		{
			Dictionary<int, IEnumerable<IDeviceConnectionInfo>> connectionChangesInfo = new Dictionary<int, IEnumerable<IDeviceConnectionInfo>>();

			foreach(var pair in _connectionChangesInfo)
			{
				connectionChangesInfo.Add(pair.Key, pair.Value);
			}

			return connectionChangesInfo;
		}

		public void AddConnectionChanges(IDeviceConnectionInfo connectionInfo)
		{
			if(!_connectionChangesInfo.ContainsKey(connectionInfo.Device.DeviceType))
			{
				_connectionChangesInfo.Add(connectionInfo.Device.DeviceType, new List<IDeviceConnectionInfo> { connectionInfo });
			}
			else
			{
				_connectionChangesInfo[connectionInfo.Device.DeviceType].Add(connectionInfo);
			}
		}

		public void AddNotConnectedDevice(IDeviceBase device)
		{
			if (!_nonConnDevs.ContainsKey(device.ID))
			{
				_nonConnDevs.Add(device.ID, device);
			}

		}
	}
}

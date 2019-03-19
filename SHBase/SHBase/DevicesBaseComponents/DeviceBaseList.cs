using SHBase.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public abstract class DeviceBaseList : IEnumerable
	{
		protected readonly Dictionary<int, IDeviceBase> devices = new Dictionary<int, IDeviceBase>();

		public DeviceBaseList(int devicesType)
		{
			DevicesType = devicesType;
		}

		public int DevicesType { get; }

		public bool IsLoaded { get; protected set; }

		public int Count => devices.Count;

		public bool ContainsKey(int key)
		{
			return devices.ContainsKey(key);
		}

		public virtual IEnumerator GetEnumerator()
		{
			return devices.Values.GetEnumerator();
		}

		public virtual void Add(IDeviceBase device)
		{
			if(device != null && device.ID > 0 && !devices.ContainsKey(device.ID))
			{
				devices.Add(device.ID, device);
			}
		}

		/// <summary>
		/// Проверить соответствие устройства с роутера с имеющимся устройсвом
		/// </summary>
		/// <param name="deviceFromRouter"></param>
		/// <param name="device"></param>
		/// <returns></returns>
		public virtual bool CheckCorresponding(IDeviceBase deviceFromRouter, IDeviceBase device)
		{
			return deviceFromRouter.Mac == device.Mac
				&& deviceFromRouter.FirmwareType == device.FirmwareType
				&& deviceFromRouter.DeviceType == device.DeviceType;
		}

		public abstract Task<bool> Load();


		/// <summary>
		/// Синхронизация с подключенными к роутеру устройствами
		/// </summary>
		/// <param name="deviceInfos"></param>
		/// <returns></returns>
		public abstract Task Synchronization(IEnumerable<IDeviceBase> deviceInfos, Communicator communicator);

		public abstract Task<IEnumerable<IDeviceBase>> GetNotConnectedDevicesAsync(Communicator communicator);

		public abstract IDeviceBase CreateDevice(int id,string name, IPAddress iP,FirmwareType firmwareType, MacAddress mac);

	}
}

﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public abstract class DeviceBaseList : IEnumerable
	{
		protected readonly Dictionary<int, IDeviceBase> _devices = new Dictionary<int, IDeviceBase>();

		public DeviceBaseList(int devicesType)
		{
			DevicesType = devicesType;
		}

		#region Properties
		public int DevicesType { get; }

		public bool IsLoaded { get; protected set; }

		public int Count => _devices.Count;
		#endregion Properties



		#region Methods
		public bool ContainsKey(int key)
		{
			return _devices.ContainsKey(key);
		}

		public virtual IEnumerator GetEnumerator()
		{
			return _devices.Values.GetEnumerator();
		}

		public void Add(IDeviceBase device)
		{
			if (device != null && device.ID > 0 && !_devices.ContainsKey(device.ID) && device.DeviceType == DevicesType)
			{
				_devices.Add(device.ID, device);
			}
		}

		public void AddRange(IEnumerable<IDeviceBase> devices)
		{
			foreach(IDeviceBase device in devices.ToArray())
			{
				Add(device);
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


		/// <summary>
		/// Синхронизация с подключенными к роутеру устройствами
		/// </summary>
		/// <param name="devicesFromRouter"></param>
		/// <returns></returns>
		public abstract Task Synchronization(IEnumerable<IDeviceBase> devicesFromRouter);

		public abstract IDeviceBase CreateDevice(int id, string name, string description, IPAddress iP, FirmwareType firmwareType, MacAddress mac);

		public abstract void RefreshDevicesConnectionState(IEnumerable<IDeviceConnectionInfo> connectionInfos);
		#endregion Methods

	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public abstract class Devices
	{
		/// <summary>
		/// Признак, что устройства загружены
		/// </summary>
		public bool IsLoaded { get; protected set; }

		/// <summary>
		/// Проверить соответствует ли устройство этому набору устройств
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		public abstract  bool CheckForComplianceDevice(IDeviceBase device);

		/// <summary>
		/// Добавить и сохранить новые устройства
		/// </summary>
		/// <param name="newDevices"></param>
		public abstract Task<bool> AddAndSaveNewDevices(IEnumerable<IDeviceBase> newDevices);

		/// <summary>
		/// Загрузить устройства
		/// </summary>
		public abstract void Load();

		/// <summary>
		/// Синхронизация с подключенными к роутеру устройствами. Предварительно должна быть произведена загрузка(метод Load)
		/// </summary>
		/// <param name="newDevices"></param>
		/// <returns></returns>
		public abstract Task<bool> Synchronization(IEnumerable<IDeviceBase> newDevices);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase;
using SHBase.DevicesBaseComponents;

namespace SH.DevicesManagement
{
	public interface IDevicesConnectionInfo
	{
		/// <summary>
		/// Не соединённые устройства
		/// </summary>
		IReadOnlyDictionary<int, IDeviceBase> NotConnectedDevices { get; }

		/// <summary>
		/// Устройства изменившие состояние соединения
		/// </summary>
		IReadOnlyDictionary<int, IEnumerable<IDeviceConnectionInfo>> GetСonnectionChangesInfo();
	}
}

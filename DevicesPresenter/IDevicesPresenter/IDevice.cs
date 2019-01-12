using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SHBase.DeviceBase;

namespace DevicesPresenter
{
	/// <summary>
	/// Интерфейс устройства
	/// </summary>
	public interface IDevice : IDeviceBase, IDeviceBaseTaskType<IDeviceTask>
	{
		/// <summary>
		/// Идентификатор устройства
		/// </summary>
		ushort ID { get; }

		/// <summary>
		/// Описание
		/// </summary>
		string Description { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public interface IDevices
	{
		/// <summary>
		/// Признак, что устройства загружены
		/// </summary>
		bool IsLoaded { get; }

		/// <summary>
		/// Проверить соответствует ли устройство этому набору устройств
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		bool CheckForComplianceDevice(IDeviceBase device);

		/// <summary>
		/// Добавить и сохранить новые устройства
		/// </summary>
		/// <param name="newDevices"></param>
		Task<bool> AddAndSaveNewDevices(IEnumerable<IDeviceBase> newDevices);

		/// <summary>
		/// Загрузить устройства
		/// </summary>
		void Load();
	}
}

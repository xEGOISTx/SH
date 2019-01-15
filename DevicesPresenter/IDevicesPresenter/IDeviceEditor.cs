using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesPresenter
{
	public interface IDeviceEditor
	{
		/// <summary>
		/// Создать задачу для устройства
		/// </summary>
		/// <param name="deviceCopy"></param>
		/// <returns></returns>
		IDeviceTask CreateNewTaskFor(ISwitchingDevice deviceCopy);

		/// <summary>
		/// Получить копию устройства по ID
		/// </summary>
		/// <param name="deviceID"></param>
		/// <returns></returns>
		ISwitchingDevice GetDeviceCopy(ushort deviceID);

		/// <summary>
		/// Применить и сохранить изменения
		/// </summary>
		/// <param name="deviceCopy"></param>
		/// <returns></returns>
		bool ApplyAndSaveChanges(ISwitchingDevice deviceCopy);

		/// <summary>
		/// Пометить задачу на удаление
		/// </summary>
		/// <param name="deviceCopy"></param>
		/// <param name="taskID"></param>
		void MarkTaskForDelete(ISwitchingDevice deviceCopy, int taskID);
	}
}

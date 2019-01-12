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
		IDeviceTask CreateNewTaskFor(IDevice deviceCopy);

		/// <summary>
		/// Получить копию устройства по ID
		/// </summary>
		/// <param name="deviceID"></param>
		/// <returns></returns>
		IDevice GetDeviceCopy(ushort deviceID);

		/// <summary>
		/// Применить и сохранить изменения
		/// </summary>
		/// <param name="deviceCopy"></param>
		/// <returns></returns>
		bool ApplyAndSaveChanges(IDevice deviceCopy);

		/// <summary>
		/// Пометить задачу на удаление
		/// </summary>
		/// <param name="deviceCopy"></param>
		/// <param name="taskID"></param>
		void MarkTaskForDelete(IDevice deviceCopy, int taskID);
	}
}

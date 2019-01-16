using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesPresenter
{
	//TODO: разделить логику на редактирования устройсва и редактирования задач для устройства
	internal class DeviceEditor : IDeviceEditor
	{
		private readonly IDevicesManager _devicesManager;

		internal DeviceEditor(IDevicesManager devicesManager)
		{
			_devicesManager = devicesManager;
		}


		#region Methods
		/// <summary>
		/// Создать задачу для устройства
		/// </summary>
		/// <param name="deviceCopy"></param>
		/// <returns></returns>
		public IDeviceTask CreateNewTaskFor(ISwitchingDevice deviceCopy)
		{
			DeviceTask task = null;

			if (deviceCopy != null)
			{
				int tempID = 0;
				if ((deviceCopy as SwitchingDevice).Tasks.Count() > 0)
				{
					tempID = (deviceCopy as SwitchingDevice).Tasks.Min(t => t.ID);

					if (tempID > 0)
						tempID = 0;
					else
						tempID--;
				}

				task = new DeviceTask { ID = tempID, Description = "New task" };
				(deviceCopy as SwitchingDevice).AddTask(task);
			}
			return task;
		}

		/// <summary>
		/// Применить и сохранить изменения
		/// </summary>
		/// <param name="deviceCopy"></param>
		/// <returns></returns>
		public bool ApplyAndSaveChanges(ISwitchingDevice deviceCopy)
		{
			Loader loader = new Loader();

			ISwitchesList switches = _devicesManager.Devices.GetDevices<ISwitchesList>();

			SwitchingDevice originalDevice = switches[deviceCopy.ID] as SwitchingDevice;
			List<IDeviceTask> newTasks = new List<IDeviceTask>();
			List<IDeviceTask> changedTasks = new List<IDeviceTask>();

			if(originalDevice.Description != deviceCopy.Description)
			{
				bool res = loader.UpdateDeviceDescriptionInDB(deviceCopy, deviceCopy.Description);
				if (res)
				{
					originalDevice.Description = deviceCopy.Description;
				}
			}

			//получаем задачи помеченные на удаление
			List<IDeviceTask> tasksForDelete = deviceCopy.Tasks.Where(t => (t as DeviceTask).IsDelete).ToList();
			//delete tasks from DB

			//удаляем задачи из копии девайса
			(deviceCopy as SwitchingDevice).RemoveTasks(tasksForDelete);


			//выбираем новые и изменёные задачи
			foreach (IDeviceTask task in deviceCopy.Tasks)
			{
				if ((task as DeviceTask).IsChanged)
				{
					changedTasks.Add(task);
				}

				if (task.IsNew)
				{
					(task as DeviceTask).ID = GenerateIDForTask(deviceCopy);
					newTasks.Add(task);
				}

				(task as DeviceTask).ResetStatusChanged();
			}

			//Update changed tasks to DB
			//Save new tasks to DB.
			////Set IDs for new tasks

			//очищаем список задач в оригинальном девайсе
			originalDevice.ClearTasks();
			//добавляем задачи с копии оригиналу
			originalDevice.AddRange(deviceCopy.Tasks);


			return true;
		}

		/// <summary>
		/// Получить копию устройства по ID
		/// </summary>
		/// <param name="deviceID"></param>
		/// <returns></returns>
		public ISwitchingDevice GetDeviceCopy(ushort deviceID)
		{
			SwitchingDevice device = _devicesManager.Devices.GetDevices<ISwitchesList>()[deviceID] as SwitchingDevice;
			return device.Copy();
		}

		/// <summary>
		/// Пометить задачу на удаление
		/// </summary>
		/// <param name="deviceCopy"></param>
		/// <param name="taskID"></param>
		public void MarkTaskForDelete(ISwitchingDevice deviceCopy, int taskID)
		{
			if (deviceCopy.IsPresentTask(taskID))
			{
				DeviceTask task = (DeviceTask)deviceCopy.GetTaskByID(taskID);

				if(task.IsNew)
				{
					(deviceCopy as SwitchingDevice).RemoveTask(task);
				}
				else
				{
					task.IsDelete = true;
				}
			}
		}

		/// <summary>
		/// Сгенерировать ID для задачи
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		private int GenerateIDForTask(ISwitchingDevice device)
		{
			int ID = 1;

			if (device.Tasks.Count() > 0)
			{
				IEnumerable<int> actualTaskIDs = device.Tasks.Where(t => t.ID > 0).OrderBy(t => t.ID).Select(t => t.ID);

				foreach (int id in actualTaskIDs)
				{
					if (ID != id)
					{
						break;
					}
					ID++;
				}

				if (ID == device.Tasks.Last().ID)
				{
					ID++;
				}
			}

			return ID;
		}
		#endregion Methods
	}
}

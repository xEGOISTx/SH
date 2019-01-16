using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RouterParser;
using SHBase;
using SHBase.DeviceBase;

namespace DevicesPresenter
{
	public class SwitchingDevice : ISwitchingDevice
	{
		private readonly Dictionary<int, IDeviceTask> _tasks = new Dictionary<int, IDeviceTask>();

		public SwitchingDevice(IPAddress iPAddress)
		{
			IP = iPAddress;
		}
		
		#region Properties
		public int ID { get; set; }

		/// <summary>
		/// Тип прошивки  
		/// </summary>
		public FirmwareType FirmwareType { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Состояние соединения
		/// </summary>
		public bool IsConnected { get; set; }

		/// <summary>
		/// Имя 
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// IP устройства
		/// </summary>
		public IPAddress IP { get; }

		/// <summary>
		/// Мак-адрес устройства
		/// </summary>
		public MacAddress Mac { get; set; }

		public IEnumerable<IDeviceTask> Tasks => _tasks.Values;

		#endregion Properties


		#region Methods
		public bool IsPresentTask(int id)
		{
			return _tasks.ContainsKey(id);
		}

		public IDeviceTask GetTaskByID(int id)
		{
			if (_tasks.ContainsKey(id))
				return _tasks[id];
			else
				return null;
		}


		/// <summary>
		/// Добавить задачу
		/// </summary>
		/// <param name="task"></param>
		public void AddTask(IDeviceTask task)
		{
			if (!_tasks.ContainsKey(task.ID))
			{
				(task as DeviceTask).OwnerIP = IP;
				_tasks.Add(task.ID, task);
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Error. Попытка добавить задачу с уже существующим ID. Device метод AddTask");
			}
		}



		/// <summary>
		/// Добавить задачи
		/// </summary>
		/// <param name="tasks"></param>
		public void AddRange(IEnumerable<IDeviceTask> tasks)
		{
			foreach (IDeviceTask task in tasks)
			{
				AddTask(task);
			}
		}

		/// <summary>
		/// Удалить задачу по ID 
		/// </summary>
		/// <param name="taskID"></param>
		public void RemoveTask(int taskID)
		{
			if (_tasks.ContainsKey(taskID))
			{
				_tasks.Remove(taskID);
			}

		}

		/// <summary>
		/// Удалить задачу
		/// </summary>
		/// <param name="task"></param>
		public void RemoveTask(IDeviceTask task)
		{
				RemoveTask(task.ID);
		}

		/// <summary>
		/// Удалить указанные задачи
		/// </summary>
		/// <param name="tasks"></param>
		public void RemoveTasks(IEnumerable<IDeviceTask> tasks)
		{
			foreach(IDeviceTask task in tasks.ToArray())
			{
				RemoveTask(task);
			}
		}

		/// <summary>
		/// Возвращает копию устройства
		/// </summary>
		/// <returns></returns>
		public ISwitchingDevice Copy()
		{
			SwitchingDevice device = new SwitchingDevice(IPAddress.Parse(IP.ToString()))
			{
				ID = ID,
				Description = Description,
				FirmwareType = FirmwareType,
				IsConnected = IsConnected,
				Mac = Mac,
				Name = Name
			};

			foreach(IDeviceTask task in Tasks)
			{
				DeviceTask taskCopy = (DeviceTask)(task as DeviceTask).Copy();
				//taskCopy.OwnerIP = device.IP;
				device.AddTask(taskCopy);
			}

			return device;
		}

		/// <summary>
		/// Очистиь список задач 
		/// </summary>
		public void ClearTasks()
		{
			_tasks.Clear();
		}


		//private void Task_ExecutionRequest(object sender, EventArgs e)
		//{
		//	OnTaskExecutionRequest(sender as IBaseDeviceTask);
		//}

		//private void OnTaskExecutionRequest(IBaseDeviceTask task)
		//{
		//	TaskExecutionRequest?.Invoke(this, new ExcecuteTaskEventArgs(task));
		//}
		#endregion Methods

	}
}

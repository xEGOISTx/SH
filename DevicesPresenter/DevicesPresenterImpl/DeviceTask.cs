using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using SHBase.DeviceBase;
using SHBase.Communication;

namespace DevicesPresenter
{
	/// <summary>
	/// Задача для устройства
	/// </summary>
	public class DeviceTask : IDeviceTask
	{
		private readonly Dictionary<byte, IActionGPIO> _actions = new Dictionary<byte, IActionGPIO>();

		#region Constructors
		public DeviceTask()
		{
			InitActions();
		}

		public DeviceTask(IEnumerable<IActionGPIO> actions)
		{
			foreach(IActionGPIO action in actions)
			{
				_actions.Add(action.PinNumber, action);
			}
		}
		#endregion Constructors


		#region Properties
		/// <summary>
		/// IP устройства которому принадлежит задача
		/// </summary>
		public IPAddress OwnerIP { get; set; }

		public int ID { get; set; }

		/// <summary>
		/// Признак задача помечена на удаление
		/// </summary>
		public bool IsDelete { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Голосовая команда
		/// </summary>
		public string VoiceCommand { get; set; }

		/// <summary>
		/// Признак новая задача
		/// </summary>
		public bool IsNew => ID < 1;

		/// <summary>
		/// Признак изменения задачи
		/// </summary>
		public bool IsChanged
		{
			get { return IsNew ? false : CheckChanges(); }
		}

		/// <summary>
		/// Действия с пинами
		/// </summary>
		public IEnumerable<IActionGPIO> Actions => _actions.Values;

		#endregion Properties


		#region Methods
		/// <summary>
		/// Возвращает копию задачи
		/// </summary>
		/// <returns></returns>
		public IDeviceTask Copy()
		{
			List<IActionGPIO> actions = new List<IActionGPIO>();

			foreach(IActionGPIO action in Actions)
			{
				actions.Add((action as ActionGPIO).Copy());
			}

			return new DeviceTask(actions)
			{
				Description = Description,
				VoiceCommand = VoiceCommand,
				ID = ID,			
			};
		}

		/// <summary>
		/// Выполнить задачу
		/// </summary>
		public async void Execute()
		{
			Communicator communicator = new Communicator();
			await communicator.SendGPIOTask(this);
		}

		public void ResetStatusChanged()
		{
			foreach(IActionGPIO action in Actions)
			{
				(action as ActionGPIO).ResetStatusChanged();
			}
		}

		/// <summary>
		/// Инициализация действий
		/// </summary>
		private void InitActions()
		{
			ActionGPIO action = new ActionGPIO(5);
			_actions.Add(action.PinNumber, action);
		}

		/// <summary>
		/// Проверить наличие изменённых действий
		/// </summary>
		/// <returns></returns>
		private bool CheckChanges()
		{
			return Actions.Any(a => (a as ActionGPIO).IsChanged);
		}
		#endregion Methods

	}
}
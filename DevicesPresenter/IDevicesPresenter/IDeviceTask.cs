using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace DevicesPresenter
{
	/// <summary>
	/// Интерфейс задачи для устройства
	/// </summary>
	public interface IDeviceTask : IGPIOActionTask<IActionGPIO>
	{
		/// <summary>
		/// Идентификатор задачи
		/// </summary>
		int ID { get; }

		/// <summary>
		/// Описание
		/// </summary>
		string Description { get; set; }

		/// <summary>
		/// Голосовая команда
		/// </summary>
		string VoiceCommand { get; set; }

		/// <summary>
		/// Признак изменения действий задачи
		/// </summary>
		bool IsChanged { get; }

		/// <summary>
		/// Признак новая задача
		/// </summary>
		bool IsNew { get; }

		/// <summary>
		/// Признак задача помечена на удаление
		/// </summary>
		bool IsDelete { get; }
	}
}
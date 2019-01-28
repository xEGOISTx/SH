using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public interface IDeviceBaseTask
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
		/// IP владельца
		/// </summary>
		IPAddress OwnerIP { get; }

		Task<bool> Execute();
	}
}

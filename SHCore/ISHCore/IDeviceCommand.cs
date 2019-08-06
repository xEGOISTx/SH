using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SH.Core
{
	/// <summary>
	/// Команда для устройства
	/// </summary>
	public interface IDeviceCommand
	{
		/// <summary>
		/// Идентификатор команды
		/// </summary>
		int ID { get; }

		/// <summary>
		/// Описание
		/// </summary>
		string Description { get;}

		/// <summary>
		/// Голосовая команда
		/// </summary>
		string VoiceCommand { get; }

		/// <summary>
		/// Выполнить команду
		/// </summary>
		void Execute();
	}
}

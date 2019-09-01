using System;
using System.Collections.Generic;
using System.Text;

namespace SH.Core
{
	public interface IEditor
	{
		/// <summary>
		/// Признак редактирования
		/// </summary>
		bool IsEditing { get; }

		/// <summary>
		/// Начать редактирование
		/// </summary>
		void StartEditing();

		/// <summary>
		/// Завершить редактирование
		/// </summary>
		/// <param name="applyCancelChanges"></param>
		void EndEditing(bool applyCancelChanges);
	}
}

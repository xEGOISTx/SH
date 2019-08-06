namespace SH.Core
{
	/// <summary>
	/// Результат выполнения какой-либо операции
	/// </summary>
	public interface IOperationResult
	{
		/// <summary>
		/// Признак успешности выполнения
		/// </summary>
		bool Success { get; }

		/// <summary>
		/// Описание ошибки по причине которой операция потерпела неудачу
		/// </summary>
		string ErrorMessage { get; }
	}
}

namespace SH.Core
{
	/// <summary>
	/// Результат операции внешнего запроса
	/// </summary>
	internal class RequestOperationResult : IRequestOperationResult
	{
		/// <summary>
		/// Ответ
		/// </summary>
		public string ResponseMessage { get; set; }

		/// <summary>
		/// Признак успешности выполнения
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// Описание ошибки по причине которой операция потерпела неудачу
		/// </summary>
		public string ErrorMessage { get; set; }
	}
}

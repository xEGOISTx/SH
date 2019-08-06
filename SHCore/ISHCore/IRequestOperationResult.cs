namespace SH.Core
{
	/// <summary>
	/// Результат операции внешнего запроса
	/// </summary>
	public interface IRequestOperationResult : IOperationResult
	{
		/// <summary>
		/// Ответ
		/// </summary>
		string ResponseMessage { get; }
	}
}

namespace DataManager
{
	public interface IDBOperationResult
	{
		bool Success { get; }

		string ErrorText { get;}
	}
}

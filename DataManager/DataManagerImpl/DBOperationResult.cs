namespace DataManager
{
	internal class DBOperationResult : IDBOperationResult
	{
		public bool Success { get; set; }

		public string ErrorText { get; set; } = null;
	}
}

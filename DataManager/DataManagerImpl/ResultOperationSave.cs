namespace DataManager
{
	internal class ResultOperationSave : DBOperationResult, IResultOperationSave
	{
		public int[] NewIDs { get; set; } = new int[0];
	}
}

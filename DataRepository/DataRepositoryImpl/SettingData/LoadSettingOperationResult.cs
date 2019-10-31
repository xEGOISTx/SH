using SH.DataPorts;

namespace SH.DataRepository
{
	internal class LoadSettingOperationResult : ILoadSettingOperationResult
	{
		public IConnectionSettings ConnectionSettings { get; set; }

		public bool Success { get; set; }

		public string ErrorMessage { get; set; }
	}
}
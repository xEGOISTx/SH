using SH.Core;

namespace SH.DataPorts
{
    public interface ILoadSettingOperationResult : IOperationResult
	{
		IConnectionSettings ConnectionSettings { get; }
	}
}

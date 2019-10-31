using SH.Core;

namespace SH.DataPorts
{
    public interface ISettingsLoader
	{
		ILoadSettingOperationResult Load();

		IOperationResult DeleteAll();

		IOperationResult Save(IConnectionSettings settings);
	}
}

using SH.Core;

namespace SH.DataPorts
{
    public interface ISettingsLoader
	{
		ILoadSettingOperationResult Load();

		IOperationResult Save(IConnectionSettings settings);
	}
}

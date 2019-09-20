namespace SH.DataPorts
{
    public interface IDataLoader
	{
		IDevicesLoader GetDevicesLoader();

		ISettingsLoader GetSettingsLoader();
	}
}

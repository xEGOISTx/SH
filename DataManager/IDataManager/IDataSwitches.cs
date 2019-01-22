namespace DataManager
{
	public interface IDataSwitches
	{
		IResultOperationSave SaveDevices(IDeviceInfo[] devices);

		IResultOperationLoad LoadDevices();

		IDBOperationResult RenameDevice(IDeviceInfo device);
	}
}

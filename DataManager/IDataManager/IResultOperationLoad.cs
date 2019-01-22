namespace DataManager
{
	public interface IResultOperationLoad : IDBOperationResult
	{
		IDeviceInfo[] DeviceInfos { get; }
	}
}

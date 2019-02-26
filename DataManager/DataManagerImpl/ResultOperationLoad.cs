namespace DataManager
{
	internal class ResultOperationLoad : DBOperationResult, IResultOperationLoad
	{
		public IDeviceInfo[] DeviceInfos { get; set; } = new DeviceInfo[0];
	}
}

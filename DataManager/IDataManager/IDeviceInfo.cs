namespace DataManager
{
	public interface IDeviceInfo
	{
		int ID { get; }

		string MacAddress { get; set; }

		int DeviceType { get; set; }

		int FirmwareType { get; set; }

		string Description { get; }
	}
}


namespace DataManager
{
	internal class Device : SHToolKit.DataManagement.IDevice
	{
		public int ID { get; set; }

		public string MacAddress { get; set; }

		public int DeviceType { get; set; }

		public int FirmwareType { get; set; }

		public string Description { get; set; } = string.Empty;
	}
}

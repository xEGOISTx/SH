namespace SH.DataPorts
{
	public interface IDeviceCommandData
	{
        int OwnerID { get; }

        int ID { get; }

		string CommandName { get; }

		string VoiceCommand { get; }

		string Description { get; }
	}
}

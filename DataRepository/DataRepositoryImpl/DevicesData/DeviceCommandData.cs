using SH.DataPorts;

namespace SH.DataRepository
{
    internal class DeviceCommandData : IDeviceCommandData
    {
        public int OwnerID { get; set; }

        public int ID { get; set; }

        public string VoiceCommand { get; set; }

        public string Description { get; set; }
    }
}

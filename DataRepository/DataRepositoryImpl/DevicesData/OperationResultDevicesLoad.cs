using SH.DataPorts;

namespace SH.DataRepository
{
    internal class OperationResultDevicesLoad : IOperationResultDevicesLoad
    {
        public IDeviceData[] Devices { get; set; }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}

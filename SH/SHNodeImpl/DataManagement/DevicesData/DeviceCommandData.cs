using System;
using System.Collections.Generic;
using System.Text;

namespace SH.DataManagement
{
	internal class DeviceCommandData : IDeviceCommandData
	{
        public int OwnerID { get; set; }

        public int ID { get; set; }

        public string CommandName { get; set; }

		public string VoiceCommand { get; set; }

		public string Description { get; set; }


    }
}

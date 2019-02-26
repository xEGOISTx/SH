using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterParser
{
	public class ParseResult
	{
		public bool Success { get; set; }

		public RDeviceInfo[] DeviceInfos { get; set; } = new RDeviceInfo[0];

		public string ErrorText { get; set; } 
	}
}

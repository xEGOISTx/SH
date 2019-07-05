﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit.DataManagement
{
	public interface IDBDevice
	{
		int ID { get; }

		string MacAddress { get; }

		int DeviceType { get;}

		int FirmwareType { get; }

		string Description { get; }
	}
}

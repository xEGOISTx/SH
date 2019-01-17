﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBase.DevicesBaseComponents
{
	public interface IDeviceBaseList<DeviceType> : BaseContainers.IContainer<int,DeviceType> //IEnumerable
		where DeviceType : IDeviceBase
	{
		//int Count { get; }

		//DeviceType GetByID(int id);

		//bool ContainsID(int id);
	}
}
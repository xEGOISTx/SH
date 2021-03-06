﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase;

namespace DevicesPresenter
{
	public interface IConnectionSettings
	{
		IConnectionParams DeviceConnParams { get; }

		IConnectionParams RouterConnParams { get; }		

		bool ChangeParamsForDevice(string ssid, string passwird);

		bool ChangeParamsForRouter(string ssid, string passwird);

		void Save();
	}
}

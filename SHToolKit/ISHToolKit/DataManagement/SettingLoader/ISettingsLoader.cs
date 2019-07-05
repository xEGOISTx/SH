﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase;

namespace SHToolKit.DataManagement
{
	public interface ISettingsLoader
	{
		ILoadSettingOperationResult Load();

		IOperationResult Save(IConnectionSettings settings);
	}
}

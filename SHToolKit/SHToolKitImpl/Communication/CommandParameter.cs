﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit.Communication
{
	public class CommandParameter
	{
		public CommandParameter(string name,string value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; set; }

		public string Value { get; set; }
	}
}
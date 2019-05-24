﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SHToolKit
{
	public interface IParseOperationResult : IOperationResult
	{
		IEnumerable<IPAddress> IPs { get; }
	}
}
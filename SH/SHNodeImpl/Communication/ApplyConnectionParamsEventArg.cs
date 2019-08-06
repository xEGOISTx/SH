using System;
using System.Collections.Generic;
using System.Text;

namespace SH.Communication
{
	internal delegate void ApplyConnectionParamsEventHandler(object sender, ApplyConnectionParamsEventArg e);
	internal class ApplyConnectionParamsEventArg
	{
		public ApplyConnectionParamsEventArg(IConnectionParams connectionParams)
		{
			ConnectionParams = connectionParams;
		}

		public IConnectionParams ConnectionParams { get; }

		public bool Cancel { get; set; }
	}
}

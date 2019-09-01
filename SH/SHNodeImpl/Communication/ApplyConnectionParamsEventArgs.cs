using System;
using System.Collections.Generic;
using System.Text;

namespace SH.Communication
{
	internal delegate void ApplyConnectionParamsEventHandler(object sender, ApplyConnectionParamsEventArgs e);
	internal class ApplyConnectionParamsEventArgs : EventArgs
	{
		public ApplyConnectionParamsEventArgs(IConnectionParams connectionParams)
		{
			ConnectionParams = connectionParams;
		}

		public IConnectionParams ConnectionParams { get; }

		public bool Cancel { get; set; }
	}
}

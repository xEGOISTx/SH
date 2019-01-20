using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
	public class DBOperationResult
	{
		public bool Success { get; set; }

		public string ErrorText { get; set; } = null;
	}
}

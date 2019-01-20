using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
	public class ResultOperationSave : DBOperationResult
	{
		public IEnumerable<int> IDs { get; set; }
	}
}

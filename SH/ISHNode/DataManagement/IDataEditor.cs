using SH.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SH.DataManagement
{
	public interface IDataEditor
	{
		IOperationResult RenameDevice(IDeviceData device);
	}
}

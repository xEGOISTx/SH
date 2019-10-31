
using SH.DataPorts;

namespace SH.DataRepository
{
	internal class ConnectionSettings : IConnectionSettings
	{
		public IParameter[] Parameters { get; set; }
	}
}

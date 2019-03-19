//using Switches;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace SHBase.DevicesBaseComponents
{
	public interface IDevicesManager : IDevicesGetter
	{
		//IConnectionSettings ConnectionSettings { get; }

		Task<bool> FindAndConnectDevicesAsync();

		Task<bool> SynchronizationWithDevicesAsync();

		//object GetDevices<DevsType>();
	}
}

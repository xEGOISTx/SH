//using Switches;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace SHToolKit.DevicesManagement
{
	public interface IDevicesManager : IDevicesGetter
	{
		Task<bool> FindAndConnectDevicesAsync();

		Task<bool> SynchronizationWithDevicesAsync();
	}
}

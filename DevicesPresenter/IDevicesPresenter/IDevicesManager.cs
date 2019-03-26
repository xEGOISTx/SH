//using Switches;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;

namespace DevicesPresenter
{
	public interface IDevicesManager : IDevicesGetter
	{
		Task<bool> FindAndConnectDevicesAsync();

		Task<bool> SynchronizationWithDevicesAsync();
	}
}

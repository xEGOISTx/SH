using Switches;
using System.Threading.Tasks;

namespace DevicesPresenter
{
	public interface IDevicesManager
	{
		IConnectionSettings ConnectionSettings { get; }

		Task<bool> FindAndConnectDevicesAsync();

		Task<bool> SynchronizationWithDevicesAsync();

		ISwitches GetSwitches();
	}
}

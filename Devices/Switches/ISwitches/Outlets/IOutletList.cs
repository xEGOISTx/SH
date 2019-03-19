using SHBase.DevicesBaseComponents;

namespace Switches
{
	public interface IOutletList : IDeviceBaseList<IOutlet>
	{
		ISwitchEditor SwitchEditor { get; }
	}
}

using SHBase.DevicesBaseComponents;

namespace Switches
{
	public interface ISwitchList : IDeviceBaseList<ISwitch>
	{
		ISwitchEditor SwitchEditor { get; }
	}
}

using SHBase;

namespace DevicesPresenter
{
	public class Loader : ILoader
	{
		public IConnectionParams LoadDeviceConnectionParams()
		{
			return new ConnectionParams() {Ssid = "Test", Password = "1234567890" };
		}

		public IConnectionParams LoadRouterConnectionParams()
		{
			return new ConnectionParams() {Ssid ="MGTS_GPON_2303", Password ="SSFR73N6" };
		}
	}
}

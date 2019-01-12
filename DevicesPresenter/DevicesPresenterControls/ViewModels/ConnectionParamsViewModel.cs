using DevicesPresenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPHelper;
using SHBase;

namespace DevicesPresenterControls.ViewModels
{
	public class ConnectionParamsViewModel : BaseViewModel
	{
		private readonly IConnectionParams _routerConnParams;
		private readonly IConnectionParams _deviceConnParams;
		private readonly IConnectionSettings _connectionSettings;

		public ConnectionParamsViewModel(IConnectionSettings connectionSettings)
		{
			_connectionSettings = connectionSettings;
			_routerConnParams = connectionSettings.RouterConnParams;
			_deviceConnParams = connectionSettings.DeviceConnParams;
		}

		#region Properties
		public string SsidForRouter
		{
			get { return _routerConnParams.Ssid; }
			set
			{
				if (_routerConnParams.Ssid != value)
				{
					_connectionSettings.ChangeParamsForRouter(value, _routerConnParams.Password);
					OnPropertyChanged(nameof(SsidForRouter));
				}
			}
		}

		public string PasswordForRouter
		{
			get { return _routerConnParams.Password; }
			set
			{
				if (_routerConnParams.Password != value)
				{
					_connectionSettings.ChangeParamsForRouter(_routerConnParams.Ssid, value);
					OnPropertyChanged(nameof(PasswordForRouter));
				}
			}
		}

		public string SsidForDevice
		{
			get { return _deviceConnParams.Ssid; }
			set
			{
				if (_deviceConnParams.Ssid != value)
				{
					_connectionSettings.ChangeParamsForDevice(value, _deviceConnParams.Password);
					OnPropertyChanged(nameof(SsidForDevice));
				}
			}
		}

		public string PasswordForDevice
		{
			get { return _deviceConnParams.Password; }
			set
			{
				if (_deviceConnParams.Password != value)
				{
					_connectionSettings.ChangeParamsForDevice(_deviceConnParams.Ssid, value);
					OnPropertyChanged(nameof(PasswordForDevice));
				}
			}
		}
		#endregion Properties
	}
}

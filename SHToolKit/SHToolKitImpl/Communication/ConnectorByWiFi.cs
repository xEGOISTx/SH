/*===================================================
 * Класс предназначен для поиска устройств как точек
 * доступа и осуществления соединения с ними
 ==================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using Windows.Security.Credentials;

namespace SHToolKit.Communication
{
	/// <summary>
	/// Класс для поиска устройств и соединения с ними
	/// </summary>
	public class ConnectorByWiFi
	{
		private  WiFiAdapter _wifiAdapter;
		private static WiFiAvailableNetwork _currentWiFi;
		private static PasswordCredential _currentCredential;


		#region Methods
		/// <summary>
		/// Возвращает первый найденный WiFi адаптер
		/// </summary>
		/// <returns></returns>
		internal async Task<WiFiAdapter> GetWiFiAdapter()
		{
			if (_wifiAdapter == null)
			{
				await InitializeFirstAdapter();
			}

			return _wifiAdapter;
		}

		internal async Task<bool> Reconnect()
		{
			if (_currentWiFi != null && _currentCredential != null)
				return await ConnectToDeviceAsync(_currentWiFi, _currentCredential.Password);
			else
				return false;
		}

		/// <summary>
		/// Возвращает список доступных устройств
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<WiFiAvailableNetwork>> GetAvailableDevicesAsAPAsync()
		{
			bool initRes = await InitializeFirstAdapter();

			if (initRes)
			{
				await _wifiAdapter.ScanAsync();

				return await Task<IEnumerable<WiFiAvailableNetwork>>.Factory.StartNew(() =>
				{
					return _wifiAdapter.NetworkReport.AvailableNetworks.Where(dev => dev.Ssid.IndexOf(Consts.ESP) != -1);
				});
			}

			return new List<WiFiAvailableNetwork>();
		}

		/// <summary>
		/// Подключится к устройству
		/// </summary>
		/// <param name="wiFi">Устройсво как точка доступа</param>
		/// <param name="credential">Пароль</param>
		/// <returns></returns>
		public async Task<bool> ConnectToDeviceAsync(WiFiAvailableNetwork wiFi, string devPassword)
		{
			_currentWiFi = wiFi;
			_currentCredential = new PasswordCredential { Password = devPassword };

			WiFiConnectionResult conResult;
			bool initRes = await InitializeFirstAdapter();

			if (initRes)
			{
				do
				{
					conResult = await _wifiAdapter.ConnectAsync(wiFi, WiFiReconnectionKind.Manual, _currentCredential);

					if (conResult.ConnectionStatus != WiFiConnectionStatus.Success)
					{
						System.Diagnostics.Debug.WriteLine("Connection failed!");
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("Connection success!");
					}
				}
				while (conResult.ConnectionStatus != WiFiConnectionStatus.Success);

				return true;
			}
			return false;
		}

		/// <summary>
		/// Отключиться от устройства
		/// </summary>
		public async Task DisconnectAsync()
		{
			await Task.Run(() =>
			{
				if (_wifiAdapter != null)
				{
					_wifiAdapter.Disconnect();

				}
			});

			_currentWiFi = null;
			_currentCredential = null;
		}

		/// <summary>
		/// Инициализация адаптера
		/// </summary>
		/// <returns></returns>
		private  async Task<bool> InitializeFirstAdapter()
		{
			return await Task.Run(async () =>
			{
				if (_wifiAdapter == null)
				{
					bool res = false;
					WiFiAccessStatus access = await WiFiAdapter.RequestAccessAsync();

					if (access != WiFiAccessStatus.Allowed)
					{
						throw new Exception("WiFiAccessStatus not allowed");
					}
					else
					{
						var wifiAdapterResults = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());

						if (wifiAdapterResults.Count >= 1)
						{
							_wifiAdapter = await WiFiAdapter.FromIdAsync(wifiAdapterResults[0].Id);
							res = true;
						}
						else
						{
							throw new Exception("WiFi Adapter not found.");
						}
					}
					return res;
				}
				else
				{
					return true;
				}
			});
		}
		#endregion Methods
	}
}

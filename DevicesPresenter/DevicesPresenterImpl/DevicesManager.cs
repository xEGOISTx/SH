using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RouterParser;
using SHBase.Communication;
using SHBase.DevicesBaseComponents;
using Switches;
using Windows.Devices.WiFi;
using Windows.Security.Credentials;

namespace DevicesPresenter
{
	public class DevicesManager : IDevicesManager
	{
		private readonly DeviceCommonList _deviceCommonList;
		private readonly IPAddress _aPDefaultIP = IPAddress.Parse("192.168.4.1");

		public DevicesManager(DeviceCommonList deviceCommonList)
		{
			_deviceCommonList = deviceCommonList;

			ConnectionSettings settings = new ConnectionSettings();
			settings.Load();
			ConnectionSettings = settings;
		}


		public IConnectionSettings ConnectionSettings { get; }

		public async Task<bool> FindAndConnectDevicesAsync()
		{
			ConnectorByWiFi connector = new ConnectorByWiFi();
			CommunicatorAP communicatorAP = new CommunicatorAP(_aPDefaultIP);

			//получаем доступные устройства
			IEnumerable<WiFiAvailableNetwork> wifiAvailableDevices = await connector.GetAvailableDevicesAsAPAsync();

			List<DeviceBase> newDevices = new List<DeviceBase>();

			foreach (WiFiAvailableNetwork wifiDevice in wifiAvailableDevices)
			{
				//подключаемся к устройству
				bool connRes = await connector.ConnectToDeviceAsync(wifiDevice, new PasswordCredential { Password = ConnectionSettings.DeviceConnParams.Password });
				if (connRes)
				{
					//получаем инфу устройства из него самого
					DeviceBase deviceInfo = await communicatorAP.GetDeviceInfoFromDeviceAsAP();
					if (deviceInfo != null && deviceInfo.ID == 0)
					{
						//отправляем параметры для подключения к роутеру
						bool postRes = await communicatorAP.SendConnectionParamsToDeviceAsAP(ConnectionSettings.RouterConnParams);
						if (postRes)
						{
							//запрашиваем ip
							IPAddress iP = await communicatorAP.GetLocalIPFromDeviceAsAP();
							if (iP != null)
							{
								deviceInfo.IP = iP;
								deviceInfo.IsConnected = true;

								newDevices.Add(deviceInfo);
							}
						}
					}
				}
			}

			//добавляем и сохраняем новые устройства. Каждый список сам определит принадлежащие ему устройства
			foreach (Devices devList in _deviceCommonList)
			{
				await devList.AddAndSaveNewDevices(newDevices);
			}

			return true;
		}

		public async Task<bool> SynchronizationWithDevicesAsync()
		{
			bool result = false;

			Parser parser = new Parser("http://192.168.1.254/", "admin", "admin");
			ParseResult pResult = await parser.LoadDeviceInfosAsync();

			if (pResult.Success)
			{
				result = await SynchronizeAsync(pResult.DeviceInfos);
			}

			return result;
		}


		public ISwitches GetSwitches()
		{
			return _deviceCommonList.GetDeviceList<Switches.Switches>();
		}

		public async Task<bool> LoadDevicesAsync()
		{
			foreach(Devices devList in _deviceCommonList)
			{
				await devList.Load();
			}

			return true;
		}


		//TODO:пока что всегда возвращаем true. В будущем определить другой исход
		private async Task<bool> SynchronizeAsync(IEnumerable<RDeviceInfo> deviceInfos)
		{
			return await Task.Run(async () =>
			{
				List<IDeviceBase> devices = new List<IDeviceBase>();

				//получаем IPs устройств подключённых к роутеру и по IP получаем базовую инфу с устройств
				foreach (RDeviceInfo deviceInfo in deviceInfos)
				{
					Communicator communicator = new Communicator();
					DeviceBase deviceBaseInfo = (DeviceBase)await communicator.GetDeviceInfo(deviceInfo.Ip);

					if (deviceBaseInfo != null && deviceBaseInfo.ID > 0)
					{
						deviceBaseInfo.IsConnected = deviceInfo.IsConnected;
						devices.Add(deviceBaseInfo);		
					}
				}

				//передаём спискам устройсва. Каждый список сам определит принадлежащие ему устройства и синхронизируется
				foreach (Devices devList in _deviceCommonList)
				{
					await devList.Synchronization(devices);
				}

				return true;
			});
		}

	}
}

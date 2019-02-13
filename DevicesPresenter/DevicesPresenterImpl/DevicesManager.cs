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
						//отправляем параметры для подключения к роутеру и ждём пока подключится
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
			Dictionary<int,IDeviceBase> notConnectedDevices = new Dictionary<int,IDeviceBase>();
			Communicator communicator = new Communicator();

			//ищем не соединённые устройства
			foreach (Devices devices in _deviceCommonList)
			{			
				foreach (IDeviceBase device in devices)
				{
					bool res = await communicator.CheckConnection(device);
					if (!res)
					{
						(device as DeviceBase).IsConnected = false;
						notConnectedDevices.Add(device.ID, device);
					}
				}
			}

			if (notConnectedDevices.Any())
			{
				result = await SynchronizeAsync(notConnectedDevices, communicator);	
			}
			else
			{
				result = true;
			}

			return result;
		}

		/// <summary>
		/// Возвращает колекцию списков переключателей
		/// </summary>
		/// <returns></returns>
		public ISwitches GetSwitches()
		{
			return _deviceCommonList.GetDeviceList<Switches.Switches>();
		}

		/// <summary>
		/// Загрузка устройств
		/// </summary>
		/// <returns></returns>
		public async Task<bool> LoadDevicesAsync()
		{
			foreach(Devices devList in _deviceCommonList)
			{
				await devList.Load();
			}

			return true;
		}


		//TODO:пока что всегда возвращаем true. В будущем определить другой исход
		private async Task<bool> SynchronizeAsync(IReadOnlyDictionary<int,IDeviceBase> devices, Communicator communicator)
		{
			List<IDeviceBase> toSynchronize = new List<IDeviceBase>();

			Parser parser = new Parser("http://192.168.1.254/", "admin", "admin");
			ParseResult pResult = await parser.LoadDeviceInfosAsync();

			return await Task.Run(async () =>
			{
				if (pResult.Success)
				{
					foreach (RDeviceInfo rDeviceInfo in pResult.DeviceInfos)
					{
						GetBaseInfoResult infoResult = await communicator.GetDeviceInfo(rDeviceInfo.Ip);

						if (infoResult.Success && devices.Any(d => d.Key == infoResult.BasicInfo.ID))
						{
							DeviceBase deviceInfo = new DeviceBase(infoResult.BasicInfo.IP)
							{
								ID = infoResult.BasicInfo.ID,
								IP = infoResult.BasicInfo.IP,
								Mac = infoResult.BasicInfo.Mac,
								DeviceType = infoResult.BasicInfo.DeviceType,
								FirmwareType = infoResult.BasicInfo.FirmwareType,
								Name = infoResult.BasicInfo.Name
							};

							toSynchronize.Add(deviceInfo);
						}
					}

					foreach (Devices devList in _deviceCommonList)
					{
						await devList.Synchronization(toSynchronize);
					}
				}

				return true;
			});
		}
	}
}

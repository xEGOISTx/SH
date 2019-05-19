using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;
using Windows.Devices.WiFi;
using Windows.Security.Credentials;
using SHBase;
using SHToolKit.Communication;

namespace SHToolKit.DevicesManagement
{
	public class DevicesManager : IDevicesManager
	{
		private readonly IPAddress _aPDefaultIP = IPAddress.Parse("192.168.4.1");
		private readonly Dictionary<int, DeviceBaseList> DevicesLists = new Dictionary<int, DeviceBaseList>();
		private readonly IPAddress routerIP = IPAddress.Parse("192.168.1.254");
		private readonly IRouterParser _routerParser;

		public DevicesManager() : this(new RouterParser())
		{

		}

		public DevicesManager(IRouterParser routerParser)
		{
			_routerParser = routerParser;

			ConnectionSettings settings = new ConnectionSettings();
			settings.Load();
			ConnectionSettings = settings;
		}




		public IConnectionSettings ConnectionSettings { get; }

		public void AddForManagement(DeviceBaseList devices)
		{
			if(!DevicesLists.ContainsKey(devices.DevicesType))
			{
				DevicesLists.Add(devices.DevicesType, devices);
			}
			else
			{
				throw new Exception($"{nameof(DevicesManager)}. Попытка добывить уже имеющийся тип утройств: {devices.DevicesType}");
			}
		}

		public async Task<bool> FindAndConnectDevicesAsync()
		{
			ConnectorByWiFi connector = new ConnectorByWiFi();
			CommunicatorAP communicatorAP = new CommunicatorAP(_aPDefaultIP);

			//получаем доступные устройства
			IEnumerable<WiFiAvailableNetwork> wifiAvailableDevices = await connector.GetAvailableDevicesAsAPAsync();

			Dictionary<int, List<IDeviceBase>> newDevices = new Dictionary<int, List<IDeviceBase>>();

			foreach (WiFiAvailableNetwork wifiDevice in wifiAvailableDevices)
			{
				//подключаемся к устройству
				bool connRes = await connector.ConnectToDeviceAsync(wifiDevice, new PasswordCredential { Password = ConnectionSettings.DeviceConnParams.Password });
				if (connRes)
				{		
					//получаем id
					int id = await communicatorAP.GetDeviceIDAsAP();
					if (id == 0)
					{
						//отправляем параметры для подключения к роутеру и ждём пока подключится
						bool postRes = await communicatorAP.SendConnectionParamsToDeviceAsAP(ConnectionSettings.RouterConnParams);
						if (postRes)
						{
							IDeviceBase deviceInfo = await communicatorAP.GetDeviceInfoFromDeviceAsAP();
							if(deviceInfo != null)
							{
								if(!newDevices.ContainsKey(deviceInfo.DeviceType))
								{
									newDevices.Add(deviceInfo.DeviceType, new List<IDeviceBase> { deviceInfo });
								}
								else
								{
									newDevices[deviceInfo.DeviceType].Add(deviceInfo);
								}
							}
						}

					}

					await connector.DisconnectAsync();
				}
			}

			foreach(DeviceBaseList devices in DevicesLists.Values)
			{
				if(newDevices.ContainsKey(devices.DevicesType))
				{
					List<IDeviceBase> newDevs = newDevices[devices.DevicesType];

					//сохраняем устройства в БД
					int[] newIDs = await devices.Save(newDevs);


					if (newIDs.Count() == newDevs.Count)
					{
						//создаём и добавляем устройства
						for (int i = 0; i < newDevs.Count; i++)
						{
							IDeviceBase nDev = newDevs[i];
							int nID = newIDs[i];

							IDeviceBase dev = devices.CreateDevice(nID, nDev.Name, nDev.IP, nDev.FirmwareType, nDev.Mac);

							//отправляем устройству его ID
							IOperationResult result = await communicatorAP.SendIdToDevice(nID, nDev.IP);

							if(!result.Success)
							{
								throw new Exception("Сбой отправки ID устройству: " + result.ErrorMessage);
							}

							devices.Add(dev);
						}
					}
					else
					{
						throw new Exception("Колл-во IDs не сответствует кол-ву переданных устройств");
					}

				}
			}

			return true;
		}

		public async Task<bool> SynchronizationWithDevicesAsync()
		{
			bool result = false;
			Dictionary<int, IDeviceBase> notConnectedDevices = new Dictionary<int, IDeviceBase>();
			Communicator communicator = new Communicator();

			//ищем не соединённые устройства
			foreach(DeviceBaseList devices in DevicesLists.Values)
			{
				IEnumerable<IDeviceBase> notConnDevs = await devices.GetNotConnectedDevicesAsync();

				foreach(IDeviceBase device in notConnDevs)
				{
					notConnectedDevices.Add(device.ID, device);
				}
			}

			if (notConnectedDevices.Any())
			{
				Dictionary<int, List<IDeviceBase>> devsToSynchronize = new Dictionary<int, List<IDeviceBase>>();

				//получаем ip устойств подключенных к роутеру
				IParseOperationResult iPsFromRouter = await _routerParser.GetDevicesIPs(routerIP, "admin", "admin");

				await Task.Run(async () =>
				{
					//идём по устройствам подключенным к роутеру
					foreach (IPAddress devIP in iPsFromRouter.IPs)
					{
						//получаем инфу об устройстве
						IOperationGetBaseInfoResult infoResult = await communicator.GetDeviceInfo(devIP);

						// проверяем на успех получения инфы
						if (infoResult.Success)
						{
							IDeviceBase devFromRouter = infoResult.BasicInfo;

							//удостоверяемся, что это устойство было определенно как не подключенное
							if (notConnectedDevices.ContainsKey(devFromRouter.ID))
							{
								if (!devsToSynchronize.ContainsKey(devFromRouter.DeviceType))
								{
									devsToSynchronize.Add(devFromRouter.DeviceType, new List<IDeviceBase> { devFromRouter });
								}
								else
								{
									devsToSynchronize[devFromRouter.DeviceType].Add(devFromRouter);
								}
							}
						}
					}

					//синхронизируем
					if (devsToSynchronize.Any())
					{
						foreach (DeviceBaseList devices in DevicesLists.Values)
						{
							if (devsToSynchronize.ContainsKey(devices.DevicesType))
							{
								IEnumerable<IDeviceBase> devsFromRouter = devsToSynchronize[devices.DevicesType];
								await devices.Synchronization(devsFromRouter);
							}
						}
					}
					return true;
				});
			}
			else
			{
				result = true;
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object GetDevices<DevsType>()
		{
			return DevicesLists.Values.Where(dL =>  dL is  DevsType).FirstOrDefault();
		}

		/// <summary>
		/// Загрузка устройств
		/// </summary>
		/// <returns></returns>
		public async Task<bool> LoadDevicesAsync()
		{
			foreach(DeviceBaseList devices in DevicesLists.Values)
			{
				await devices.Load();
			}

			return true;
		}
	}
}

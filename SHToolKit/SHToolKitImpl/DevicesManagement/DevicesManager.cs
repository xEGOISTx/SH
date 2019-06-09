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
		private readonly Dictionary<int, DeviceBaseList> _devicesLists = new Dictionary<int, DeviceBaseList>();
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
			if (!_devicesLists.ContainsKey(devices.DevicesType))
			{
				_devicesLists.Add(devices.DevicesType, devices);
			}
			else
			{
				throw new Exception($"{nameof(DevicesManager)}. Попытка добывить уже имеющийся тип утройств: {devices.DevicesType}");
			}
		}

		public async Task<bool> FindAndConnectDevicesAsync()
		{
			ConnectorByWiFi connector = new ConnectorByWiFi();
			//CommunicatorAP communicatorAP = new CommunicatorAP();
			Communicator communicator = new Communicator();

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
					int id = await communicator.GetDeviceID(_aPDefaultIP);
					if (id == 0)
					{
						//отправляем параметры для подключения к роутеру и ждём пока подключится
						IOperationResult postRes = await communicator.SendConnectionParamsToDevice(_aPDefaultIP, ConnectionSettings.RouterConnParams);
						if (postRes.Success)
						{
							IOperationGetBaseInfoResult result = await communicator.GetDeviceInfo(_aPDefaultIP, true);
							if (result.Success)
							{
								IDeviceBase deviceInfo = result.BasicInfo;

								if (!newDevices.ContainsKey(deviceInfo.DeviceType))
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

			foreach (DeviceBaseList devices in _devicesLists.Values)
			{
				if (newDevices.ContainsKey(devices.DevicesType))
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
							IOperationResult result = await communicator.SendIdToDevice(nID, nDev.IP);

							if (!result.Success)
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
			Communicator communicator = new Communicator();

			try
			{
				//ищем не соединённые устройства и устройсва изменившие состояние подключения
				var found = await FindNotConnectionDevices(communicator);

				//обновляем состояния подключения устройств в списках устройств
				RefreshDevicesConnectionState(found.connectionChangesInfos);

				//пробуем получить инфу с роутера о соответствующих устройствах если они подключены.
				//если у не подкл. устройств IP = 0.0.0.0
				//или у не подкл. устройств IP был изменён
				//данный шаг вернёт соответствующте устройства с верным для них IP
				Dictionary<int, List<IDeviceBase>> devsToSynchronize = await FindCorrespDevsAtRouterIfConn(_routerParser, communicator, found.notConnectedDevices);

				//синхронизируем
				if (devsToSynchronize.Any())
				{
					await Task.Run(async () =>
					{
						if (devsToSynchronize.Any())
						{
							foreach (DeviceBaseList devices in _devicesLists.Values)
							{
								if (devsToSynchronize.ContainsKey(devices.DevicesType))
								{
									IEnumerable<IDeviceBase> devsFromRouter = devsToSynchronize[devices.DevicesType];
									await devices.Synchronization(devsFromRouter);
								}
							}
						}
					});
				}

				result = true;
			}
			catch(Exception e)
			{
				//TODO: создать логер
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object GetDevices<DevsType>()
		{
			return _devicesLists.Values.Where(dL => dL is DevsType).FirstOrDefault();
		}

		/// <summary>
		/// Загрузка устройств
		/// </summary>
		/// <returns></returns>
		public async Task<bool> LoadDevicesAsync()
		{
			foreach (DeviceBaseList devices in _devicesLists.Values)
			{
				await devices.Load();
			}

			return true;
		}

		private async Task<(Dictionary<int, IDeviceBase> notConnectedDevices, Dictionary<int, List<DeviceConnectionInfo>> connectionChangesInfos)> FindNotConnectionDevices(ICommunicator communicator)
		{
			//словарь для не подключеных устройств
			Dictionary<int, IDeviceBase> notConnectedDevices = new Dictionary<int, IDeviceBase>();

			//словарь для устойств изменивших своё состояние подключения на момент проверки.
			//int = тип устройств, List<DeviceConnectionInfo> = список инфы по каждому устройсту о состоянии подкючения
			Dictionary<int, List<DeviceConnectionInfo>> connectionInfos = new Dictionary<int, List<DeviceConnectionInfo>>();

			//поиск
			foreach (DeviceBaseList devices in _devicesLists.Values)
			{
				List<DeviceConnectionInfo> connectionInfosList = new List<DeviceConnectionInfo>();

				foreach (IDeviceBase device in devices)
				{
					//получаем состояние подключения
					bool isConnected = await communicator.CheckConnection(device);

					//если состояние изменилось, запоминаем текущее действительное состояние
					if (device.IsConnected != isConnected)
					{
						connectionInfosList.Add(new DeviceConnectionInfo(device, isConnected));
					}

					if (!isConnected)
					{
						notConnectedDevices.Add(device.ID, device);
					}
				}

				//запоминаем текущее действительное состояние всех устройсв заданного типа
				connectionInfos.Add(devices.DevicesType, connectionInfosList);
			}

			return (notConnectedDevices, connectionInfos);
		}

		private void RefreshDevicesConnectionState(Dictionary<int, List<DeviceConnectionInfo>> connectionInfos)
		{
			foreach (var pair in connectionInfos)
			{
				int devicesType = pair.Key;
				List<DeviceConnectionInfo> connectionInfoList = pair.Value;

				_devicesLists[devicesType].RefreshDevicesConnectionState(connectionInfoList);
			}
		}

		/// <summary>
		///  Возвращает соответствующие устройства на маршрутизаторе, если по факту они подключены
		/// </summary>
		/// <param name="parser"></param>
		/// <param name="notConnectedDevices"></param>
		/// <param name="communicator"></param>
		/// <returns></returns>
		private async Task<Dictionary<int, List<IDeviceBase>>> FindCorrespDevsAtRouterIfConn(IRouterParser parser, ICommunicator communicator, Dictionary<int, IDeviceBase> notConnectedDevices)
		{
			Dictionary<int, List<IDeviceBase>> devsToSynchronize = new Dictionary<int, List<IDeviceBase>>();

			if (notConnectedDevices.Any())
			{				
				//получаем ip устойств подключенных к роутеру
				IParseOperationResult iPsFromRouter = await parser.GetDevicesIPs(routerIP, "admin", "admin");

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
				});
			}

			return devsToSynchronize;
		}
	}
}

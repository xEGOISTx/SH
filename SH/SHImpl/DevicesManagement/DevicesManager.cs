using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SHBase.DevicesBaseComponents;
using Windows.Devices.WiFi;
using Windows.Security.Credentials;
using SHBase;
using SHToolKit;
using SHToolKit.Communication;
using SHToolKit.DevicesManagement;
using SHToolKit.DataManagement;

namespace SH
{
	internal class DevicesManager : IDevicesManager
	{
		private readonly Dictionary<int, DeviceBaseList> _devicesLists = new Dictionary<int, DeviceBaseList>();
		private ConnectionSettings _connectionSettings;
		private bool _devicesIsLoad;

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


		/// <summary>
		/// Найти и подключить новые устройства к роутеру
		/// </summary>
		/// <param name="devicesFinder">Средство для поиска утройств</param>
		/// <returns></returns>
		public async Task<IFindDevicesOperationResult> FindAndConnectToRouterNewDevicesAsync(IDevicesFinder devicesFinder)
		{
			return await devicesFinder.FindAndConnectNewDevicesToRouterAsync(
				_connectionSettings.GetConnectionParamsToRouterAsAP(),
				_connectionSettings.DeviceDafaultIP,
				_connectionSettings.DeviceWiFiPassword);
		}

		/// <summary>
		/// Сохраняет устройства в хранилище и распределяет по соответствующим спискам устройств
		/// </summary>
		/// <param name="findDevicesResult">Резултат поиска новых устройств</param>
		/// <param name="loader">Загрузчик. Используется для сохранения устройств в хранилище</param>
		/// <param name="communicator">Средство общения с устройствами</param>
		/// <returns></returns>
		public async Task<IOperationResult> SaveAndDistributeNewDevices(IFindDevicesOperationResult findDevicesResult, IDevicesLoader loader, ICommunicator communicator)
		{
			IOperationResult result = new OperationResult { Success = true };

			var (iDs, saveResult) = await SaveDivices(findDevicesResult, loader);

			if(saveResult.Success)
			{
				IOperationResult result1 = await DistributeNewDevices(iDs, findDevicesResult, communicator);

				if(!result1.Success)
				{
					result = result1;
				}
			}
			else
			{
				result = saveResult;
			}

			return result;
		}


		/// <summary>
		/// Возвращает неподключенные устройства и информацию о подключении устройств измеривших своё состояние
		/// </summary>
		/// <param name="devicesFinder">Средство для поиска утройств</param>
		/// <returns></returns>
		public async Task<IDevicesConnectionInfo> GetDevicesConnectionInfo(IDevicesFinder devicesFinder)
		{
			List<IDeviceBase> devices = new List<IDeviceBase>();

			foreach (DeviceBaseList deviceList in _devicesLists.Values)
			{
				foreach (IDeviceBase device in deviceList)
				{
					devices.Add(device);
				}
			}

			return await devicesFinder.FindNotConnectedDevices(devices);
		}

		/// <summary>
		/// Обновить состояния подключения устройств
		/// </summary>
		/// <param name="devsConnInfo">Информация о состоянии подключения устройств</param>
		public void RefreshDevicesConnectionState(IDevicesConnectionInfo devsConnInfo)
		{
			foreach (var pair in devsConnInfo.GetСonnectionChangesInfo())
			{
				int devicesType = pair.Key;
				IEnumerable<IDeviceConnectionInfo> connectionInfoList = pair.Value;

				_devicesLists[devicesType].RefreshDevicesConnectionState(connectionInfoList);
			}
		}

		/// <summary>
		/// Попытка найти соответствующие устройства на роутере если они подключены
		/// </summary>
		/// <param name="devicesFinder">Средство для поиска утройств</param>
		/// <param name="devsConnInfo">Информация о состоянии подключения устройств</param>
		/// <returns></returns>
		public async Task<IFindDevicesOperationResult> FindDevicesAtRouterIfItsConn(IDevicesFinder devicesFinder, IDevicesConnectionInfo devsConnInfo)
		{
			return await devicesFinder.FindDevicesAtRouterIfItsConn(devsConnInfo, _connectionSettings.GetRouterCredentials());
		}

		/// <summary>
		/// Синхронизация устройств с найденными устройствами на роутере
		/// </summary>
		/// <param name="devsFromRouterResult"></param>
		/// <returns></returns>
		public async Task<IOperationResult> DevicesSynchronization(IFindDevicesOperationResult devsFromRouterResult)
		{
			var foundDevs = devsFromRouterResult.FoundDevices;
			OperationResult result = new OperationResult { Success = true };

			try
			{
				if (foundDevs.Any())
				{
					foreach (DeviceBaseList devices in _devicesLists.Values)
					{
						if (foundDevs.ContainsKey(devices.DevicesType))
						{
							IEnumerable<IDeviceBase> devsFromRouter = foundDevs[devices.DevicesType];
							await devices.Synchronization(devsFromRouter);
						}
					}
				}
			}
			catch(Exception ex)
			{
				result.Success = false;
				result.ErrorMessage = ex.Message;
			}

			return result;
		}

		public async Task<IOperationResult> LoadDataFromRepository(IDataLoader loader)
		{
				return await Task.Run(() =>
				{
					if (!_devicesIsLoad)
					{
						IOperationResult devLoadRes = LoadDevicesAsync(loader.GetDevicesLoader());

						if (!devLoadRes.Success)
						{
							return devLoadRes;
						}

						//TODO: реализовать загрузчик
						LoadSetting(/*loader.GetSettingsLoader()*/);

						_devicesIsLoad = true;
					}

					return new OperationResult { Success = true };
				});
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object GetDevices<DevsType>()
		{
			return _devicesLists.Values.Where(dL => dL is DevsType).FirstOrDefault();
		}

		private Task<(Dictionary<int, int[]> iDs, IOperationResult saveResult)> SaveDivices(IFindDevicesOperationResult findDevicesResult, IDevicesLoader loader)
		{
			return Task.Run(() =>
			{
				Dictionary<int, int[]> savedDevs = new Dictionary<int, int[]>();
				IOperationResult result = new OperationResult { Success = true };

				foreach (var pair in findDevicesResult.FoundDevices)
				{
					int devsType = pair.Key;
					IEnumerable<IDeviceBase> devsToSave = pair.Value;

					IDBDevice[] dBItems = MakeDBItems(devsToSave);
					IOperationResultSaveDevices saveResult = loader.SaveDevices(dBItems);

					if (saveResult.Success)
					{
						savedDevs.Add(devsType, saveResult.DevicesIDs);
					}
					else
					{
						result = saveResult;
						savedDevs = null;
					}
				}
				return (savedDevs, result);
			});
		}

		private async Task<IOperationResult> DistributeNewDevices(Dictionary<int, int[]> iDs, IFindDevicesOperationResult findDevicesResult, ICommunicator communicator)
		{
			OperationResult result = new OperationResult { Success = true };

			await Task.Run(async () =>
			{
				//Dictionary<int, List<IDeviceBase>> filledDevices = new Dictionary<int, List<IDeviceBase>>(iDs.Count);
				try
				{
					foreach (DeviceBaseList baseList in _devicesLists.Values)
					{
						int devsType = baseList.DevicesType;
						int[] newIDs = iDs[devsType];
						IDeviceBase[] newDevs = findDevicesResult.FoundDevices[devsType].ToArray();

						List<IDeviceBase> devicesToAdd = new List<IDeviceBase>(newIDs.Length);

						for (int i = 0; i < newIDs.Length; i++)
						{
							int nID = newIDs[i];
							IDeviceBase nDev = newDevs[i];

							//отправляем устройству его ID
							IOperationResult sendResult = await communicator.SendIdToDevice(nID, nDev.IP);


							IDeviceBase dev = baseList.CreateDevice
							(
								nID,
								nDev.Name,
								string.Empty,
								nDev.IP,
								nDev.FirmwareType,
								nDev.Mac
							);

							baseList.Add(dev);
						}
					}
				}
				catch(Exception ex)
				{
					result.Success = false;
					result.ErrorMessage = ex.Message;
				}

			});

			return result;
		}

		/// <summary>
		/// Загрузка устройств
		/// </summary>
		/// <returns></returns>
		private IOperationResult LoadDevicesAsync(IDevicesLoader loader)
		{
			OperationResult result = new OperationResult { Success = true };

			foreach (DeviceBaseList devices in _devicesLists.Values)
			{
				IOperationResultDevicesLoad loadResult = loader.LoadDevices(devices.DevicesType);

				try
				{
					if (loadResult.Success)
					{
						foreach (IDBDevice dBItem in loadResult.Devices)
						{
							IDeviceBase device = devices.CreateDevice
							(
								dBItem.ID,
								string.Empty,
								dBItem.Description,
								IPAddress.Parse("0.0.0.0"),
								(FirmwareType)dBItem.FirmwareType,
								new MacAddress(dBItem.MacAddress)
							);

							devices.Add(device);
						}
					}
					else
					{
						result.Success = false;
						result.ErrorMessage = $"Ошибка загрузки устройств типа {devices.DevicesType}: {loadResult.ErrorMessage}";
						break;
					}
				}
				catch(Exception ex)
				{
					result.Success = false;
					result.ErrorMessage = ex.Message;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Загрузка настроек
		/// </summary>
		/// <param name="loader"></param>
		/// <returns></returns>
		private bool LoadSetting(ISettingsLoader loader = null)
		{
			//TODO: заглушка
			_connectionSettings = new ConnectionSettings()
			{
				RouterIP = IPAddress.Parse("192.168.1.254"),
				RouterLogin = "admin",
				RouterPassword = "admin",
				RouterSsid = "MGTS_GPON_2303",
				RouterWiFiPassword = "SSFR73N6",
				DeviceDafaultIP = IPAddress.Parse("192.168.4.1"),
				DeviceWiFiPassword = "1234567890"
			};

			return true;

			//DataInterfaces.ILoadSettingOperationResult loadRes = await settingloader.Load();

			//if(loadRes.Success)
			//{
			//	_connectionSettings = new ConnectionSettings(loadRes.ConnectionSettings);
			//}

			//return loadRes.Success;
		}

		private IDBDevice[] MakeDBItems(IEnumerable<IDeviceBase> devices)
		{
			List<IDBDevice> dBItems = new List<IDBDevice>(devices.Count());

			foreach (IDeviceBase device in devices)
			{
				dBItems.Add(new Device
				{
					Description = device.Description,
					DeviceType = device.DeviceType,
					FirmwareType = (int)device.FirmwareType,
					MacAddress = device.Mac.ToString()
				}); ;
			}

			return dBItems.ToArray();
		}
	}
}

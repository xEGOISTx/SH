using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SHBase;
using SHBase.DevicesBaseComponents;
using SH.Communication;
using Windows.Devices.WiFi;
using SHToolKit;

namespace SH.DevicesManagement
{
	internal class DevicesFinder : IDevicesFinder
	{
		private readonly Communicator _communicator;
		private readonly ConnectorByWiFi _connector = new ConnectorByWiFi();
		private readonly IRouterParser _rParser;

		internal DevicesFinder(Communicator communicator, IRouterParser routerParser = null)
		{
			_communicator = communicator;
			_rParser = routerParser ?? new RouterParser();
		}

		public async Task<IFindDevicesOperationResult> FindAndConnectNewDevicesToRouterAsync(IConnectionParamsToAP connParamsToRouter, IPAddress devDefaultIP, string devPassAsAP)
		{
			//получаем доступные устройства
			IEnumerable<WiFiAvailableNetwork> wifiAvailableDevices = await _connector.GetAvailableDevicesAsAPAsync();

			FindDevicesOperationResult result = new FindDevicesOperationResult();
			Dictionary<int, List<IDeviceBase>> newDevices = new Dictionary<int, List<IDeviceBase>>();

			try
			{
				foreach (WiFiAvailableNetwork wifiDevice in wifiAvailableDevices)
				{
					//подключаемся к устройству
					bool connRes = await _connector.ConnectToDeviceAsync(wifiDevice, devPassAsAP);
					if (connRes)
					{
						//получаем id
						//int id = await _communicator.GetDeviceID(devDefaultIP);
						IPAddress iP = await _communicator.GetLocalIPFromDeviceAsAP(devDefaultIP);
						if (iP == Consts.ZERO_IP)
						{
							//отправляем параметры для подключения к роутеру и ждём пока подключится
							IOperationResult postRes = await _communicator.SendConnectionParamsToDevice(devDefaultIP, connParamsToRouter);
							if (postRes.Success)
							{
								IOperationGetBaseInfoResult infoRes = await _communicator.GetDeviceInfo(devDefaultIP, true);
								if (infoRes.Success)
								{
									IDeviceBase deviceInfo = infoRes.BasicInfo;

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

						await _connector.DisconnectAsync();
					}
				}

				Dictionary<int, IEnumerable<IDeviceBase>> nDevs = new Dictionary<int, IEnumerable<IDeviceBase>>();

				foreach (var pair in newDevices)
				{
					nDevs.Add(pair.Key, pair.Value);
				}

				result.Success = true;
				result.FoundDevices = nDevs;
			}
			catch (Exception e)
			{
				result.ErrorMessage = e.Message;
			}

			return result;
		}

		public async Task<IFindDevicesOperationResult> FindDevicesAtRouterIfItsConn(IDevicesConnectionInfo devsConnInfo, SHBase.ICredentials routerCredentials)
		{
			Dictionary<int, List<IDeviceBase>> devsFromRouter = new Dictionary<int, List<IDeviceBase>>();
			FindDevicesOperationResult result = new FindDevicesOperationResult { Success = true };
			var notConDevs = devsConnInfo.NotConnectedDevices;

			try
			{
				if (notConDevs.Any())
				{
					//получаем ip устойств подключенных к роутеру
					IParseOperationResult iPsFromRouter = await _rParser.GetDevicesIPs(routerCredentials);

					if (iPsFromRouter.Success)
					{
						IEnumerable<IPAddress> iPs = iPsFromRouter.IPs.Select(ip => IPAddress.Parse(ip));

						await Task.Run(async () =>
						{
							//идём по устройствам подключенным к роутеру
							foreach (IPAddress devIP in iPs)
							{
								//получаем инфу об устройстве
								IOperationGetBaseInfoResult infoResult = await _communicator.GetDeviceInfo(devIP);

								// проверяем на успех получения инфы
								if (infoResult.Success)
								{
									IDeviceBase devFromRouter = infoResult.BasicInfo;

									//удостоверяемся, что это устойство было определенно как не подключенное
									if (notConDevs.ContainsKey(devFromRouter.ID))
									{
										if (!devsFromRouter.ContainsKey(devFromRouter.DeviceType))
										{
											devsFromRouter.Add(devFromRouter.DeviceType, new List<IDeviceBase> { devFromRouter });
										}
										else
										{
											devsFromRouter[devFromRouter.DeviceType].Add(devFromRouter);
										}
									}
								}
								else
								{
									result.Success = false;
									result.ErrorMessage = infoResult.ErrorMessage;
								}
							}
						});
					}
					else
					{
						result.Success = false;
						result.ErrorMessage = iPsFromRouter.ErrorMessage;
					}
				}
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.ErrorMessage = ex.Message;
			}


			if(result.Success)
			{
				Dictionary<int, IEnumerable<IDeviceBase>> temp = new Dictionary<int, IEnumerable<IDeviceBase>>(devsFromRouter.Count);

				foreach(var pair in devsFromRouter)
				{
					temp.Add(pair.Key, pair.Value);
				}

				result.FoundDevices = temp;
			}

			return result;
		}

		public async Task<IDevicesConnectionInfo> FindNotConnectedDevices(IEnumerable<IDeviceBase> devices)
		{
			DevicesConnectionInfo devicesConnectionInfo = new DevicesConnectionInfo();

			await Task.Run(async() =>
			{
				foreach (IDeviceBase device in devices)
				{
					bool isConnected = await _communicator.CheckConnection(device);

					if (device.IsConnected != isConnected)
					{
						devicesConnectionInfo.AddConnectionChanges(new DeviceConnectionInfo(device, isConnected));
					}

					if (!isConnected)
					{
						devicesConnectionInfo.AddNotConnectedDevice(device);
					}
				}
			});

			return devicesConnectionInfo;
		}
	}
}

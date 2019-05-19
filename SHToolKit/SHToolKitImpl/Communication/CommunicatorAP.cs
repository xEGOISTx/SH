/*===========================================================
 * Класс предназначен для первоначальной настройки устройств,
 * содержит методы бызового управления и настройки устройства
 * в режиме точки доступа.
 ==========================================================*/

using SHBase;
using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Windows.Devices.WiFi;

namespace SHToolKit.Communication
{
	/// <summary>
	/// Класс для общения с устройством как точкой доступа
	/// </summary>
	internal class CommunicatorAP : RequestsSender
	{
		private readonly IPAddress _ip;

		/// <summary>
		/// Инициализация класса для общения с устройством как точкой доступа
		/// </summary>
		/// <param name="deviceIpAsAP">Ip устройства как точки доступа</param>
		public CommunicatorAP(IPAddress deviceIpAsAP)
		{
			_ip = deviceIpAsAP;
		}



		#region Methods
		/// <summary>
		/// Возвращает базовую инфу об устройстве
		/// </summary>
		/// <returns></returns>
		public async Task<IDeviceBase> GetDeviceInfoFromDeviceAsAP()
		{
			return await Task.Run(async () =>
			{
				DeviceInfo device = null;

				OperationResult result = await SendToDevice(_ip, CommandName.GetInfo) as OperationResult;
				IPAddress ip = await GetLocalIPFromDeviceAsAP();

				if (result.Success)
				{
					string[] info = result.ResponseMessage.Split('&');

					device = new DeviceInfo(ip)
					{
						ID = ushort.Parse(info[0]),
						FirmwareType = (FirmwareType)int.Parse(info[1]),
						Mac = new MacAddress(info[2]),
						Name = info[3],
						DeviceType = int.Parse(info[4]),
						IsConnected = (ip != null && ip != Consts.ZERO_IP),
						Description = info[3]
					};
				}

				return device;
			});
		}

		/// <summary>
		/// Получить ID устройства
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		public async Task<int> GetDeviceIDAsAP()
		{
			return await Task.Run(async () =>
			{
				if (_ip != null && _ip != Consts.ZERO_IP)
				{
					OperationResult result = await SendToDevice(_ip, CommandName.GetID) as OperationResult;

					if (result.Success)
					{
						return ushort.Parse(result.ResponseMessage);
					}
					else
					{
						return -1;
					}
				}

				return -1;
			});

		}

		/// <summary>
		/// Возвращает ip присвоенный роутером
		/// </summary>
		/// <returns></returns>
		public async Task<IPAddress> GetLocalIPFromDeviceAsAP()
		{
			return await Task.Run(async () =>
			{
				IPAddress ip = Consts.ZERO_IP;

				OperationResult result = await SendToDevice(_ip, CommandName.GetIP) as OperationResult;

				if (result.Success)
				{
					//byte[] byteArr = BitConverter.GetBytes(int.Parse(result.ResponseMessage));
					ip = IPAddress.Parse(result.ResponseMessage);//new IPAddress(byteArr);
				}
				else
				{
					return null;
				}

				return ip;
			});
		}

		/// <summary>
		/// Отправить IP хоста устройству
		/// </summary>
		/// <param name="iP"></param>
		/// <returns></returns>
		public Task<bool> SendHostIPToDeviceAsAP(IPAddress iP)
		{
			return Task.Run(async () =>
			{
				if (_ip != null && _ip != Consts.ZERO_IP)
				{
					List<CommandParameter> content = new List<CommandParameter>(4);
					byte[] bytes = iP.GetAddressBytes();
					byte bNumber = 1;

					foreach(byte b in bytes)
					{					
						content.Add(new CommandParameter($"b{bNumber}", b.ToString()));
						bNumber++;
					}

					IOperationResult result = await SendToDevice(_ip, CommandName.SetHostIP, content);

					return result.Success;
				}
				else
				{
					return false;
				}
			});
		}

		/// <summary>
		/// Отпрпавить id устройству
		/// </summary>
		/// <param name = "id" ></ param >
		/// < returns ></ returns >
		public async Task<IOperationResult> SendIdToDevice(int id, IPAddress deviceIP)
		{
			if (deviceIP != null && deviceIP != Consts.ZERO_IP)
			{
				byte[] bytes = BitConverter.GetBytes(id);
				string byte1value = bytes[0].ToString();
				string byte2value = bytes[1].ToString();

				List<CommandParameter> content = new List<CommandParameter>
				{
					new CommandParameter("b1", byte1value),
					new CommandParameter("b2", byte2value)
				};

				IOperationResult result = await SendToDevice(deviceIP, CommandName.SetID, content);

				return result;
			}
			else
			{
				return new OperationResult { Success = false, ErrorMessage = "Не задан IP" };
			}
		}

		/// <summary>
		/// Отправить устройству параметры для подключения к роутеру
		/// </summary>
		/// <param name="connectionParams"></param>
		/// <returns></returns>
		public async Task<bool> SendConnectionParamsToDeviceAsAP(IConnectionParams connectionParams)
		{
			return await Task.Run(async() =>
			{				
				List<CommandParameter> content = new List<CommandParameter>
				{
					new CommandParameter("ssid", connectionParams.Ssid),
					new CommandParameter("password", connectionParams.Password)
				};

				
				OperationResult result = await SendToDevice(_ip, CommandName.ConnectionParams, content) as OperationResult;

				if(!result.Success)
				{
					ConnectorByWiFi connector = new ConnectorByWiFi();
					WiFiAdapter wiFiAdapter = await connector.GetWiFiAdapter();
					if (wiFiAdapter.NetworkAdapter.NetworkItem.GetNetworkTypes() == Windows.Networking.Connectivity.NetworkTypes.None)
					{
						result.Success = await connector.Reconnect();
					}
				}

				return result.Success;
			});
		}
		#endregion Methods
	}
}

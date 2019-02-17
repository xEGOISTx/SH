using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFi;

namespace SHBase.Communication
{
	/// <summary>
	/// Класс для общения с устройством как точкой доступа
	/// </summary>
	public class CommunicatorAP
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
				DeviceBase device = null;
				Communicator communicator = new Communicator();

				OperationResult result = await communicator.SendToDevice(_ip, CommandNames.GetInfo);
				IPAddress ip = await GetLocalIPFromDeviceAsAP(); 

				if (result.Success)
				{
					string[] info = result.ResponseMessage.Split('&');

					device = new DeviceBase(ip)
					{
						ID = ushort.Parse(info[0]),
						FirmwareType = (FirmwareType)int.Parse(info[1]),
						Mac = new MacAddress(info[2]),
						Name = info[3],
						DeviceType = (DeviceType)int.Parse(info[4]),
						IsConnected = true
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
					Communicator communicator = new Communicator();

					OperationResult result = await communicator.SendToDevice(_ip, CommandNames.GetID);

					if (result.Success && result.ResponseMessage != "0")
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
				Communicator communicator = new Communicator();

				OperationResult result = await communicator.SendToDevice(_ip, CommandNames.GetIP);

				if (result.Success)
				{
					byte[] byteArr = BitConverter.GetBytes(int.Parse(result.ResponseMessage));
					ip = new IPAddress(byteArr);
				}

				return ip == Consts.ZERO_IP ? null : ip;
			});
		}

		/// <summary>
		/// Отпрпавить id устройству
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<bool> SendIdToDeviceAsAP(ushort id)
		{
			if (_ip != null && _ip != Consts.ZERO_IP)
			{
				byte[] bytes = BitConverter.GetBytes(id);
				string byte1value = bytes[0].ToString();
				string byte2value = bytes[1].ToString();

				List<CommandParameter> content = new List<CommandParameter>
				{
					new CommandParameter("b1", byte1value),
					new CommandParameter("b2", byte2value)
				};

				Communicator communicator = new Communicator();
				OperationResult result = await communicator.SendToDevice(_ip, CommandNames.SetID, content);

				return result.Success;
			}
			else
			{
				return false;
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

				Communicator communicator = new Communicator();
				OperationResult result = await communicator.SendToDevice(_ip, CommandNames.ConnectionParams, content);

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

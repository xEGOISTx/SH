/*======================================================================
 * Класс содержит методы передачи/получения информации и управления уже
 * подключенным устойствам.
 ======================================================================*/

using SHBase;
using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Web.Http;

namespace SHToolKit.Communication
{
	/// <summary>
	/// Класс для общения с устройством
	/// </summary>
	public class Communicator : RequestsSender, ICommunicator
	{
		/// <summary>
		/// Отправить устройству задачу действия с пинами
		/// </summary>
		/// <typeparam name="T">Тип действия</typeparam>
		/// <param name="task">Задача</param>
		/// <returns></returns>
		public async Task<bool> SendGPIOTask<T>(IBaseGPIOActionTask<T> task)
			where T: IBaseGPIOAction
		{
			return await Task.Run(async () =>
			{
				if(task != null && task.Owner != null &&  task.Owner.IP != null && task.Owner.IP != Consts.ZERO_IP)
				{
					List<CommandParameter> content = new List<CommandParameter>();
					foreach (IBaseGPIOAction gPIOAction in task.Actions)
					{
						if (gPIOAction.Mode != GPIOMode.NotDefined && gPIOAction.Level != GPIOLevel.NotDefined)
						{
							CommandParameter commandParameter = new CommandParameter(gPIOAction.PinNumber.ToString(), $"{gPIOAction.Mode}_{gPIOAction.Level}");
							content.Add(commandParameter);
						}
					}

					if (content.Count() > 0)
					{
						IOperationResult result = await SendToDevice(task.Owner.IP, CommandName.GPIOActions, content);
						return result.Success;
					}
				}

				return false;
			});
		}

		/// <summary>
		/// Возвращает базовую инфу об устройстве
		/// </summary>
		/// <param name="deviceIP"></param>
		/// <param name="asAP">Получить инфо устройсва как точки доступа</param>
		/// <returns></returns>
		public async Task<IOperationGetBaseInfoResult> GetDeviceInfo(IPAddress deviceIP, bool asAP = false)
		{
			if(deviceIP == null)
			{
				return new GetBaseInfoResult(new OperationResult { ErrorMessage = "deviceIP не может быть null" });
			}

			return await Task.Run(async () =>
			{
				DeviceInfo deviceInfo = null;
				IPAddress curIP = deviceIP;

				OperationResult result = await SendToDevice(deviceIP, CommandName.GetInfo) as OperationResult;

				if(asAP)
				{
					curIP = await GetLocalIPFromDeviceAsAP(deviceIP);
				}

				if (result.Success && curIP != null)
				{
					string[] info = result.ResponseMessage.Split('&');

					deviceInfo = new DeviceInfo(curIP)
					{
						ID = ushort.Parse(info[0]),
						FirmwareType = (FirmwareType)int.Parse(info[1]),
						Mac = new MacAddress(info[2]),
						Name = info[3],
						DeviceType = int.Parse(info[4]),
						IsConnected = curIP != Consts.ZERO_IP
					};
				}
				else if(result.Success && curIP == null)
				{
					result.Success = false;
					result.ErrorMessage = "Не удалось получить IP";
				}

				return new GetBaseInfoResult(result) { BasicInfo = deviceInfo };
			});
		}

		/// <summary>
		/// Получить ID устройства
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		public async Task<int> GetDeviceID(IPAddress deviceIP)
		{
			return await Task.Run(async () =>
			{
				if (deviceIP != null && deviceIP != Consts.ZERO_IP)
				{
					OperationResult result = await SendToDevice(deviceIP, CommandName.GetID) as OperationResult;

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
		/// Проверить соединение с устройством
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		public async Task<bool> CheckConnection(IDeviceBase device)
		{
			if (device.IP != null)
			{
				IOperationGetBaseInfoResult getInfoResult = await GetDeviceInfo(device.IP);

				if (getInfoResult.Success &&
					getInfoResult.BasicInfo.ID == device.ID &&
					getInfoResult.BasicInfo.Mac == device.Mac &&
					getInfoResult.BasicInfo.DeviceType == device.DeviceType)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Отправить IP хоста устройству
		/// </summary>
		/// <param name="hostIP"></param>
		/// <returns></returns>
		public async Task<IOperationResult> SendHostIPToDevice(IPAddress deviceIP, IPAddress hostIP)
		{
			OperationResult result = new OperationResult();

			if (deviceIP != null && deviceIP != Consts.ZERO_IP)
			{
				List<CommandParameter> content = new List<CommandParameter>(4);
				byte[] bytes = hostIP.GetAddressBytes();
				byte bNumber = 1;

				foreach (byte b in bytes)
				{
					content.Add(new CommandParameter($"b{bNumber}", b.ToString()));
					bNumber++;
				}

				result = (OperationResult)await SendToDevice(deviceIP, CommandName.SetHostIP, content);			
			}
			else
			{
				result.Success = false;
				result.ErrorMessage = $"IP не может быть null или {Consts.ZERO_IP}";
			}

			return result;
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
		/// Отправить устройству параметры для подключения к роутеру. Устройство сразу попытается подключится к роутеру
		/// </summary>
		/// <param name="connectionParams"></param>
		/// <returns></returns>
		public async Task<IOperationResult> SendConnectionParamsToDevice(IPAddress deviceIP, IConnectionParams connectionParams)
		{
			return await Task.Run(async () =>
			{
				List<CommandParameter> content = new List<CommandParameter>
				{
					new CommandParameter("ssid", connectionParams.Ssid),
					new CommandParameter("password", connectionParams.Password)
				};


				OperationResult result = await SendToDevice(deviceIP, CommandName.ConnectionParams, content) as OperationResult;

				if (!result.Success)
				{
					ConnectorByWiFi connector = new ConnectorByWiFi();
					WiFiAdapter wiFiAdapter = await connector.GetWiFiAdapter();
					if (wiFiAdapter.NetworkAdapter.NetworkItem.GetNetworkTypes() == Windows.Networking.Connectivity.NetworkTypes.None)
					{
						result.Success = await connector.Reconnect();
					}
				}

				return result;
			});
		}

		/// <summary>
		/// Возвращает ip присвоенный роутером
		/// </summary>
		/// <returns></returns>
		private async Task<IPAddress> GetLocalIPFromDeviceAsAP(IPAddress accessPointIP)
		{
			return await Task.Run(async () =>
			{
				IPAddress ip = Consts.ZERO_IP;

				OperationResult result = await SendToDevice(accessPointIP, CommandName.GetIP) as OperationResult;

				if (result.Success)
				{
					ip = IPAddress.Parse(result.ResponseMessage);
				}
				else
				{
					return null;
				}

				return ip;
			});
		}
	}
}

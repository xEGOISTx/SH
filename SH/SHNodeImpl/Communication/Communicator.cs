/*======================================================================
 * Класс содержит методы передачи/получения информации и управления уже
 * подключенным устойствам.
 ======================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SH.Core;
using SH.Core.DevicesComponents;
using SH.Node;

namespace SH.Communication
{
	/// <summary>
	/// Класс для общения с устройством
	/// </summary>
	internal class Communicator : RequestsSender
	{
		/// <summary>
		/// Отправить устройству задачу действия с пинами
		/// </summary>
		/// <typeparam name="T">Тип действия</typeparam>
		/// <param name="task">Задача</param>
		/// <returns></returns>
		//public async Task<bool> SendGPIOTask<T>(IBaseGPIOActionTask<T> task)
		//	where T: IBaseGPIOAction
		//{
		//	return await Task.Run(async () =>
		//	{
		//		if(task != null && task.Owner != null &&  task.Owner.IP != null && task.Owner.IP != Consts.ZERO_IP)
		//		{
		//			List<CommandParameter> content = new List<CommandParameter>();
		//			foreach (IBaseGPIOAction gPIOAction in task.Actions)
		//			{
		//				if (gPIOAction.Mode != GPIOMode.NotDefined && gPIOAction.Level != GPIOLevel.NotDefined)
		//				{
		//					CommandParameter commandParameter = new CommandParameter(gPIOAction.PinNumber.ToString(), $"{gPIOAction.Mode}_{gPIOAction.Level}");
		//					content.Add(commandParameter);
		//				}
		//			}

		//			if (content.Count() > 0)
		//			{
		//				IOperationResult result = await SendToDevice(task.Owner.IP, CommandName.GPIOActions, content);
		//				return result.Success;
		//			}
		//		}

		//		return false;
		//	});
		//}

		/// <summary>
		/// Возвращает базовую инфу об устройстве
		/// </summary>
		/// <param name="deviceIP"></param>
		/// <returns></returns>
		public async Task<GetBaseInfoOperationResult> GetDeviceInfo(IPAddress deviceIP/*, bool asAP = false*/)
		{
			GetBaseInfoOperationResult result = new GetBaseInfoOperationResult();

			if(deviceIP == null)
			{
				result.ErrorMessage = "deviceIP не может быть null";
				return result;
			}

			return await Task.Run(async () =>
			{  
				DeviceInfo deviceInfo = null;
				//IPAddress curIP = deviceIP;                                    

				IRequestOperationResult getInfoResult = await SendToDevice(deviceIP, CommandName.GetInfo);

				//if(asAP)
				//{
				//	curIP = await GetLocalIPFromDevice(deviceIP);
				//}

				if (getInfoResult.Success /*&& curIP != null*/)
				{
					string[] info = getInfoResult.ResponseMessage.Split('&');

					deviceInfo = new DeviceInfo()
					{
						ID = ushort.Parse(info[0]),

						//FirmwareType = int.Parse(info[1]),
						Mac = new MacAddress(info[2]),
						Name = info[3],
						DeviceType = int.Parse(info[4]),
						IsConnected = true
					};

					result.Success = true;
					result.DeviceBasicInfo = deviceInfo;
				}
				else if(!getInfoResult.Success)
				{
					result.ErrorMessage = getInfoResult.ErrorMessage;
				}

				return result;
			});
		}

		public async Task<GetDeviceCommandsOperationResult> GetDeviceCommands(IPAddress deviceIP)
		{
			GetDeviceCommandsOperationResult result = new GetDeviceCommandsOperationResult();

			if (deviceIP == null)
			{
				result.ErrorMessage = "deviceIP не может быть null";
				return result;
			}

			return await Task.Run(async () =>
			{
				IRequestOperationResult getCommandsResult = await SendToDevice(deviceIP, CommandName.GetCommands);

				if(getCommandsResult.Success)
				{
					string[] commandsParams = getCommandsResult.ResponseMessage.Split('&');
                    List<DeviceCommandInfo> commandsInfos = new List<DeviceCommandInfo>(commandsParams.Length);

                    foreach(string commandParams in commandsParams)
                    {
                        string[] cParams = commandParams.Split('=');
                        commandsInfos.Add(new DeviceCommandInfo { ID = int.Parse(cParams[0]), CommandName = cParams[1] });
                    }

					result.CommandsInfos = commandsInfos;
					result.Success = true;
				}
				else
				{
					result.ErrorMessage = getCommandsResult.ErrorMessage;
				}

				return result;
			});
		}

        /// <summary>
        /// Получить ID устройства
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public async Task<int> GetDeviceIDAsync(IPAddress deviceIP)
        {
            if (deviceIP != null && deviceIP != Consts.ZERO_IP)
            {
                IRequestOperationResult result = await SendToDevice(deviceIP, CommandName.GetID);

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
        }

		/// <summary>
		/// Проверить соединение с устройством
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		public async Task<bool> CheckConnection(IDevice device)
		{
			if (device.IP != null)
			{
				GetBaseInfoOperationResult getInfoResult = await GetDeviceInfo(device.IP);

				if (getInfoResult.Success &&
					getInfoResult.DeviceBasicInfo.ID == device.ID &&
					getInfoResult.DeviceBasicInfo.Mac == device.Mac &&
					getInfoResult.DeviceBasicInfo.DeviceType == device.DeviceType)
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
				List<RequestParameter> content = new List<RequestParameter>(4);
				byte[] bytes = hostIP.GetAddressBytes();
				byte bNumber = 1;

				foreach (byte b in bytes)
				{
					content.Add(new RequestParameter($"b{bNumber}", b.ToString()));
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
		/// <param name="id"></param>
		/// <param name="deviceIP"></param>
		/// <returns></returns>
		public async Task<IOperationResult> SendIdToDevice(int id, IPAddress deviceIP)
		{
			if (deviceIP != null && deviceIP != Consts.ZERO_IP)
			{
				byte[] bytes = BitConverter.GetBytes(id);
				string byte1value = bytes[0].ToString();
				string byte2value = bytes[1].ToString();

				List<RequestParameter> content = new List<RequestParameter>
				{
					new RequestParameter("b1", byte1value),
					new RequestParameter("b2", byte2value)
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
		public async Task<IOperationResult> SendConnectionParamsToDevice(IPAddress deviceIP, IConnectionParamsToAP connectionParams)
		{
			return await Task.Run(async () =>
			{
				List<RequestParameter> content = new List<RequestParameter>
				{
					new RequestParameter("ssid", connectionParams.SSID),
					new RequestParameter("password", connectionParams.Password)
				};


				OperationResult result = await SendToDevice(deviceIP, CommandName.ConnectionParams, content) as OperationResult;

				//TODO: не забыть о коннекторе
				//if (!result.Success)
				//{
				//	ConnectorByWiFi connector = new ConnectorByWiFi();
				//	WiFiAdapter wiFiAdapter = await connector.GetWiFiAdapter();
				//	if (wiFiAdapter.NetworkAdapter.NetworkItem.GetNetworkTypes() == Windows.Networking.Connectivity.NetworkTypes.None)
				//	{
				//		result.Success = await connector.Reconnect();
				//	}
				//}

				return result;
			});
		}

		/// <summary>
		/// Возвращает ip присвоенный роутером
		/// </summary>
		/// <returns></returns>
		public async Task<IPAddress> GetLocalIPFromDevice(IPAddress accessPointIP)
		{
			return await Task.Run(async () =>
			{
				IPAddress ip = Consts.ZERO_IP;

				IRequestOperationResult result = await SendToDevice(accessPointIP, CommandName.GetIP);

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

		/// <summary>
		/// Отправить устройству запрос
		/// </summary>
		/// <param name="deviceIP"></param>
		/// <param name="commandName"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		private async Task<IRequestOperationResult> SendToDevice(IPAddress deviceIP, CommandName commandName, IEnumerable<RequestParameter> content = null)
		{
			return await SendToDeviceAsync(deviceIP, commandName.ToString(), content);
		}
	}
}

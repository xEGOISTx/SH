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
		/// <returns></returns>
		public async Task<IOperationGetBaseInfoResult> GetDeviceInfo(IPAddress deviceIP)
		{
			return await Task.Run(async () =>
			{
				DeviceInfo deviceInfo = null;

				OperationResult result = await SendToDevice(deviceIP, CommandName.GetInfo) as OperationResult;

				if (result.Success)
				{
					string[] info = result.ResponseMessage.Split('&');

					deviceInfo = new DeviceInfo(deviceIP)
					{
						ID = ushort.Parse(info[0]),
						FirmwareType = (FirmwareType)int.Parse(info[1]),
						Mac = new MacAddress(info[2]),
						Name = info[3],
						DeviceType = int.Parse(info[4]),
						IsConnected = true
					};
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
			return await Task.Run(async () =>
			{
				if (device.IP != null && device.IP != Consts.ZERO_IP)
				{
					IOperationGetBaseInfoResult result = await GetDeviceInfo(device.IP);

					if(result.Success && 
					result.BasicInfo.ID == device.ID && 
					result.BasicInfo.Mac == device.Mac && 
					result.BasicInfo.DeviceType == device.DeviceType)
					{
						return true;
					}
				}

				return false;
			});

		}
	}
}

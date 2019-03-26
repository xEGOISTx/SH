/*======================================================================
 * Класс содержит методы передачи/получения информации и управления уже
 * подключенным устойствам.
 ======================================================================*/

using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Web.Http;

namespace SHBase.Communication
{
	/// <summary>
	/// Класс для общения с устройством
	/// </summary>
	public class Communicator
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
						OperationResult result = await SendToDevice(task.Owner.IP, CommandNames.GPIOActions, content);
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
		public async Task<GetBaseInfoResult> GetDeviceInfo(IPAddress iPAddress)
		{
			return await Task.Run(async () =>
			{
				DeviceBase deviceInfo = null;

				OperationResult result = await SendToDevice(iPAddress, CommandNames.GetInfo);

				if (result.Success)
				{
					string[] info = result.ResponseMessage.Split('&');

					deviceInfo = new DeviceBase(iPAddress)
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
		public async Task<int> GetDeviceID(IPAddress iP)
		{
			return await Task.Run(async () =>
			{
				if (iP != null && iP != Consts.ZERO_IP)
				{
					OperationResult result = await SendToDevice(iP, CommandNames.GetID);

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
		/// Отпрпавить id устройству
		/// </summary>
		/// <param name = "id" ></ param >
		/// < returns ></ returns >
		public async Task<OperationResult> SendIdToDevice(int id, IPAddress ip)
		{
			if (ip != null && ip != Consts.ZERO_IP)
			{
				byte[] bytes = BitConverter.GetBytes(id);
				string byte1value = bytes[0].ToString();
				string byte2value = bytes[1].ToString();

				List<CommandParameter> content = new List<CommandParameter>
				{
					new CommandParameter("b1", byte1value),
					new CommandParameter("b2", byte2value)
				};

				OperationResult result = await SendToDevice(ip, CommandNames.SetID, content);

				return result;
			}
			else
			{
				return new OperationResult { Success = false, ErrorMessage = "Не задан IP" };
			}
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
					GetBaseInfoResult result = await GetDeviceInfo(device.IP);

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

		/// <summary>
		/// Отправить устройству запрос
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="commandName"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		internal async Task<OperationResult> SendToDevice(IPAddress ip,CommandNames commandName, IEnumerable<CommandParameter> content = null)
		{
			return await Task.Run(async () =>
			{
				OperationResult result = new OperationResult();

				string strContent = $"&{commandName}&";

				if (content != null)
				{
					foreach (var param in content)
					{
						strContent += $"{param.Name}={param.Value}&";
					}
				}

				Uri uri = new Uri($"http://{ip}/{strContent}");

				while (true)
				{
					using (HttpClient httpClient = new HttpClient())
					{
						try
						{
							using (HttpResponseMessage responseMessage = await httpClient.GetAsync(uri))
							{

								if (responseMessage.ReasonPhrase == "OK")
								{
									string response = await responseMessage.Content.ReadAsStringAsync();

									result.Success = true;
									result.ResponseMessage = response.Replace("\r\n", string.Empty);
									break;
								}
								else
								{
									result.ErrorMessage = "Post failed!";
								}
							}
						}
						catch(Exception ex)
						{
							result.ErrorMessage = ex.Message;
							result.Success = false;
							break;
						}

						//responseMessage.EnsureSuccessStatusCode();
					};
				}

				return result;
			});
		}
	}
}

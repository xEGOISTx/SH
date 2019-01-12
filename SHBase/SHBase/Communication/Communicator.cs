using SHBase.DeviceBase;
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
	/// Класс для общения с устройством через роутер
	/// </summary>
	public class Communicator
	{
		/// <summary>
		/// Отправить устройству задачу действия с пинами
		/// </summary>
		/// <typeparam name="T">Тип действия</typeparam>
		/// <param name="task">Задача</param>
		/// <returns></returns>
		public async Task<bool> SendGPIOTask<T>(IGPIOActionTask<T> task)
			where T: IGPIOAction
		{
			return await Task.Run(async () =>
			{
				if(task != null && task.OwnerIP != null && task.OwnerIP != Consts.ZERO_IP)
				{
					List<ICommandParameter> content = new List<ICommandParameter>();
					foreach (IGPIOAction gPIOAction in task.Actions)
					{
						if (gPIOAction.Mode != GPIOMode.NotDefined && gPIOAction.Level != GPIOLevel.NotDefined)
						{
							//TODO: изменить в коде устройства прием команд для пинов
							CommandParameter commandParameter = new CommandParameter(gPIOAction.PinNumber.ToString(), $"{gPIOAction.Mode}_{gPIOAction.Level}");
							content.Add(commandParameter);
						}
					}

					if (content.Count() > 0)
					{
						OperationResult result = await SendToDevice(task.OwnerIP, CommandNames.GPIOActions, content);
						return result.Success;
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
		internal async Task<OperationResult> SendToDevice(IPAddress ip,CommandNames commandName, IEnumerable<ICommandParameter> content = null)
		{
			return await Task.Run(async () =>
			{
				OperationResult resilt = new OperationResult();

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

									resilt.Success = true;
									resilt.ResponseMessage = response.Replace("\r\n", string.Empty);
									System.Diagnostics.Debug.WriteLine("Post success!");
									break;
								}
								else
								{
									resilt.ErrorMessage = "Post failed!";
									System.Diagnostics.Debug.WriteLine("Post failed!");
								}
							}
						}
						catch(Exception ex)
						{
							resilt.ErrorMessage = ex.Message;
							resilt.Success = false;
							break;
						}

						//responseMessage.EnsureSuccessStatusCode();
					};
				}

				return resilt;
			});
		}
	}
}

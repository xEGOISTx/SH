using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SHBase;

namespace SHToolKit.Communication
{
	public abstract class RequestsSender
	{
		/// <summary>
		/// Отправить устройству запрос
		/// </summary>
		/// <param name="deviceIP"></param>
		/// <param name="commandName"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		protected async Task<IOperationResult> SendToDevice(IPAddress deviceIP, string commandName, IEnumerable<CommandParameter> content = null)
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

				Uri uri = new Uri($"http://{deviceIP}/{strContent}");

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
						catch (Exception ex)
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

		/// <summary>
		/// Отправить устройству запрос
		/// </summary>
		/// <param name="deviceIP"></param>
		/// <param name="commandName"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		internal async Task<IOperationResult> SendToDevice(IPAddress deviceIP, CommandName commandName, IEnumerable<CommandParameter> content = null)
		{
			return await SendToDevice(deviceIP, commandName.ToString(), content);
		}
	}
}

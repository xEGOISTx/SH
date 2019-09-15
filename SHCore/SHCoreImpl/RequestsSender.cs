using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SH.Core
{
	/// <summary>
	/// Отправитель запросов
	/// </summary>
	public abstract class RequestsSender
	{
        /// <summary>
        /// Отправить устройству запрос
        /// </summary>
        /// <param name="deviceIP"></param>
        /// <param name="requestName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected async Task<IRequestOperationResult> SendToDeviceAsync(IPAddress deviceIP, string requestName, IEnumerable<RequestParameter> content = null)
		{
			return await Task.Run(async () =>
			{
				RequestOperationResult result = new RequestOperationResult();

				string strContent = $"&{requestName}&";

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
							//result.Success = false;
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

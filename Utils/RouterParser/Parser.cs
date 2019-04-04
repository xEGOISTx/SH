using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;



namespace RouterParser
{
    public class Parser : SHBase.IRouterParser
    {
		//private readonly string _ip;
		//private readonly string _login;
		//private readonly string _password;
		private static WebView _webView;
		private static bool _loadComplete = true;
		private static bool _parseComplete = true;
		private string _content;

		public Parser(/*string ip,string login,string password*/)
		{
			//_ip = ip;
			//_login = login;
			//_password = password;

			if(_webView == null)
			{
				_webView = new WebView();
			}
		}

		public async Task<IEnumerable<IPAddress>> GetDevicesIPs(IPAddress routerIP, string login, string password)
		{
			ParseResult result = new ParseResult();
			string strUrl = $"http://{ routerIP }/";

			if (_parseComplete)
			{
				_parseComplete = false;
				_loadComplete = false;

				OperationResult authorizationResult = await Authorization(strUrl, login, password);

				_webView.LoadCompleted += WebView_LoadCompleted;

				await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					_webView.Navigate(new Uri(strUrl));
				});


				await Task.Run(() =>
				{
					while (!_loadComplete)
					{ }

					if (_content != null)
					{
						result.Success = true;
						result.DeviceInfos = GetDivInfo(_content).ToArray();
					}
					else
					{
						result.ErrorText = "Не удалось загрузить контент";
					}
				});

				_webView.LoadCompleted -= WebView_LoadCompleted;

				await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					_webView.NavigateToString("");
				});


				_parseComplete = true;
			}
			else
			{
				result.ErrorText = "Предыдущий процесс загрузки устройств ещё не завершен!";
			}

			_content = null;
			return result.DeviceInfos.Select(dI => dI.Ip);
		}

		//public async Task<ParseResult> LoadDeviceInfosAsync()
		//{
		//	ParseResult result = new ParseResult();

		//	if (_parseComplete)
		//	{
		//		_parseComplete = false;
		//		_loadComplete = false;

		//		OperationResult authorizationResult = await Authorization();

		//		_webView.LoadCompleted += WebView_LoadCompleted;

		//		await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
		//		{
		//			 _webView.Navigate(new Uri(_ip));
		//		});


		//		await Task.Run(() =>
		//		{
		//			while (!_loadComplete)
		//			{ }

		//			if (_content != null)
		//			{
		//				result.Success = true;
		//				result.DeviceInfos = GetDivInfo(_content).ToArray();
		//			}
		//			else
		//			{
		//				result.ErrorText = "Не удалось загрузить контент";
		//			}
		//		});

		//		_webView.LoadCompleted -= WebView_LoadCompleted;

		//		await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
		//		{
		//			_webView.NavigateToString("");
		//		});


		//		_parseComplete = true;
		//	}
		//	else
		//	{
		//		result.ErrorText = "Предыдущий процесс загрузки устройств ещё не завершен!";
		//	}

		//	_content = null;
		//	return result;
		//}



		private IEnumerable<RDeviceInfo> GetDivInfo(string content)
		{
			List<List<string>> data = new List<List<string>>();
			List<RDeviceInfo> deviceInfos = new List<RDeviceInfo>();

			//разбаваем на строки
			string[] pars = content.Split(new char[] { '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			//получаем строку в которой хранятся нужные данные
			string necessaryString = pars.Where(s => s.IndexOf("show_connected_devices();") != -1).FirstOrDefault();
			//разбаваем на строки
			pars = necessaryString.Trim(' ', '"').Split(new string[] { "tr" }, StringSplitOptions.RemoveEmptyEntries);
			//отбираем только нужные строки
			IEnumerable<string> necessaryStrings =	from s in pars
													where s.IndexOf("td") != -1
													select s.Trim('>', '/');

			foreach (string str in necessaryStrings)
			{
				string[] colStrs = str.Split(new string[] { "td" }, StringSplitOptions.RemoveEmptyEntries);
				List<string> temp = new List<string>();

				foreach (string colStr in colStrs)
				{
					if (colStr.Length > 3)
					{
						string[] res1 = colStr.Split('<', '>');
						temp.Add(res1[1]);
					}
				}

				data.Add(temp);
			}

			foreach (List<string> item in data)
			{
				string name = item[2];
				bool isConnected = item[5] == "Подключено";

				if (name.Contains("ESP") && isConnected)
				{
					string ip = item[3];
					string mac = item[4];
					RDeviceInfo deviceInfo = new RDeviceInfo(name, ip, mac, isConnected);
					deviceInfos.Add(deviceInfo);
				}
			}

			return deviceInfos;
		}

		private async Task<OperationResult> Authorization(string strUrl, string login, string password)
		{
			//OperationResult operationResult = null;

			return await Task.Run(async () => 
			{
				OperationResult operationResult = new OperationResult();
				PasswordCredential credential = new PasswordCredential(strUrl, login, password);
				Windows.Web.Http.Filters.HttpBaseProtocolFilter filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
				filter.ServerCredential = credential;

				Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient(filter);

				var headers = httpClient.DefaultRequestHeaders;

				string header = "ie";
				if (!headers.UserAgent.TryParseAdd(header))
				{
					throw new Exception("Invalid header value: " + header);
				}

				header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
				if (!headers.UserAgent.TryParseAdd(header))
				{
					throw new Exception("Invalid header value: " + header);
				}

				Uri requestUri = new Uri(strUrl);

				Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
			 
				try
				{
					httpResponse = await httpClient.GetAsync(requestUri);
					httpResponse.EnsureSuccessStatusCode();
					operationResult.Success = true;
				}
				catch (Exception ex)
				{
					operationResult.ErrorDescription = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
				}

				return operationResult;
			});	
		}

		private async void WebView_LoadCompleted(object sender, NavigationEventArgs e)
		{
			_content = await (sender as WebView).InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
			_loadComplete = true;
		}

	}
}

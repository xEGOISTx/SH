using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SH.Communication;
using SH.Core;
using Windows.Security.Credentials;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SH.UWPToolKit
{
    public class RouterParser : IRouterParser
    {
        private const string IS_CONNECTED = "Подключено";

        private static WebView _webView;
        private static bool _loadComplete = true;
        private static bool _parseComplete = true;
        private string _content;

        public async Task<IParseOperationResult> GetActiveIPs(Uri uriToParse, ICredentials routerCredentials, IAPSSIDs aPSSIDs)
        {
            if (_webView == null)
            {
                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _webView = new WebView();
                });
            }

            ParseOperationResult result = new ParseOperationResult();

            if (_parseComplete)
            {
                _parseComplete = false;
                _loadComplete = false;

                bool authorizationComplete = await Authorization(uriToParse, routerCredentials.Login, routerCredentials.Password);

                if (authorizationComplete)
                {
                    _webView.LoadCompleted += WebView_LoadCompleted;

                    await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        _webView.Navigate(uriToParse);
                    });


                    await Task.Run(() =>
                    {
                        while (!_loadComplete)
                        { }

                        if (_content != null)
                        {
                            result.Success = true;
                            result.IPs = GetIPs(_content, aPSSIDs);
                        }
                        else
                        {
                            result.ErrorMessage = "Не удалось загрузить контент";
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
                    result.ErrorMessage = "Не удалось авторизироваться";
                }
            }
            else
            {
                result.ErrorMessage = "Предыдущий процесс загрузки устройств ещё не завершен!";
            }

            _content = null;
            return result;
        }

        private IEnumerable<string> GetIPs(string content, IAPSSIDs aPSSIDs)
        {
            //TODO: тестовый парсинг. перделать на что-нибудь по серьёзнее

            List<List<string>> data = new List<List<string>>();
            List<string> iPs = new List<string>();

            //разбаваем на строки
            string[] pars = content.Split(new char[] { '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            //получаем строку в которой хранятся нужные данные
            string necessaryString = pars.Where(s => s.IndexOf("show_connected_devices();") != -1).FirstOrDefault();
            //разбаваем на строки
            pars = necessaryString.Trim(' ', '"').Split(new string[] { "tr" }, StringSplitOptions.RemoveEmptyEntries);
            //отбираем только нужные строки
            IEnumerable<string> necessaryStrings = from s in pars
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
                bool isConnected = item[5] == IS_CONNECTED;

                if (aPSSIDs.Contains(name) && isConnected)//name.Contains("ESP")
                {
                    string ip = item[3];
                    //MacAddress mac = new MacAddress(item[4]);
                    //DeviceInfo deviceInfo = new DeviceInfo(ip)
                    //{
                    //	Name = name,
                    //	Mac = mac,
                    //	IsConnected = isConnected
                    //};
                    iPs.Add(ip);
                }
            }

            return iPs;
        }

        private async Task<bool> Authorization(Uri url, string login, string password)
        {
            bool result;

            return await Task.Run(async () =>
            {
                PasswordCredential credential = new PasswordCredential(url.AbsoluteUri, login, password);
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

                Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();

                try
                {
                    httpResponse = await httpClient.GetAsync(url);
                    httpResponse.EnsureSuccessStatusCode();
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                }

                return result;
            });
        }

        private async void WebView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            _content = await (sender as WebView).InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
            _loadComplete = true;
        }
    }
}

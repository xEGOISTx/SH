using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace RouterParser
{
	internal class ContentLoader
	{
		private string _content = string.Empty;
		private bool _isLoad;
		private OperationResult _operationResult;

		public string GetContent()
		{
			return _content;
		}

		public async Task<OperationResult> LoadContentAsync(Uri uri)
		{
			_isLoad = false;
			WebView webView = new WebView();
			webView.LoadCompleted += WebView_LoadCompleted;
			_operationResult = new OperationResult();
			webView.Navigate(uri);

			await Task.Run(() =>
			{
				while (!_isLoad)
				{

				}
			});
			//await waitTask;

			webView.NavigateToString("");

			//webView.Stop();
			//await WebView.ClearTemporaryWebDataAsync();
			//Windows.Web.Http.Filters.HttpBaseProtocolFilter myFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
			//var cookieManager = myFilter.CookieManager;

			//HttpCookieCollection myCookieJar = cookieManager.GetCookies(uri);
			//foreach (HttpCookie cookie in myCookieJar)
			//{
			//	cookieManager.DeleteCookie(cookie);
			//}


			//webView.LoadCompleted -= WebView_LoadCompleted;
			//webView = null;

			//HttpClient httpClient = new HttpClient();
			//HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);


			//HttpResponseMessage res = await httpClient.GetAsync(new Uri("http://192.168.1.254/status_dhcp.htm?l0=0&l1=3&l2=-1&l3=-1"));
			//string cont = await res.Content.ReadAsStringAsync();



			return _operationResult;
		}

















		private async void WebView_LoadCompleted(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
		{
			try
			{
				_content = await (sender as WebView).InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
				_isLoad = true;
				_operationResult.Success = true;
			}
			catch(Exception ex)
			{
				_operationResult.ErrorDescription = ex.Message;
			}			
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace SHBase
{
	//TODO: перенести в инструменты
	public class Server
	{
		private const string PORT = "8000";

		public Server()
		{
			//string ip = FindIPAddress();
			//StartServer();
		}


		private async void StartServer()
		{
			StreamSocketListener listener = new StreamSocketListener();
			await listener.BindServiceNameAsync(PORT);

			listener.ConnectionReceived += async (s, e) =>
			{
				string request = null;
				using (var streamReader = new StreamReader(e.Socket.InputStream.AsStreamForRead()))
				{
					request = await streamReader.ReadLineAsync();
				}
			};
		}



		private static string FindIPAddress()
		{
			string ip = null;

			foreach (HostName localHostName in NetworkInformation.GetHostNames())
			{
				if (localHostName.IPInformation != null)
				{
					if (localHostName.Type == HostNameType.Ipv4)
					{
						ip = localHostName.ToString();
						break;
					}
				}
			}
			return ip;
		}
	}
}

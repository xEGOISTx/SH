using System;
using System.Collections.Generic;
using System.Linq;
using IPAddress = System.Net.IPAddress;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using Windows.Security.Credentials;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using RouterParser;
using System.IO;
using Windows.Networking;
using Windows.Networking.Connectivity;
using SHBase.Communication;
using SHBase.DeviceBase;

namespace DevicesPresenter
{
	public class DevicesManager : IDevicesManager
	{
		#region Fields
		private readonly IPAddress _aPDefaultIP = IPAddress.Parse("192.168.4.1");
		private readonly IPAddress _host;
		private readonly Dictionary<ushort, IDevice> _devices = new Dictionary<ushort, IDevice>();


		//private WiFiAdapter _wifiAdapter;
		//private readonly IPAddress _zeroIP = IPAddress.Parse("0.0.0.0");

		//private readonly List<IDevice> _newDevices = new List<IDevice>();
		#endregion Fields

		public DevicesManager()
		{
			ConnectionSettings settings = new ConnectionSettings();
			settings.Load();
			ConnectionSettings = settings;

			foreach (HostName localHostName in NetworkInformation.GetHostNames())
			{
				if (localHostName.IPInformation != null)
				{
					if (localHostName.Type == HostNameType.Ipv4)
					{
						_host =  IPAddress.Parse(localHostName.ToString());
						break;
					}
				}
			}


			//Device device = new Device
			//{
			//	ID = 5,
			//	IP = IPAddress.Parse("1.1.1.1"),
			//	Description = "ESP",
			//	FirmwareType = FirmwareType.ESP_8266,
			//	IsConnected = true,
			//	Mac = new SHBase.MacAddress("12:34:56:78:90:ab"),
			//	Name = "ESP"
			//};

			//DeviceTask task = new DeviceTask { Description = "Task", ID = 1 };
			//device.AddTask(task);


			//_devices.Add(5, device);
		}

		public IReadOnlyDictionary<ushort,IDevice> Devices => _devices;

		public IConnectionSettings ConnectionSettings { get; }

		public IDeviceEditor GetDeviceEditor()
		{
			return new DeviceEditor(this);
		}

		public async Task<bool> FindAndConnectDevicesAsync()
		{
			ConnectorByWiFi connector = new ConnectorByWiFi();
			CommunicatorAP communicatorAP = new CommunicatorAP(_aPDefaultIP);

			//получаем доступные девайсы
			IEnumerable<WiFiAvailableNetwork> wifiAvailableDevices = await connector.GetAvailableDevicesAsAPAsync();

			List<Device> newDevices = new List<Device>();

			foreach (WiFiAvailableNetwork wifiDevice in wifiAvailableDevices)
			{
				//подключаемся к девайсу
				bool connRes = await connector.ConnectToDeviceAsync(wifiDevice, new PasswordCredential { Password = ConnectionSettings.DeviceConnParams.Password });
				if (connRes)
				{
					//получаем инфу девайса из него самого
					DeviceBaseInfo deviceInfo = await communicatorAP.GetDeviceInfoFromDeviceAsAP();
					if (deviceInfo != null && deviceInfo.ID == 0)
					{
						//отправляем параметры для подключения к роутеру
						bool postRes = await communicatorAP.SendConnectionParamsToDeviceAsAP(ConnectionSettings.RouterConnParams);

						if (postRes)
						{
							//запрашиваем ip
							IPAddress iP = await communicatorAP.GetLocalIPFromDeviceAsAP();

							if (iP != null)
							{
								ushort id = GeneratedId();

								//отправляем id девайсу
								bool sendRes = await communicatorAP.SendIdToDeviceAsAP(id);

								if (sendRes)
								{
									Device device = new Device()
									{
										ID = id,
										IP = iP,
										FirmwareType = deviceInfo.FirmwareType,
										Mac = deviceInfo.Mac,
										Name = deviceInfo.Name,
										IsConnected = true,
										Description = deviceInfo.Name
									};


									newDevices.Add(device);
									_devices.Add(device.ID, device);
								}
							}
						}
					}
				}

				//отключаемся
				connector.DisconnectAsync();
			}

			//сохраняем новые девайсы в базу


			return true;
		}

		public async Task UpdateConnectedDevicesAsync()
		{
			throw new NotImplementedException();
		}

		//public async Task<IEnumerable<IDevice>> TestDevices()
		//{
		//	Parser parser = new Parser("http://192.168.1.254", "admin", "admin");
		//	//IEnumerable<RDeviceInfo> deviceInfos = await parser.GetDeviceInfosAsync();
		//	List<IDevice> devices = new List<IDevice>();

		//	//foreach(RDeviceInfo rDeviceInfo in deviceInfos)
		//	//{
		//	//	Device device = new Device { IP = rDeviceInfo.Ip};

		//	//	devices.Add(device);
		//	//}

		//	return devices;
		//}

		//public async Task<bool> TestSendIdTo(IPAddress iPAddress, ushort id)
		//{
		//	return await SendIdTo(iPAddress, id);
		//}


		//private async Task<IEnumerable<WiFiAvailableNetwork>> GetAvailableDevices(string ssid)
		//{
		//	ssid = ssid ?? string.Empty;

		//	bool initAdapterRes = await InitializeFirstAdapter();

		//	if (initAdapterRes)
		//	{
		//		await _wifiAdapter.ScanAsync();

		//		return await Task<IEnumerable<WiFiAvailableNetwork>>.Factory.StartNew(() =>
		//		{
		//			return _wifiAdapter.NetworkReport.AvailableNetworks.Where(dev => dev.Ssid == ssid);
		//		});
		//	}

		//	return new List<WiFiAvailableNetwork>();

		//}

		//private async Task<bool> SendIdTo(IPAddress iPAddress, ushort id)
		//{
		//	byte[] bytes = BitConverter.GetBytes(id);

		//	Dictionary<string, string> content = new Dictionary<string, string>
		//	{
		//		{"setid", "code" },
		//		{"b1", bytes[0].ToString() },
		//		{"b2", bytes[1].ToString() }
		//	};

		//	return await PostToDevice(iPAddress, content);
		//}

		//private async Task<ushort> GetId(IPAddress iPAddress)
		//{
		//	return await Task.Run(async () =>
		//	{
		//		HttpClient httpClient = new HttpClient();
		//		Uri uri = new Uri($"http://{iPAddress}/getID/");

		//		ushort res;

		//		while (true)
		//		{

		//			using (HttpResponseMessage responseMessage = await httpClient.GetAsync(uri))
		//			{
		//				if (responseMessage.StatusCode == HttpStatusCode.Ok)
		//				{
		//					System.Diagnostics.Debug.WriteLine("Get success!");
		//					string content = (await responseMessage.Content.ReadAsStringAsync()).Trim('\\', 'r', 'n');
		//					res = ushort.Parse(content);

		//					//res = BitConverter.ToUInt16(idData, 0);
		//					break;
		//				}
		//				else
		//				{
		//					System.Diagnostics.Debug.WriteLine("Get failed!");
		//				}
		//				//responseMessage.EnsureSuccessStatusCode();
		//			};

		//		}

		//		return res;
		//	});
		//}

		//public async Task<Device> GetDeviceInfoFromDevice(IPAddress iPAddress)
		//{
		//	return await Task.Run(async () =>
		//	{
		//		HttpClient httpClient = new HttpClient();
		//		Uri request = new Uri($"http://{iPAddress}/getInfo");
		//		Device device = null;

		//		using (HttpResponseMessage httpResponse = await httpClient.GetAsync(request))
		//		{
		//			if (httpResponse.ReasonPhrase == "OK")
		//			{
		//				string response = (await httpResponse.Content.ReadAsStringAsync()).Replace("\r\n", string.Empty);
		//				string[] info = response.Split('&');

		//				ushort id = ushort.Parse(info[0]);
		//				FirmwareType firmwareType = (FirmwareType)int.Parse(info[1]);
		//				string mac = info[2];
		//				string name = info[3];

		//				device = new Device
		//				{
		//					ID = id,
		//					FirmwareType = firmwareType,
		//					Mac = mac,
		//					Name = name,
		//					Description = name,
		//				};
		//			}
		//		}

		//		return device;
		//	});
		//}

		//private async Task<IPAddress> GetLocalIP(IPAddress iPAddress)
		//{
		//	return await Task.Run(async () =>
		//	{
		//		HttpClient httpClient = new HttpClient();
		//		Uri request = new Uri($"http://{iPAddress}/getIP");
		//		IPAddress res = _zeroIP;

		//		using (HttpResponseMessage response = await httpClient.GetAsync(request))
		//		{
		//			if (response.ReasonPhrase == "OK")
		//			{
		//				string content = (await response.Content.ReadAsStringAsync()).Replace("\r\n", string.Empty);

		//				byte[] byteArr = BitConverter.GetBytes(int.Parse(content));
		//				res = new IPAddress(byteArr);
		//			}
		//		};

		//		return res;
		//	});
		//}

		//private async Task<bool> InitializeFirstAdapter()
		//{
		//	return await Task.Run(async () =>
		//	{
		//		if (_wifiAdapter == null)
		//		{
		//			bool res = false;
		//			WiFiAccessStatus access = await WiFiAdapter.RequestAccessAsync();

		//			if (access != WiFiAccessStatus.Allowed)
		//			{
		//				throw new Exception("WiFiAccessStatus not allowed");
		//			}
		//			else
		//			{
		//				var wifiAdapterResults = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());

		//				if (wifiAdapterResults.Count >= 1)
		//				{
		//					_wifiAdapter = await WiFiAdapter.FromIdAsync(wifiAdapterResults[0].Id);
		//					res = true;
		//				}
		//				else
		//				{
		//					throw new Exception("WiFi Adapter not found.");
		//				}
		//			}
		//			return res;
		//		}
		//		else
		//		{
		//			return true;
		//		}
		//	});
		//}

		////TODO:добавть контроль над выполнением и возврат IOperationResult
		//private async Task<bool> ConnectToDevice(WiFiAvailableNetwork wiFi, IConnectionParams deviceConnectionParams)
		//{
		//	WiFiConnectionResult conResult;

		//	do
		//	{
		//		conResult = await _wifiAdapter.ConnectAsync(wiFi, WiFiReconnectionKind.Manual,
		//			new PasswordCredential { Password = deviceConnectionParams.Password });

		//		if (conResult.ConnectionStatus != WiFiConnectionStatus.Success)
		//			System.Diagnostics.Debug.WriteLine("Connection failed!");
		//		else
		//		{
		//			System.Diagnostics.Debug.WriteLine("Connection success!");
		//		}
		//	}
		//	while (conResult.ConnectionStatus != WiFiConnectionStatus.Success);

		//	return true;
		//}

		//private async Task<bool> PostToDevice(IPAddress iPAddress, Dictionary<string, string> content)
		//{
		//	return await Task.Run(async () =>
		//	{
		//		string strContent = "&";

		//		foreach (var pair in content)
		//		{
		//			strContent += $"{pair.Key}={pair.Value}&";
		//		}

		//		Uri uri = new Uri($"http://{iPAddress}/{strContent}");

		//		while (true)
		//		{
		//			using (HttpClient httpClient = new HttpClient())
		//			{
		//				using (HttpResponseMessage responseMessage = await httpClient.GetAsync(uri))
		//				{
		//					string getCont = await responseMessage.Content.ReadAsStringAsync();

		//					if (responseMessage.ReasonPhrase == "OK")
		//					{
		//						System.Diagnostics.Debug.WriteLine("Post success!");
		//						break;
		//					}
		//					else
		//					{
		//						System.Diagnostics.Debug.WriteLine("Post failed!");
		//					}

		//				}


		//				//responseMessage.EnsureSuccessStatusCode();
		//			};
		//		}

		//		return true;
		//	});
		//}


		private ushort GeneratedId()
		{
			Loader loader = new Loader();
			List<int> iDs = loader.GetDevicesIDs();
			int id = 0;

			if (iDs.Count == 0)
				return 1;

			for (int i = 1; i < ushort.MaxValue; i++)
			{
				if (!iDs.Contains(i))
				{
					id = i;
					break;
				}
			}

			return (ushort)id;
		}
	}
}

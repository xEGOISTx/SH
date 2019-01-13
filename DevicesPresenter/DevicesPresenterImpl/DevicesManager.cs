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

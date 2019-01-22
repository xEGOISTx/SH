using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataManager;
using SHBase;
using SHBase.Communication;
using SHBase.DevicesBaseComponents;

namespace Switches.SwitchesOutlets
{
	public class Swithes : Devices, ISwithes
	{
		private readonly SwitchesAndOutletsList _switchesAndOutlets = new SwitchesAndOutletsList();

		public ISwitchesAndOutletsList SwitchesAndOutlets => _switchesAndOutlets;


		/// <summary>
		/// Проверить соответствует ли устройство этому набору устройств
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		public override bool CheckForComplianceDevice(IDeviceBase device)
		{
			return device.DeviceType == DeviceType.Outlet || device.DeviceType == DeviceType.Switch;
		}

		/// <summary>
		/// Добавить и сохранить новые устройства
		/// </summary>
		/// <param name="newDevices"></param>
		public override async Task<bool> AddAndSaveNewDevices(IEnumerable<IDeviceBase> newDevices)
		{
			return await Task.Run(async () =>
			{
				List<ISwitchOutlet> deivices = new List<ISwitchOutlet>();

				//создаём сответствующие устройства
				foreach (IDeviceBase device in newDevices.Where(d => d.ID < 1))
				{
					if (CheckForComplianceDevice(device))
					{
						SwitchOutlet sO = new SwitchOutlet(device.Mac, device.FirmwareType, device.DeviceType)
						{
							Description = device.Name,
							IP = device.IP,
							IsConnected = device.IsConnected,
							Name = device.Name,
							State = CurrentState.TurnedOn
						};

						deivices.Add(sO);
					}
				}

				if (deivices.Count > 0)
				{
					ISwitchesAndOutletsLoader loader = _switchesAndOutlets.GetLoader();
					IDeviceInfo[] deviceInfos = ConvertToDeviceInfos(deivices);
					IResultOperationSave resultSave = await loader.SaveDevices(deviceInfos);

					Communicator communicator = new Communicator();

					//передать новым устройствам их ID
					for (int i = 0; i < deivices.Count; i++)
					{
						SwitchOutlet device = deivices[i] as SwitchOutlet;
						int newID = resultSave.NewIDs[i];

						device.ID = newID;

						OperationResult result = await communicator.SendIdToDevice(newID, device);

						if(!result.Success)
						{
							throw new Exception("Не удалось отправить ID устройству. " + result.ErrorMessage);
						}
					}

					_switchesAndOutlets.AddRange(deivices);

					return true;
				}

				return false;
			});
		}

		/// <summary>
		/// Занрузить устройства
		/// </summary>
		public override async void Load()
		{
			if (!IsLoaded)
			{
				await Task.Run(async () =>
				{
					ISwitchesAndOutletsLoader loader = _switchesAndOutlets.GetLoader();
					IResultOperationLoad result = await loader.LoadDevices();

					if (result.Success)
					{
						IEnumerable<ISwitchOutlet> devices = ConvertToSwitchesOutlets(result.DeviceInfos);
						_switchesAndOutlets.AddRange(devices);
					}

					IsLoaded = true;
				});
			}
		}

		/// <summary>
		/// Синхронизация с подключенными к роутеру устройствами. Предварительно должна быть произведена загрузка(метод Load)
		/// </summary>
		/// <param name="devicesFromRouter"></param>
		/// <returns></returns>
		public override async Task<bool> Synchronization(IEnumerable<IDeviceBase> devicesFromRouter)
		{
			return await Task.Run(() =>
			{
				foreach(IDeviceBase deviceFromRouter in devicesFromRouter)
				{
					if (CheckForComplianceDevice(deviceFromRouter))
					{
						SwitchOutlet device = (SwitchOutlet)_switchesAndOutlets.GetByKey(deviceFromRouter.ID);
						if (device != null && CheckCorresponding(deviceFromRouter, device))
						{
							device.IP = deviceFromRouter.IP;
							device.Name = deviceFromRouter.Name;
							device.IsConnected = deviceFromRouter.IsConnected;
							//device.State = статус будем запрашивать отсюда
						}
						else
						{
							throw new Exception("Устройство не распознано");
						}
					}
				}

				return false;
			});
		}

		private IDeviceInfo[] ConvertToDeviceInfos(IEnumerable<ISwitchOutlet> devices)
		{
			List<DeviceInfo> deviceInfos = new List<DeviceInfo>();

			foreach (ISwitchOutlet device in devices)
			{
				DeviceInfo deviceInfo = new DeviceInfo
				{
					Description = device.Description,
					DeviceType = (int)device.DeviceType,
					FirmwareType = (int)device.FirmwareType,
					MacAddress = device.Mac.ToString()
				};

				deviceInfos.Add(deviceInfo);
			}

			return deviceInfos.ToArray();
		}

		private IEnumerable<ISwitchOutlet> ConvertToSwitchesOutlets(IDeviceInfo[] deviceInfos)
		{
			List<ISwitchOutlet> devices = new List<ISwitchOutlet>();

			foreach(IDeviceInfo deviceInfo in deviceInfos)
			{
				MacAddress mac = new MacAddress(deviceInfo.MacAddress);
				FirmwareType firmwareType = (FirmwareType)deviceInfo.FirmwareType;
				DeviceType deviceType = (DeviceType)deviceInfo.DeviceType;

				SwitchOutlet device = new SwitchOutlet(mac, firmwareType, deviceType)
				{
					Description = deviceInfo.Description,
					ID = deviceInfo.ID,					
				};

				devices.Add(device);
			}

			return devices;
		}

		private bool CheckCorresponding(IDeviceBase deviceFromRouter, ISwitchOutlet device)
		{
			return deviceFromRouter.Mac == device.Mac && deviceFromRouter.FirmwareType == device.FirmwareType &&
				deviceFromRouter.DeviceType == device.DeviceType;
		}
	}
}

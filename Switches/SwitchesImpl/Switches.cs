﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataManager;
using SHBase;
using SHBase.Communication;
using SHBase.DevicesBaseComponents;
using Switches.SwitchesOutlets;

namespace Switches
{
	public class Switches : Devices, ISwitches
	{
		private readonly SwitchList _switches = new SwitchList();
		private readonly OutletList _outlets = new OutletList();

		/// <summary>
		/// Список обычных выключателей
		/// </summary>
		public ISwitchesAndOutletsList SwitchList => _switches;

		/// <summary>
		/// Список розеток
		/// </summary>
		public ISwitchesAndOutletsList OutletList => _outlets;

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
				List<ISwitchOutlet> switches = new List<ISwitchOutlet>();
				List<ISwitchOutlet> outlets = new List<ISwitchOutlet>();

				//создаём сответствующие устройства
				foreach (IDeviceBase device in newDevices.Where(d => d.ID < 1))
				{
					if (CheckForComplianceDevice(device))
					{
						if (device.DeviceType == DeviceType.Switch && device.DeviceType == DeviceType.Outlet)
						{
							SwitchOutlet sO = new SwitchOutlet(device.Mac, device.FirmwareType, device.DeviceType)
							{
								Description = device.Name,
								IP = device.IP,
								IsConnected = device.IsConnected,
								Name = device.Name,
								State = CurrentState.TurnedOn
							};

							if (device.DeviceType == DeviceType.Switch)
								switches.Add(sO);

							if (device.DeviceType == DeviceType.Outlet)
								outlets.Add(sO);
						}
						else
						{
							//здесь будет обработка регулируемых выключателей
							//исключение, чтобы не упустить это момент
							throw new Exception($"Switches метод ({nameof(AddAndSaveNewDevices)}): Неопределённый тип утройства");
						}
					}
				}
				
				//сохраняем усройства, передаём устойствам ID
				if(switches.Count > 0)
				{
					ISwitchesLoader loader = _switches.GetLoader();
					IResultOperationSave result = await SaveDevices(switches, loader);

					if(result.Success)
					{
						await FillIDs(switches.ToArray(), result.NewIDs);
					}

					_switches.AddRange(switches);
				}

				if (outlets.Count > 0)
				{
					ISwitchesLoader loader = _outlets.GetLoader();
					IResultOperationSave result = await SaveDevices(outlets, loader);

					if (result.Success)
					{
						await FillIDs(outlets.ToArray(), result.NewIDs);
					}

					_outlets.AddRange(switches);
				}

				return true;
			});
		}

		/// <summary>
		/// Занрузить устройства
		/// </summary>
		public override async Task<bool> Load()
		{
			if (!IsLoaded)
			{
				return await Task.Run(async () =>
				{
					ISwitchesLoader swLoader = _switches.GetLoader();
					IResultOperationLoad swResult = await swLoader.LoadDevices();

					ISwitchesLoader ouLoader = _outlets.GetLoader();
					IResultOperationLoad ouResult = await ouLoader.LoadDevices();

					if (swResult.Success && ouResult.Success)
					{
						IEnumerable<ISwitchOutlet> swDevices = ConvertToSwitchesOutlets(swResult.DeviceInfos);
						_switches.AddRange(swDevices);

						IEnumerable<ISwitchOutlet> ouDevices = ConvertToSwitchesOutlets(ouResult.DeviceInfos);
						_outlets.AddRange(ouDevices);

						IsLoaded = true;
						return true;
					}

					return false;
				});
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Синхронизация с подключенными к роутеру устройствами. Предварительно должна быть произведена загрузка(метод Load)
		/// </summary>
		/// <param name="devicesFromRouter"></param>
		/// <returns></returns>
		public override async Task Synchronization(IEnumerable<IDeviceBase> devicesFromRouter)
		{
			await Task.Run(() =>
			{
				foreach(IDeviceBase deviceFromRouter in devicesFromRouter)
				{
					if (CheckForComplianceDevice(deviceFromRouter))
					{
						SwitchOutlet device = (SwitchOutlet)_switches.GetByKey(deviceFromRouter.ID);

						if(device == null)
						{
							device = (SwitchOutlet)_outlets.GetByKey(deviceFromRouter.ID);
						}

						if (device != null && CheckCorresponding(deviceFromRouter, device))
						{
							device.IP = deviceFromRouter.IP;
							device.Name = deviceFromRouter.Name;
							device.IsConnected = true;
							//device.State = статус будем запрашивать отсюда
						}
						//TODO: обработать случай, если утройство найдено но в базе его нет
					}
				}
			});
		}

		private IDeviceInfo[] ConvertToDeviceInfos(IEnumerable<IDeviceBase> devices)
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

		private async Task<IResultOperationSave> SaveDevices(IEnumerable<IDeviceBase> devices, ISwitchesLoader loader)
		{
			IDeviceInfo[] deviceInfos = ConvertToDeviceInfos(devices);
			return  await loader.SaveDevices(deviceInfos);
		}

		private async Task FillIDs(IDeviceBase[] newDevices, int[] newIDs)
		{
			Communicator communicator = new Communicator();

			//передать новым устройствам их ID
			for (int i = 0; i < newDevices.Length; i++)
			{
				IDeviceBase device = newDevices[i];
				int newID = newIDs[i];

				if (device is SwitchOutlet sO)
				{
					sO.ID = newID;
				}


				if (device.ID > 0)
				{
					OperationResult result = await communicator.SendIdToDevice(newID, device);

					if (!result.Success)
					{
						throw new Exception("Не удалось отправить ID устройству. " + result.ErrorMessage);
					}
				}
				else
				{
					throw new Exception("Передаваемый ID не может быть равен и меньше 0");
				}
			}
		}

	}
}
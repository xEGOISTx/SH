﻿using System;
using System.Collections;
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
		public ISwitchList SwitchList => _switches;

		/// <summary>
		/// Список розеток
		/// </summary>
		public IOutletList OutletList => _outlets;

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
				List<ISwitch> switches = new List<ISwitch>();
				List<IOutlet> outlets = new List<IOutlet>();

				//создаём сответствующие устройства
				foreach (IDeviceBase device in newDevices.Where(d => d.ID < 1))
				{
					if (CheckForComplianceDevice(device))
					{
						if (device.DeviceType == DeviceType.Switch)
						{

							Switch sw = new Switch(device.Mac, device.FirmwareType, device.DeviceType)
							{
								Description = device.Name,
								IP = device.IP,
								IsConnected = device.IsConnected,
								Name = device.Name,
								State = CurrentState.TurnedOff
							};

							switches.Add(sw);
						}
						else if(device.DeviceType == DeviceType.Outlet)
						{
							Outlet ou = new Outlet(device.Mac, device.FirmwareType, device.DeviceType)
							{
								Description = device.Name,
								IP = device.IP,
								IsConnected = device.IsConnected,
								Name = device.Name,
								State = CurrentState.TurnedOff
							};

							outlets.Add(ou);
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
					IDeviceInfo[] infos = _switches.Convertor.ConvertToDeviceInfos(switches);
					IResultOperationSave result = await loader.SaveDevices(infos);

					if(result.Success)
					{
						await FillIDs(switches.ToArray(), result.NewIDs);
					}

					_switches.AddRange(switches);
				}

				if (outlets.Count > 0)
				{
					ISwitchesLoader loader = _outlets.GetLoader();
					IDeviceInfo[] infos = _outlets.Convertor.ConvertToDeviceInfos(outlets);
					IResultOperationSave result = await loader.SaveDevices(infos);

					if (result.Success)
					{
						await FillIDs(outlets.ToArray(), result.NewIDs);
					}

					_outlets.AddRange(outlets);
				}

				return true;
			});
		}

		/// <summary>
		/// Загрузить устройства
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
						//IEnumerable<ISwitchOutlet> fake = FakeDevices();
						//_switches.AddRange(fake);

						IEnumerable<ISwitch> swDevices = _switches.Convertor.ConvertToDevices(swResult.DeviceInfos);
						_switches.AddRange(swDevices);

						IEnumerable<IOutlet> ouDevices = _outlets.Convertor.ConvertToDevices(ouResult.DeviceInfos);
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
			if (IsLoaded)
			{
				await Task.Run(() =>
				{
					foreach (IDeviceBase deviceFromRouter in devicesFromRouter)
					{
						if (CheckForComplianceDevice(deviceFromRouter))
						{
							//пробуем получить устройство из имеющихся списков
							Switch device = (Switch)GetDevice(deviceFromRouter.ID);

							if (device != null && CheckCorresponding(deviceFromRouter, device))
							{					
								device.IP = deviceFromRouter.IP;
								device.Name = deviceFromRouter.Name;
								device.IsConnected = true;
								device.State = CurrentState.TurnedOff; //статус будем запрашивать отсюда

								//if (device is регул. выключатель)
								//{
									//запрос на уровень напряжения
								//}
							}
							else if (device == null)
							{
								//TODO: обработать случай, если утройство найдено но в базе его нет
							}
							else
							{
								//не соответствие устройств
							}
						}
					}
				});
			}
		}

		//public override IEnumerable<IDeviceBase> GetAllDevices()
		//{
		//	List<IDeviceBase> allDevices = new List<IDeviceBase>();
		//	allDevices.AddRange(SwitchList.Cast<IDeviceBase>());
		//	allDevices.AddRange(OutletList.Cast<IDeviceBase>());

		//	return allDevices;
		//}

		public override IEnumerator GetEnumerator()
		{
			List<IDeviceBase> allDevices = new List<IDeviceBase>();
			allDevices.AddRange(SwitchList.Cast<IDeviceBase>());
			allDevices.AddRange(OutletList.Cast<IDeviceBase>());

			return allDevices.GetEnumerator();
		}

		//private IDeviceInfo[] ConvertToDeviceInfos(IEnumerable<IDeviceBase> devices)
		//{
		//	List<DeviceInfo> deviceInfos = new List<DeviceInfo>();

		//	foreach (IDeviceBase device in devices)
		//	{
		//		DeviceInfo deviceInfo = new DeviceInfo
		//		{
		//			Description = device.Description,
		//			DeviceType = (int)device.DeviceType,
		//			FirmwareType = (int)device.FirmwareType,
		//			MacAddress = device.Mac.ToString()
		//		};

		//		deviceInfos.Add(deviceInfo);
		//	}

		//	return deviceInfos.ToArray();
		//}

		//private IEnumerable<ISwitch> ConvertToSwitches(IDeviceInfo[] deviceInfos)
		//{
		//	List<ISwitch> devices = new List<ISwitch>();

		//	foreach(IDeviceInfo deviceInfo in deviceInfos)
		//	{
		//		MacAddress mac = new MacAddress(deviceInfo.MacAddress);
		//		FirmwareType firmwareType = (FirmwareType)deviceInfo.FirmwareType;
		//		DeviceType deviceType = (DeviceType)deviceInfo.DeviceType;

		//		Switch device = new Switch(mac, firmwareType, deviceType)
		//		{
		//			Description = deviceInfo.Description,
		//			ID = deviceInfo.ID,
		//			State = CurrentState.TurnedOff
		//		};

		//		devices.Add(device);
		//	}

		//	return devices;
		//}



		/// <summary>
		/// Проверить соответствие устройства с роутера с имеющимся устройсвом
		/// </summary>
		/// <param name="deviceFromRouter"></param>
		/// <param name="device"></param>
		/// <returns></returns>
		private bool CheckCorresponding(IDeviceBase deviceFromRouter, IDeviceBase device)
		{
			return deviceFromRouter.Mac == device.Mac 
				&& deviceFromRouter.FirmwareType == device.FirmwareType 
				&& deviceFromRouter.DeviceType == device.DeviceType;
		}

		//private async Task<IResultOperationSave> SaveDevices(IEnumerable<IDeviceBase> devices, ISwitchesLoader loader)
		//{
		//	IDeviceInfo[] deviceInfos = ConvertToDeviceInfos(devices);
		//	return  await loader.SaveDevices(deviceInfos);
		//}

		private async Task FillIDs(IDeviceBase[] newDevices, int[] newIDs)
		{
			Communicator communicator = new Communicator();

			//передать новым устройствам их ID
			for (int i = 0; i < newDevices.Length; i++)
			{
				IDeviceBase device = newDevices[i];
				int newID = newIDs[i];

				if (device is Switch sO)
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

		private IDeviceBase GetDevice(int id)
		{
			IDeviceBase device = _switches.GetByKey(id);

			if (device == null)
				device = _outlets.GetByKey(id);

			return device;
		}

		//private IEnumerable<ISwitchOutlet> FakeDevices()
		//{
		//	List<ISwitchOutlet> devs = new List<ISwitchOutlet>();

		//	for(int i = 1; i < 10; i++)
		//	{
		//		SwitchOutlet dev = new SwitchOutlet(new MacAddress("EC:FA:BC:85:8F:18"), FirmwareType.ESP_8266, DeviceType.Switch)
		//		{
		//			Description = "Dev" + i,
		//			ID = i
		//		};

		//		devs.Add(dev);
		//	}

		//	return devs;
		//}


	}
}

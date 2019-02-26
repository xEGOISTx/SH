using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataManager;
using SHBase;
using SHBase.Communication;
using SHBase.DevicesBaseComponents;
using Switches;

namespace Switches
{
	public class Switches : Devices, ISwitches
	{
		private readonly SwitchList _switches = new SwitchList();
		private readonly OutletList _outlets = new OutletList();
		private readonly List<LoadableList> _lists = new List<LoadableList>();

		public Switches()
		{
			_lists.Add(_switches);
			_lists.Add(_outlets);
		}


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
					await SaveDevices(switches, _switches.Loader, _switches.Convertor);
					_switches.AddRange(switches);
				}

				if (outlets.Count > 0)
				{
					await SaveDevices(outlets, _outlets.Loader, _outlets.Convertor);
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
					foreach(LoadableList list in _lists)
					{
						ISwitchesLoader loader = list.Loader;
						IResultOperationLoad result = await loader.LoadDevices();

						if(result.Success)
						{
							IEnumerable<IBaseSwitch> devices = _switches.Convertor.ConvertToDevices(result.DeviceInfos);
							list.AddRange(devices);
						}
					}

					IsLoaded = true;
					return true;
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
		public override async Task Synchronization(IEnumerable<IDeviceBase> devicesFromRouter, Communicator communicator)
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
							IBaseSwitch device = GetDevice(deviceFromRouter.ID);

							if (device != null && CheckCorresponding(deviceFromRouter, device))
							{
								BaseSwitch dev = (device as BaseSwitch);
								dev.IP = deviceFromRouter.IP;
								dev.Name = deviceFromRouter.Name;
								dev.IsConnected = true;

								//TODO: здесь будет вызов метода GetOwnParams
								dev.State = CurrentState.TurnedOff; //статус будем запрашивать отсюда

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

		public override IEnumerator GetEnumerator()
		{
			List<IDeviceBase> allDevices = new List<IDeviceBase>();
			allDevices.AddRange(SwitchList.Cast<IDeviceBase>());
			allDevices.AddRange(OutletList.Cast<IDeviceBase>());

			return allDevices.GetEnumerator();
		}

		public override async Task<IEnumerable<IDeviceBase>> GetNotConnectedDevicesAsync(Communicator communicator)
		{
			List<IDeviceBase> devs = new List<IDeviceBase>();

			foreach (IDeviceBase device in this)
			{
				bool result = await communicator.CheckConnection(device);

				if (!result)
				{
					(device as BaseSwitch).IsConnected = false;
					devs.Add(device);
				}
			}

			return devs;
		}

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

		private async Task<bool> SaveDevices(IEnumerable<IBaseSwitch> devices, ISwitchesLoader loader, DBConvertor convertor)
		{
			IDeviceInfo[] deviceInfos = convertor.ConvertToDeviceInfos(devices);
			IResultOperationSave result = await loader.SaveDevices(deviceInfos);

			if(result.Success)
			{
				await FillIDs(devices.ToArray(), result.NewIDs);
			}

			return result.Success;
		}

		private async Task FillIDs(IBaseSwitch[] newDevices, int[] newIDs)
		{
			Communicator communicator = new Communicator();

			//передать новым устройствам их ID
			for (int i = 0; i < newDevices.Length; i++)
			{
				IDeviceBase device = newDevices[i];
				int newID = newIDs[i];

				(device as BaseSwitch).ID = newID;

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
					throw new Exception("Передаваемый ID не может быть равен или меньше 0");
				}
			}
		}

		private IBaseSwitch GetDevice(int id)
		{
			IBaseSwitch device = null;

			foreach (LoadableList list in _lists)
			{
				device = list.GetByKey(id);
				if(device != null)
				{
					break;
				}
			}


			//IDeviceBase device = _switches.GetByKey(id);

			//if (device == null)
			//	device = _outlets.GetByKey(id);

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

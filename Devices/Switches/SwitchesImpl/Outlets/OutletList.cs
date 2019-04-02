using DataManager;
using SHBase;
using SHBase.Communication;
using SHBase.DevicesBaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Switches
{
	public class OutletList : DeviceBaseList, IOutletList//SwitchesAndOutletsBaseList<IOutlet>, IOutletList
	{
		public OutletList() : base(DeviceTypes.OUTLET) { }

		public ISwitchEditor SwitchEditor { get; } = new SwitchEditor();

		public override IDeviceBase CreateDevice(int id, string name, IPAddress iP, FirmwareType firmwareType, MacAddress mac)
		{
			return new Outlet(iP, mac, firmwareType)
			{
				Name = name,
				ID = id,
				Description = name
			};
		}

		public IOutlet GetByKey(int key)
		{
			return devices.ContainsKey(key) ? (IOutlet)devices[key] : null;
		}

		public override async Task<IEnumerable<IDeviceBase>> GetNotConnectedDevicesAsync(Communicator communicator)
		{
			List<IDeviceBase> notConndevices = new List<IDeviceBase>();

			await Task.Run(async () =>
			{
				foreach (IDeviceBase device in this)
				{
					bool result = await communicator.CheckConnection(device);

					if (!result)
					{
						(device as Outlet).IsConnected = false;
						notConndevices.Add(device);
					}
				}
			});

			return notConndevices;
		}

		public override async Task<bool> Load()
		{
			DevicesLoader loader = new DevicesLoader(DevicesType);
			IResultOperationLoad result = await loader.LoadDevices();

			await Task.Run(() =>
			{
				if (result.Success)
				{
					foreach (IDeviceInfo deviceInfo in result.DeviceInfos)
					{
						var mac = new MacAddress(deviceInfo.MacAddress);
						var fType = (FirmwareType)deviceInfo.FirmwareType;

						Outlet sw = new Outlet(mac, fType)
						{
							ID = deviceInfo.ID,
							Description = deviceInfo.Description
						};

						Add(sw);
					}
				}
			});

			return result.Success;
		}

		public async override Task<int[]> Save(IEnumerable<IDeviceBase> devices)
		{
			DevicesLoader loader = new DevicesLoader(DevicesType);
			IDeviceInfo[] infos = MakeInfos(devices);
			IResultOperationSave result = await loader.SaveDevices(infos);

			return result.NewIDs;
		}

		public override async Task Synchronization(IEnumerable<IDeviceBase> devicesFromRouter, Communicator communicator)
		{
			if (IsLoaded)
			{
				await Task.Run(() =>
				{
					foreach (IDeviceBase deviceFromRouter in devicesFromRouter)
					{
						IDeviceBase device = GetByKey(deviceFromRouter.ID);

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
				});
			}
		}

		private IDeviceInfo[] MakeInfos(IEnumerable<IDeviceBase> devices)
		{
			List<IDeviceInfo> infos = new List<IDeviceInfo>(devices.Count());

			foreach (IDeviceBase device in devices)
			{
				DeviceInfo info = new DeviceInfo
				{
					Description = device.Description,
					DeviceType = device.DeviceType,
					FirmwareType = (int)device.FirmwareType,
					MacAddress = device.Mac.ToString()
				};

				infos.Add(info);
			}

			return infos.ToArray();
		}

	}
}

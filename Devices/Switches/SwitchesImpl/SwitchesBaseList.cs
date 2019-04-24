using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataManager;
using SHBase;
using SHBase.Communication;
using SHBase.DevicesBaseComponents;

namespace Switches
{
	public abstract class SwitchesBaseList : DeviceBaseList
	{
		protected readonly IDataManager _data;

		public SwitchesBaseList(int deviceType, IDataManager dataManager) : base(deviceType)
		{
			_data = dataManager;
		}


		public async override Task<IEnumerable<IDeviceBase>> GetNotConnectedDevicesAsync(Communicator communicator)
		{
			List<IDeviceBase> notConndevices = new List<IDeviceBase>();

			await Task.Run(async () =>
			{
				foreach (IDeviceBase device in this)
				{
					bool result = await communicator.CheckConnection(device);

					if (!result)
					{
						(device as BaseSwitch).IsConnected = false;
						notConndevices.Add(device);
					}
				}
			});

			return notConndevices;
		}


		public async override Task<int[]> Save(IEnumerable<IDeviceBase> devices)
		{
			IResultOperationSave result = null;

			await Task.Run(() =>
			{
				IDeviceInfo[] infos = MakeInfos(devices);
				result = _data.SaveDevices(infos);
			});


			return result.NewIDs;
		}

		public async override Task Synchronization(IEnumerable<IDeviceBase> devicesFromRouter, Communicator communicator)
		{
			if (IsLoaded)
			{
				await Task.Run(() =>
				{
					foreach (IDeviceBase deviceFromRouter in devicesFromRouter)
					{
						BaseSwitch device = _devices.ContainsKey(deviceFromRouter.ID) ? (BaseSwitch)_devices[deviceFromRouter.ID] : null;

						if (device != null && CheckCorresponding(deviceFromRouter, device))
						{
							device.IP = deviceFromRouter.IP;
							device.Name = deviceFromRouter.Name;
							device.IsConnected = true;

							//TODO: здесь будет вызов метода GetOwnParams
							device.State = CurrentState.TurnedOff; //статус будем запрашивать отсюда

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

		public override async Task<bool> Load()
		{
			IResultOperationLoad result = null;

			if (!IsLoaded)
			{
				await Task.Run(() =>
				{
					result = _data.LoadDevices(DevicesType);

					if (result.Success)
					{
						foreach (IDeviceInfo deviceInfo in result.DeviceInfos)
						{
							IBaseSwitch sw = CreteDeviceFromDeviceInfo(deviceInfo);
							Add(sw);
						}

						IsLoaded = result.Success;
					}
				});
			}

			return result != null ? result.Success : false;
		}

		protected abstract IBaseSwitch CreteDeviceFromDeviceInfo(IDeviceInfo deviceInfo);

		protected IDeviceInfo[] MakeInfos(IEnumerable<IDeviceBase> devices)
		{
			List<IDeviceInfo> infos = new List<IDeviceInfo>(devices.Count());

			foreach (IDeviceBase device in devices)
			{
				IDeviceInfo info = _data.CreateDeviceInfo(device.Description, device.DeviceType, (int)device.FirmwareType, device.Mac.ToString());
				infos.Add(info);
			}

			return infos.ToArray();
		}
	}
}

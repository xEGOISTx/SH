using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataManager;
using SHBase;
using SHBase.DevicesBaseComponents;

namespace Switches
{
	public class SwitchList : SwitchesBaseList , ISwitchList
	{
		//private readonly IDataManager _data;

		public SwitchList(IDataManager dataManager) : base(DeviceTypes.SWITCH, dataManager)
		{
			//_data = dataManager;
			SwitchEditor = new SwitchEditor(_data);
		}

		public ISwitchEditor SwitchEditor { get; }

		public override IDeviceBase CreateDevice(int id, string name, IPAddress iP, FirmwareType firmwareType, MacAddress mac)
		{
			return new Switch(iP, mac, firmwareType)
			{
				Name = name,
				ID = id,
				Description = name
			};
		}

		public ISwitch GetByKey(int key)
		{
			return _devices.ContainsKey(key) ? (ISwitch)_devices[key] : null;
		}

		protected override IBaseSwitch CreteDeviceFromDeviceInfo(IDeviceInfo deviceInfo)
		{
			var mac = new MacAddress(deviceInfo.MacAddress);
			var fType = (FirmwareType)deviceInfo.FirmwareType;

			return new Switch(mac, fType)
			{
				ID = deviceInfo.ID,
				Description = deviceInfo.Description
			};			
		}


		//public override async Task<bool> Load()
		//{
		//	IResultOperationLoad result = null;

		//	if (!IsLoaded)
		//	{
		//		await Task.Run(() =>
		//		{
		//			result = _data.LoadDevices(DeviceTypes.SWITCH);

		//			if (result.Success)
		//			{
		//				foreach (IDeviceInfo deviceInfo in result.DeviceInfos)
		//				{
		//					var mac = new MacAddress(deviceInfo.MacAddress);
		//					var fType = (FirmwareType)deviceInfo.FirmwareType;

		//					Switch sw = new Switch(mac, fType)
		//					{
		//						ID = deviceInfo.ID,
		//						Description = deviceInfo.Description
		//					};

		//					Add(sw);
		//				}

		//				IsLoaded = result.Success;
		//			}
		//		});
		//	}

		//	return result != null ? result.Success : false;
		//}



		//public override async Task<IEnumerable<IDeviceBase>> GetNotConnectedDevicesAsync(Communicator communicator)
		//{
		//	List<IDeviceBase> notConndevices = new List<IDeviceBase>();

		//	await Task.Run(async () =>
		//	{
		//		foreach (IDeviceBase device in this)
		//		{
		//			bool result = await communicator.CheckConnection(device);

		//			if (!result)
		//			{
		//				(device as Switch).IsConnected = false;
		//				notConndevices.Add(device);
		//			}
		//		}
		//	});

		//	return notConndevices;
		//}
		//public async override Task<int[]> Save(IEnumerable<IDeviceBase> devices)
		//{
		//	IResultOperationSave result = null;

		//	await Task.Run(() =>
		//	{
		//		IDeviceInfo[] infos = MakeInfos(devices);
		//		result = _data.SaveDevices(infos);
		//	});


		//	return result.NewIDs;
		//}

		//public override async Task Synchronization(IEnumerable<IDeviceBase> devicesFromRouter, Communicator communicator)
		//{
		//	if (IsLoaded)
		//	{
		//		await Task.Run(() =>
		//		{
		//			foreach (IDeviceBase deviceFromRouter in devicesFromRouter)
		//			{
		//				IDeviceBase device = GetByKey(deviceFromRouter.ID);

		//				if (device != null && CheckCorresponding(deviceFromRouter, device))
		//				{
		//					BaseSwitch dev = (device as BaseSwitch);
		//					dev.IP = deviceFromRouter.IP;
		//					dev.Name = deviceFromRouter.Name;
		//					dev.IsConnected = true;

		//					//TODO: здесь будет вызов метода GetOwnParams
		//					dev.State = CurrentState.TurnedOff; //статус будем запрашивать отсюда

		//				}
		//				else if (device == null)
		//				{
		//					//TODO: обработать случай, если утройство найдено но в базе его нет
		//				}
		//				else
		//				{
		//					//не соответствие устройств
		//				}
		//			}
		//		});
		//	}
		//}

		//private IDeviceInfo[] MakeInfos(IEnumerable<IDeviceBase> devices)
		//{
		//	List<IDeviceInfo> infos = new List<IDeviceInfo>(devices.Count());

		//	foreach (IDeviceBase device in devices)
		//	{
		//		IDeviceInfo info = _data.CreateDeviceInfo(device.Description, device.DeviceType, (int)device.FirmwareType, device.Mac.ToString());
		//		infos.Add(info);
		//	}

		//	return infos.ToArray();
		//}

	}
}

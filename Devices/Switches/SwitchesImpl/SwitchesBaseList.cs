using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SHBase;
using SHBase.DevicesBaseComponents;

namespace Switches
{
	public abstract class SwitchesBaseList : DeviceBaseList
	{
		public SwitchesBaseList(int deviceType) : base(deviceType)
		{

		}


		public async override Task Synchronization(IEnumerable<IDeviceBase> devicesFromRouter)
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


		public override void RefreshDevicesConnectionState(IEnumerable<IDeviceConnectionInfo> connectionInfos)
		{
			foreach(IDeviceConnectionInfo connectionInfo in connectionInfos)
			{
				(connectionInfo.Device as BaseSwitch).IsConnected = connectionInfo.ConnectionState;
			}
		}
	}
}

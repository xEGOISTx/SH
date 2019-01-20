using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHBase.Communication;
using SHBase.DevicesBaseComponents;

namespace Switches.SwitchesOutlets
{
	public class Swithes : ISwithes
	{
		private readonly SwitchesAndOutletsList _switchesAndOutletsList = new SwitchesAndOutletsList();

		public ISwitchesAndOutletsList SwitchesAndOutletsList => _switchesAndOutletsList;

		public bool IsLoaded { get; private set; }

		public async void Load()
		{

			//ISwitchesAndOutletsLoader loader = _switchesAndOutletsList.GetLoader();
			//await loader.Load();
		}

		/// <summary>
		/// Проверить соответствует ли устройство этому набору устройств
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		public bool CheckForComplianceDevice(IDeviceBase device)
		{
			return device.DeviceType == DeviceType.Outlet || device.DeviceType == DeviceType.Switch;
		}

		/// <summary>
		/// Добавить и сохранить новые устройства
		/// </summary>
		/// <param name="newDevices"></param>
		public async Task<bool> AddAndSaveNewDevices(IEnumerable<IDeviceBase> newDevices)
		{
			return await Task.Run(async () =>
			{
				List<ISwitchOutlet> myDevices = new List<ISwitchOutlet>();

				foreach (IDeviceBase device in newDevices.Where(d => d.ID < 1))
				{
					if (CheckForComplianceDevice(device))
					{
						SwitchOutlet sO = new SwitchOutlet(device.Mac, device.FirmwareType, device.DeviceType)
						{
							Description = device.Name,
							IP = device.IP,
							IsConnected = device.IsConnected,
							Name = device.Name
						};

						myDevices.Add(sO);
					}
				}

				if (myDevices.Count > 0)
				{
					ISwitchesAndOutletsLoader loader = _switchesAndOutletsList.GetLoader();
					int[] newIDs = await loader.SaveDevices(myDevices);

					Communicator communicator = new Communicator();

					//передать новым устройствам их ID
					for (int i = 0; i < myDevices.Count; i++)
					{
						SwitchOutlet device = myDevices[i] as SwitchOutlet;
						int newID = newIDs[i];

						device.ID = newID;

						OperationResult result = await communicator.SendIdToDevice(newID, device);

						if(!result.Success)
						{
							throw new Exception("Не удалось отправить ID устройству. " + result.ErrorMessage);
						}
					}

					_switchesAndOutletsList.AddRange(myDevices);

					return true;
				}

				return false;
			});
		}
	}
}

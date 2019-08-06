using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SH.Communication;
using SH.Core;
using SH.DataManagement;
using MList = SH.Node.IManegedList<SH.Core.IDeviceBase>;

namespace SH.Node
{
	internal class DevicesManager
	{
		private readonly Dictionary<int, MList> _devsLists = new Dictionary<int, MList>();
		private bool _searchModeIsActive;
		private bool _searchStop;
		private readonly Communicator _communicator = new Communicator();

		public DevicesManager(IEnumerable<IManegedList<IDeviceBase>> devices)
		{
			foreach(MList devsList in devices)
			{
				_devsLists.Add(devsList.DevicesType, devsList);
			}
		}


		public void ActivateSearchModeAsync(IConnector connector, IConnectionParams connectionParams)
		{
			if(!_searchModeIsActive)
			{
				_searchModeIsActive = true;

				Task.Run(async() =>
				{
					while(true)
					{
						connector.Clear();

						connector.FindAPs(connectionParams.APSSIDsForSearch);
						if(connector.CountFoundAP > 0)
						{
							foreach(IAP aP in connector.APs)
							{
								aP.SetPasswordToConnected(connectionParams.DeviceAPPassword);
								connector.ConnectTo(aP);

								//TODO: сделять возврат результата операции как везде 
								IPAddress ip = await _communicator.GetLocalIPFromDevice(connectionParams.DeviceDafaultIP);

								if (ip != null)
								{
									if (ip == Consts.ZERO_IP)
									{
										IOperationResult sendHIPRes = await _communicator.SendHostIPToDevice(connectionParams.DeviceDafaultIP, connector.GetHostIP());
										IOperationResult sendConnParamsRes = await _communicator.SendConnectionParamsToDevice(connectionParams.DeviceDafaultIP, connectionParams.ConnectionParamsToRouter.ConnectionParams);
									}
								}
								else
								{
									//TODO: тут будет запись в лог
								}


								connector.Disconnect(aP);

								if (_searchStop)
									break;
							}
						}


						if (_searchStop)
						{
							connector.Clear();
							_searchModeIsActive = false;
							_searchStop = false;
							break;
						}
					}
				});
			}
		}

		public void DeactivateSearchMode()
		{
			if (_searchModeIsActive)
				_searchStop = true;
		}

		public async Task<IOperationResult> RefreshDevices()
		{
			throw new NotImplementedException();
		}

		public async Task<IOperationResult> LoadDataFromRepository(IDevicesLoader devicesLoader)
		{
			return await Task.Run(() =>
			{
				Dictionary<int, IDevice[]> loadedDevs = new Dictionary<int, IDevice[]>();

				try
				{
					//патаемся загрузить устройства
					foreach (MList mList in _devsLists.Values)
					{
						IOperationResultDevicesLoad loadRes = devicesLoader.LoadDevices(mList.DevicesType);

						if(loadRes.Success)
						{
							loadedDevs.Add(mList.DevicesType, loadRes.Devices);
						}
						else
						{
							return new OperationResult { ErrorMessage = loadRes.ErrorMessage };
						}
					}


					//создаём устройства и заполняем управляемые списки
					foreach(var devs in loadedDevs)
					{
						int devicesType = devs.Key;
						IDevice[] devices = devs.Value;
						MList curMList = _devsLists[devicesType];
						IDeviceEditor curMListEditor = curMList.Editor;

						foreach (IDevice loadedDevice in devices)
						{
							IDeviceBase device = curMListEditor.CreateDevice();

							curMListEditor.ChangeID(device, loadedDevice.ID);
							curMListEditor.ChangeDescription(device, loadedDevice.Description);
							curMListEditor.ChangeDeviceType(device, loadedDevice.DeviceType);
							curMListEditor.ChangeFirmwareType(device, loadedDevice.FirmwareType);
							curMListEditor.ChangeMacAddress(device, new MacAddress(loadedDevice.MacAddress));

							curMList.Add(device);
						}
					}		
				}
				catch(Exception ex)
				{
					return new OperationResult { ErrorMessage = ex.Message };
				}

				return new OperationResult { Success = true };
			});
		}
	}
}

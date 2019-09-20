using SH.Communication;
using SH.Core;
using SH.Core.DevicesComponents;
using SH.DataManagement;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MList = SH.Core.DevicesComponents.IManegedList<SH.Core.DevicesComponents.IDevice>;
using System.Linq;
using SH.DataPorts;

namespace SH.Node
{
	internal class DevicesManager
	{
		private bool _searchModeIsActive;
		private bool _searchStop;
		private readonly IDevicesLoader _loader;
		private Queue<IDeviceRequest> _requestsQueue = new Queue<IDeviceRequest>();
		private bool _requestsProcessed;
        private bool _refreshIsActive;

        public DevicesManager(IEnumerable<IManegedList<IDevice>> devices, Communicator communicator,IDevicesLoader loader)
		{
			foreach(MList devsList in devices)
			{
				DevsLists.Add(devsList.DevicesType, devsList);
			}

            Communicator = communicator;
            Communicator.RequestFromDevice += Communicator_RequestFromDevice;
            _loader = loader;
		}

        public Dictionary<int, MList> DevsLists { get; } = new Dictionary<int, MList>();

        public Communicator Communicator { get; }

		public void ActivateSearchModeAsync(IConnector connector, ConnectionParams connectionParams)
		{
			if(!_searchModeIsActive)
			{
				_searchModeIsActive = true;
				ConnectionParamsToDevice connectionParamsToDevice = connectionParams.GetConnectionParamsToDevice();

				Task.Run(async() =>
				{		
					while (true)
					{
						connector.Clear();

						connector.FindAPs(connectionParamsToDevice.APSSIDsForSearch);
						if(connector.CountFoundAP > 0)
						{
							foreach(IAP aP in connector.APs)
							{
								aP.SetPasswordToConnected(connectionParamsToDevice.DeviceAPPassword);
								connector.ConnectTo(aP);

								//TODO: сделать возврат результата операции как везде 
								IPAddress ip = await Communicator.GetLocalIPFromDevice(connectionParamsToDevice.DeviceDafaultIP);

								if (ip != null)
								{
									if (ip == Consts.ZERO_IP)
									{									
										IOperationResult sendHIPRes = await Communicator.SendHostIPToDevice(connectionParamsToDevice.DeviceDafaultIP, connector.GetHostIP());
										IOperationResult sendConnParamsRes = await Communicator.SendConnectionParamsToDevice(connectionParamsToDevice.DeviceDafaultIP, connectionParams.GetConnectionParamsToRouter().ConnectionParams);
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

        //public void HandleRequestAsync(string request)
        //{
        //    Task.Run(() =>
        //    {
        //        string[] requestParams = request.Split('&');

        //        DeviceRequest deviceRequest = new DeviceRequest
        //        {
        //            RequestType = int.Parse(requestParams[0]),
        //            DeviceType = int.Parse(requestParams[1]),
        //            DeviceIP = IPAddress.Parse(requestParams[2])
        //        };

        //        _requestsQueue.Enqueue(deviceRequest);

        //        if (!_requestsProcessed)
        //        {
        //            ExecuteHandleRequests();
        //        }
        //    });
        //}

        public IOperationResult LoadDevices()
        {
            Dictionary<int, IDeviceData[]> loadedDevs = new Dictionary<int, IDeviceData[]>();
            IOperationResult result = new OperationResult { Success = true };

            foreach (MList mList in DevsLists.Values)
            {
                IOperationResultDevicesLoad loadRes = _loader.LoadDevices(mList.DevicesType);

                if (loadRes.Success)
                {
                    loadedDevs.Add(mList.DevicesType, loadRes.Devices);
                }
                else
                {
                    result = loadRes;
                    break;
                }
            }

            if (result.Success)
            {
                result = AddLoadedDevicesToMLists(loadedDevs);
            }

            return result;
        }

        public async Task<IOperationResult> RefreshDevicesAsync(ConnectionParams connectionParams, IRouterParser routerParser)
        {
            IOperationResult result = new OperationResult { Success = true };

            if (!_searchModeIsActive && !_refreshIsActive)
            {
                _refreshIsActive = true;
                ConnectionParamsToRouter connToRouter = connectionParams.GetConnectionParamsToRouter();

                if (connToRouter.RouterUriToParse.AbsoluteUri != string.Empty
                    && connToRouter.Credentials.Login != string.Empty
                    && connToRouter.Credentials.Password != string.Empty)
                {
                    await Task.Run(async () =>
                    {
                        IParseOperationResult parseRes = routerParser.GetActiveIPs(connToRouter.RouterUriToParse, connToRouter.Credentials);

                        if (parseRes.Success)
                        {
                            //получаем изменённые состояния подключений
                            IEnumerable<IDeviceConnectionState> connStates = await GetChangedConnectionStates(parseRes.IPs);

                            foreach (IDeviceConnectionState connState in connStates)
                            {
                                MList curMList = DevsLists[connState.Device.DeviceType];
                                curMList.RefreshDeviceConnectionState(connState);

                                foreach(DeviceCommand command in connState.Device.Commands)
                                {
                                    command.OwnerIP = connState.IP;
                                }
                            }
                        }
                        else
                        {
                            result = parseRes;
                        }
                    });
                }

                _refreshIsActive = false;
            }
            else if (_searchModeIsActive)
            {
                (result as OperationResult).Success = false;
                (result as OperationResult).ErrorMessage = "Не допускается обновление устройств во время поиска устройств!";
            }

            return result;
        }

        private async Task<IEnumerable<IDeviceConnectionState>> GetChangedConnectionStates(IEnumerable<string> iPs)
        {
             //TODO: может быть форсмажорный случай когда одинаковые ID у разных устройств


            Dictionary<int, DeviceConnectionState> connStates = new Dictionary<int,DeviceConnectionState>();

            //получаем состояния подкл. который изменились на true
            foreach (string sIP in iPs)
            {
                IPAddress ip = IPAddress.Parse(sIP);
                int id = await Communicator.GetDeviceIDAsync(ip);

                if(id != -1)
                {
                    IDevice device = DevicesInnerRegister.GetByID(id);

                    if (device != null)
                    {
                        bool isConnected = device.IP != null && device.IP != Consts.ZERO_IP && device.IP == ip;

                        if (!isConnected)
                        {
                            connStates.Add(device.ID, new DeviceConnectionState { Device = device, IP = ip, ConnectionState = true });
                        }
                    }
                    else
                    {
                        //нераспознанное устройство
                        //пытаемся идентифицировать
                        //если удалось идентифицировать сбрасываем устройсво или пробуем восстановить
                        //если не удалось идентифицировать принять меры по безопаснсти затем отключить устройство то сети принудительно 
                        //log
                    }

                }
            }

            //получаем состояния подкл. который изменились на false
            IEnumerable<IDevice> notConndevices = DevicesInnerRegister.List.Where(device => device.IsConnected);
            foreach (IDevice device in notConndevices)
            {
                if (!connStates.ContainsKey(device.ID))
                {
                    bool isConnected = await Communicator.CheckConnection(device);

                    if (!isConnected)
                    {
                        connStates.Add(device.ID, new DeviceConnectionState { Device = device, IP = Consts.ZERO_IP, ConnectionState = false });
                    }
                }
            }


            return connStates.Values;
        }

        private IOperationResult AddLoadedDevicesToMLists(Dictionary<int, IDeviceData[]> loadedDevices)
        {
            OperationResult result = new OperationResult { Success = true };

            try
            {
                foreach (var devices in loadedDevices)
                {
                    int devsType = devices.Key;
                    MList curMList = DevsLists[devsType];
                    IDeviceData[] deviceDatas = devices.Value;

                    foreach (IDeviceData deviceData in deviceDatas)
                    {
                        IDevice device = MakeDeviceFromData(deviceData, curMList);
                        (device.Commands.Editor as DeviceCommandEditor).Apply += CommandsEditor_Apply;
                        DevicesInnerRegister.Add(device);
                    }
                }
            }
            catch(Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        private void ExecuteHandleRequests()
        {
            _requestsProcessed = true;

            Task.Run(async () =>
            {
                while (_requestsQueue.Count != 0)
                {
                    IDeviceRequest request = _requestsQueue.Dequeue();
                    IPAddress devIP = request.DeviceIP;

                    if (request.RequestType == 1)//1 = устройство успешно подключилось после передачи параметров подключения
                    {
                        if (DevsLists.ContainsKey(request.DeviceType))
                        {
                            GetBaseInfoOperationResult baseInfoRes = await Communicator.GetDeviceInfo(devIP);
                            GetDeviceCommandsOperationResult commandsRes = await Communicator.GetDeviceCommands(devIP);

                            if (baseInfoRes.Success && commandsRes.Success)
                            {
                                DeviceInfo dInfo = baseInfoRes.DeviceBasicInfo;
                                MList mList = DevsLists[dInfo.DeviceType];

                                List<DeviceCommand> deviceCommands = new List<DeviceCommand>();

                                foreach (DeviceCommandInfo commandInfo in commandsRes.CommandsInfos)
                                {
                                    IDefaultDeviceCommandParams defaultCommandParams = mList.GetDefaultParamsForCommand(commandInfo.ID);

                                    deviceCommands.Add(new DeviceCommand(devIP, commandInfo.ID, commandInfo.CommandName)
                                    {
                                        Description = defaultCommandParams.Description,
                                        VoiceCommand = defaultCommandParams.VoiceCommand
                                    });
                                }

                                int newID = DevicesInnerRegister.GetFreeID();
                                IDeviceData deviceData = MakeDeviceData(newID, dInfo, deviceCommands);
                                IOperationResult saveRes = _loader.SaveDevice(deviceData);

                                if (saveRes.Success)
                                {
                                    IOperationResult sendIdRes = await Communicator.SendIdToDevice(newID, devIP);

                                    if (sendIdRes.Success)
                                    {
                                        DeviceCommandList commands = new DeviceCommandList(newID);
                                        commands.AddRange(deviceCommands);
                                        DeviceCommandEditor editor = new DeviceCommandEditor(commands);
                                        editor.Apply += CommandsEditor_Apply;
                                        commands.Editor = editor;

                                        IDevice device = mList.AddNewDevice(newID, dInfo.DeviceType, devIP, dInfo.Mac, commands, dInfo.Name);
                                        DevicesInnerRegister.Add(device);
                                    }
                                    else
                                    {
                                        _loader.RemoveDevice(dInfo.ID);
                                        //log
                                        continue;
                                    }
                                }
                                else
                                {
                                    //log
                                }
                            }
                            else
                            {
                                //log
                            }
                        }
                        else
                        {
                            //log
                        }
                    }

                }

                _requestsProcessed = false;
            });

        }

        private void CommandsEditor_Apply(object sender, ApplyCommandsChangesEventArgs e)
        {
            IDeviceCommandData[] commandDatas = MakeCommandsData(e.OwnerID, e.EditedCommands);

            IOperationResult updateRes = _loader.UpdateDeviceCommands(commandDatas);

            if(!updateRes.Success)
            {
                e.Cancel = true;
                //log
            }
        }

        private void Communicator_RequestFromDevice(object sender, RequestEventArgs e)
        {
            if (e.Request.RequestType >= 0 && e.Request.RequestType <= 255)
            {
                _requestsQueue.Enqueue(e.Request);

                if (!_requestsProcessed)
                {
                    ExecuteHandleRequests();
                }
            }
            else
            {               
                if (DevsLists.ContainsKey(e.Request.DeviceType))
                {
                    DevsLists[e.Request.DeviceType].HandleDeviceRequest(e.Request);
                }
            }
        }

        //      private IDeviceData MakeDeviceData(IDevice device, IEnumerable<DeviceCommand> commands)
        //{
        //	return new DeviceData
        //	{
        //		ID = device.ID,
        //		Description = device.Description,
        //		DeviceType = device.DeviceType,
        //		MacAddress = device.Mac.ToString(),
        //              Commands = MakeCommandsData(commands)
        //	};
        //}

        private IDeviceData MakeDeviceData(int devID, DeviceInfo device, IEnumerable<DeviceCommand> commands)
        {
            return new DeviceData
            {
                ID = devID,
                Description = string.Empty,
                DeviceType = device.DeviceType,
                MacAddress = device.Mac.ToString(),
                Commands = MakeCommandsData(devID, commands)
            };
        }

        private IDeviceCommandData[] MakeCommandsData(int ownerID, IEnumerable<IDeviceCommand> deviceCommands)
		{
			List<IDeviceCommandData> deviceCommandDatas = new List<IDeviceCommandData>();

			foreach(DeviceCommand com in deviceCommands)
			{
				deviceCommandDatas.Add(new DeviceCommandData 
                { 
                    ID = com.ID,
                    OwnerID = ownerID,
                    Description = com.Description,
                    VoiceCommand = com.VoiceCommand,
                    CommandName = com.CommandName 
                });
			}

			return deviceCommandDatas.ToArray();
		}

        private IDevice MakeDeviceFromData(IDeviceData deviceData, MList targetMList)
        {          
            List<IDeviceCommand> deviceCommands = new List<IDeviceCommand>();

            foreach (IDeviceCommandData commandData in deviceData.Commands)
            {
                deviceCommands.Add(new DeviceCommand(Consts.ZERO_IP, commandData.ID, commandData.CommandName)
                {
                    Description = commandData.Description,
                    VoiceCommand = commandData.VoiceCommand
                });
            }

            DeviceCommandList commands = new DeviceCommandList(deviceData.ID);
            commands.AddRange(deviceCommands);
            commands.Editor = new DeviceCommandEditor(commands);
            IDevice device = targetMList.AddNewDevice(deviceData.ID, deviceData.DeviceType, Consts.ZERO_IP, new MacAddress(deviceData.MacAddress), commands, deviceData.Description);
            return device;
        }
	}
}

using SH.Communication;
using SH.Core;
using SH.DataManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SH.Core.DevicesComponents;
using SH.DataPorts;

namespace SH.Node
{
	public sealed class SHNode
	{
		private readonly DevicesManager _devicesManager;
		private readonly IDataLoader _loader;
		private readonly IConnector _connector;
		private readonly ConnectionParams _connectionParams;
		private readonly IRouterParser _routerParser;
		private bool _nodeIsInit;

		public SHNode(IEnumerable<IManegedList<IDevice>> devices, IDataLoader loader, IConnector connector, IRouterParser routerParser, IDevicesRequestsListener requestsListener)
		{
            _devicesManager = new DevicesManager(devices, new Communicator(requestsListener), loader.GetDevicesLoader());
			_loader = loader;
			_connector = connector;
			_routerParser = routerParser;

			_connectionParams =  new ConnectionParams();
			(_connectionParams.Editor as ConnectionParamsEditor).Apply += SHNode_ApplyConnParams;
		}


		public IConnectionParams ConnectionParams => _connectionParams;



        public async Task<IOperationResult> Start()
        {
            IOperationResult result = new OperationResult { Success = true };

            if (!_nodeIsInit)
            {
                return await Task.Run(async () =>
                {
                    //патаемся загрузить настройки для подключений
                    ILoadSettingOperationResult loadSettingRes = _loader.GetSettingsLoader().Load();

                    if (loadSettingRes.Success)
                    {
                        _connectionParams.InsertConnetionSettings(loadSettingRes.ConnectionSettings);

                        //пытаемся загрузить устройства
                        result = _devicesManager.LoadDevices();

                        if (result.Success)
                        {
                            //запускаем синхронизацию устройств
                            result = await _devicesManager.RefreshDevicesAsync(_connectionParams, _routerParser);

                            _nodeIsInit = true;
                            //запускаем прослушку запросов от устройств
                            await _devicesManager.Communicator.RequestsListener.StartListening();
                        }
                    }
                    else
                    {
                        result = loadSettingRes;
                    }

                    return result;
                });
            }
            else
            {
                return result;
            }
        }

        public async void RefreshDevicesAsync()
        {
            if (_nodeIsInit)
            {
                IOperationResult result = await _devicesManager.RefreshDevicesAsync(_connectionParams, _routerParser);
            }
        }

        public void ActivateDevicesSearchModeAsync()
		{
            if (_nodeIsInit)
                _devicesManager.ActivateSearchModeAsync(_connector, _connectionParams);
		}

		public void DeactivateDevicesSearchMode()
		{
            if (_nodeIsInit)
                _devicesManager.DeactivateSearchMode();
		}

		private void SHNode_ApplyConnParams(object sender, ApplyConnectionParamsEventArgs e)
		{
			IConnectionSettings connectionSettings = new ConnectionSettings(e.ConnectionParams);

			IOperationResult saveRes = _loader.GetSettingsLoader().Save(connectionSettings);

			if(!saveRes.Success)
			{
				e.Cancel = true;
			}
		}
	}
}

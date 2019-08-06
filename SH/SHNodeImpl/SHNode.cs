using SH.Communication;
using SH.Core;
using SH.DataManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SH.Node
{
	public class SHNode
	{
		private readonly DevicesManager _devicesManager;
		private readonly IDataLoader _loader;
		private readonly IConnector _connector;
		private ConnectionParams _connectionParams;
		private bool _nodeIsInit;

		public SHNode(IEnumerable<IManegedList<IDeviceBase>> devices, IDataLoader loader, IConnector connector)
		{
			_devicesManager = new DevicesManager(devices);
			_loader = loader;
			_connector = connector;

			_connectionParams =  new ConnectionParams();
			(_connectionParams.Editor as ConnectionParamsEditor).Apply += SHNode_ApplyConnParams;
		}


		public IConnectionParams ConnectionParams => _connectionParams;




		public async Task<IOperationResult> Start()
		{
			IOperationResult result = new OperationResult { Success = true };

			if (!_nodeIsInit)
			{
				await Task.Run(() =>
				{
					//патаемся загрузить настройки для подключений
					ILoadSettingOperationResult loadSettingRes = _loader.GetSettingsLoader().Load();

					if (loadSettingRes.Success)
					{
						_connectionParams.InsertConnetionSettings(loadSettingRes.ConnectionSettings);
					}
					else
					{
						result = loadSettingRes;
					}
				});

				if (!result.Success)
					return result;

				//патаемся загрузить устройства
				result = await _devicesManager.LoadDataFromRepository(_loader.GetDevicesLoader());

				if(result.Success)
				{
					_nodeIsInit = true;
				}
			}

			return result;
		}

		public void ActivateDevicesSearchModeAsync()
		{
			_devicesManager.ActivateSearchModeAsync(_connector, _connectionParams);
		}

		public void DeactivateDevicesSearchMode()
		{
			_devicesManager.DeactivateSearchMode();
		}

		private void SHNode_ApplyConnParams(object sender, ApplyConnectionParamsEventArg e)
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

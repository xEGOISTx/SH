using System;
using System.Collections.Generic;
using System.Net;
using SH.DataManagement;

namespace SH.Communication
{
	internal enum ParamName
	{
		RouterIP = 1,
		RouterSsid,
		RouterAPPassword,
		RouterLogin,
		RouterPassword,
		DeviceDafaultIP,
		DeviceAPPassword,
		APSSIDsForSearch
	}


	internal class ConnectionParams : IConnectionParams
	{
		public ConnectionParams()
		{
			ConnectionParamsToRouter = new ConnectionParamsToRouter
			{
				Credentials = new Core.Credentials(),
				ConnectionParams = new Core.ConnectionParamsToAP()
			};

			Editor = new ConnectionParamsEditor(this);
		}

		public ConnectionParams(IConnectionSettings connectionSettings) : base()
		{
			InsertConnetionSettings(connectionSettings);
		}


		public IAPSSIDs APSSIDsForSearch { get; set; } = new APSSIDs();

		public IPAddress DeviceDafaultIP { get; set; }

		public string DeviceAPPassword { get; set; } = string.Empty;

		public IConnectionParamsToRouter ConnectionParamsToRouter { get; set; }

		public IConnectionParamsEditor Editor { get; }

		public void InsertConnetionSettings(IConnectionSettings connectionSettings)
		{
			if (connectionSettings.Parameters != null)
			{
				Dictionary<ParamName, string> connParams = new Dictionary<ParamName, string>();

				foreach (IParameter parameter in connectionSettings.Parameters)
				{
					connParams.Add((ParamName)parameter.Index, parameter.Value);
				}

				if (connParams[ParamName.DeviceDafaultIP] != string.Empty)
				{
					DeviceDafaultIP = IPAddress.Parse(connParams[ParamName.DeviceDafaultIP]);
				}

				DeviceAPPassword = connParams[ParamName.DeviceAPPassword];

				APSSIDs targetAPSSIDs = APSSIDsForSearch as APSSIDs;
				string APSSIDs = connParams[ParamName.APSSIDsForSearch];
				targetAPSSIDs.Clear();
				targetAPSSIDs.AddRange(APSSIDs.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries));

				if (connParams[ParamName.RouterIP] != string.Empty)
				{
					(ConnectionParamsToRouter as ConnectionParamsToRouter).RouterIP = IPAddress.Parse(connParams[ParamName.RouterIP]);
				}

				Core.ConnectionParamsToAP paramsToRouter = ConnectionParamsToRouter.ConnectionParams as Core.ConnectionParamsToAP;
				paramsToRouter.SSID = connParams[ParamName.RouterSsid];
				paramsToRouter.Password = connParams[ParamName.RouterAPPassword];

				Core.Credentials routerCreadentials = ConnectionParamsToRouter.Credentials as Core.Credentials;
				routerCreadentials.Login = connParams[ParamName.RouterLogin];
				routerCreadentials.Password = connParams[ParamName.RouterPassword];
			}
		}

		public ConnectionParams GetCopy()
		{
			ConnectionParams connParamsCopy = new ConnectionParams
			{				
				DeviceAPPassword = DeviceAPPassword,
				APSSIDsForSearch = (APSSIDsForSearch as APSSIDs).GetCopy(),
				ConnectionParamsToRouter = (ConnectionParamsToRouter as ConnectionParamsToRouter).GetCopy()
			};

			if(DeviceDafaultIP != null)
			{
				connParamsCopy.DeviceDafaultIP = IPAddress.Parse(DeviceDafaultIP.ToString());
			}

			return connParamsCopy;
		}	
	}
}

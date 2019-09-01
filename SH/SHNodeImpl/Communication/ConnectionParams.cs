using System;
using System.Collections.Generic;
using System.Net;
using SH.DataManagement;
using SH.Node;

namespace SH.Communication
{
	internal enum ParamName
	{
		RouterUriToParse = 1,
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
		internal ConnectionParams()
		{
			Editor = new ConnectionParamsEditor(this);
		}

	
		public Uri RouterUriToParse { get; set; } = new Uri("");

        public string RouterLogin { get; set; } = string.Empty;

		public string RouterPassword { get; set; } = string.Empty;

        public string RouterSsid { get; set; } = string.Empty;

        public string RouterAPPassword { get; set; } = string.Empty;

        public IAPSSIDs APSSIDsForSearch { get; internal set; } = new APSSIDs();

		public IPAddress DeviceDafaultIP { get; set; }

		public string DeviceAPPassword { get; set; } = string.Empty;

        public IConnectionParamsEditor Editor { get; }



		internal ConnectionParamsToRouter GetConnectionParamsToRouter()
		{
			if (Editor.IsEditing)
			{
				return GetConnParams((Editor as ConnectionParamsEditor).OriginalCopyConnectionParams);
			}
			else
			{
				return GetConnParams(this);
			}

			ConnectionParamsToRouter GetConnParams(ConnectionParams target)
			{
				return new ConnectionParamsToRouter
				{
					RouterUriToParse = target.RouterUriToParse,
					Credentials = new Core.Credentials { Login = target.RouterLogin, Password = target.RouterPassword },
					ConnectionParams = new Core.ConnectionParamsToAP { SSID = target.RouterSsid, Password = target.RouterAPPassword }
				};
			}

		}

		internal ConnectionParamsToDevice GetConnectionParamsToDevice()
		{
			if (Editor.IsEditing)
			{
				return GetConnParams((Editor as ConnectionParamsEditor).OriginalCopyConnectionParams);
			}
			else
			{
				return GetConnParams(this);
			}

			ConnectionParamsToDevice GetConnParams(ConnectionParams target)
			{
				return new ConnectionParamsToDevice
				{
					APSSIDsForSearch = target.APSSIDsForSearch,
					DeviceAPPassword = target.DeviceAPPassword,
					DeviceDafaultIP = target.DeviceDafaultIP ?? Consts.ZERO_IP
				};
			}
		}

		internal void InsertConnetionSettings(IConnectionSettings connectionSettings)
		{
			if (connectionSettings.Parameters != null && connectionSettings.Parameters.Length > 0)
			{
				Dictionary<ParamName, string> connParams = new Dictionary<ParamName, string>();

				foreach (IParameter parameter in connectionSettings.Parameters)
				{
					connParams.Add((ParamName)parameter.Index, parameter.Value);
				}

				RouterSsid = connParams[ParamName.RouterSsid];
				RouterAPPassword = connParams[ParamName.RouterAPPassword];
				RouterLogin = connParams[ParamName.RouterLogin];
				RouterPassword = connParams[ParamName.RouterPassword];
				DeviceAPPassword = connParams[ParamName.DeviceAPPassword];

				if (connParams[ParamName.DeviceDafaultIP] != string.Empty)
				{
					DeviceDafaultIP = IPAddress.Parse(connParams[ParamName.DeviceDafaultIP]);
				}

				//if (connParams[ParamName.RouterUriToParse] != string.Empty)
				//{
				RouterUriToParse = new Uri(connParams[ParamName.RouterUriToParse]);
				//}

				APSSIDs targetAPSSIDs = APSSIDsForSearch as APSSIDs;
				string APSSIDs = connParams[ParamName.APSSIDsForSearch];
				targetAPSSIDs.Clear();
				targetAPSSIDs.AddRange(APSSIDs.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries));
			}
		}

		internal ConnectionParams GetCopy()
		{
			ConnectionParams connParamsCopy = new ConnectionParams
			{	
				RouterLogin = this.RouterLogin,
				RouterPassword = this.RouterPassword,
				RouterSsid = this.RouterSsid,
				RouterAPPassword = this.RouterAPPassword,				
				DeviceAPPassword = DeviceAPPassword,
				APSSIDsForSearch = (APSSIDsForSearch as APSSIDs).GetCopy(),
			};

			if(RouterUriToParse != null)
			{
				connParamsCopy.RouterUriToParse = new Uri(RouterUriToParse.AbsoluteUri);
			}

			if(DeviceDafaultIP != null)
			{
				connParamsCopy.DeviceDafaultIP = IPAddress.Parse(DeviceDafaultIP.ToString());
			}

			return connParamsCopy;
		}	
	}
}

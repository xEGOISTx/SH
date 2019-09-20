using SH.Communication;
using System.Linq;
using System.Collections.Generic;
using SH.DataPorts;

namespace SH.DataManagement
{
    internal class ConnectionSettings : IConnectionSettings
	{
		public ConnectionSettings() { }

		public ConnectionSettings(IConnectionParams connectionParams)
		{
			FillParameters(connectionParams);
		}


		public IParameter[] Parameters { get; set; }

		private void FillParameters(IConnectionParams connectionParams)
		{
			List<IParameter> parameters = new List<IParameter>();
			//IConnectionParamsToRouter paramsToRouter = connectionParams.ConnectionParamsToRouter;

			Parameter routerIP = new Parameter
			{
				Index = (int)ParamName.RouterUriToParse,
				Value =  connectionParams.RouterUriToParse.AbsoluteUri
			};
			parameters.Add(routerIP);

			Parameter routerSsid = new Parameter { Index = (int)ParamName.RouterSsid, Value = connectionParams.RouterSsid};
			parameters.Add(routerSsid);

			Parameter routerAPPassword = new Parameter { Index = (int)ParamName.RouterAPPassword, Value = connectionParams.RouterAPPassword };
			parameters.Add(routerAPPassword);

			Parameter routerLogin = new Parameter { Index = (int)ParamName.RouterLogin, Value = connectionParams.RouterLogin };
			parameters.Add(routerLogin);

			Parameter routerPassword = new Parameter { Index = (int)ParamName.RouterPassword, Value = connectionParams.RouterPassword };
			parameters.Add(routerPassword);

			Parameter deviceDafaultIP = new Parameter
			{
				Index = (int)ParamName.DeviceDafaultIP,
				Value = connectionParams.DeviceDafaultIP != null ? connectionParams.DeviceDafaultIP.ToString() : string.Empty
			};
			parameters.Add(deviceDafaultIP);

			Parameter deviceAPPassword = new Parameter { Index = (int)ParamName.DeviceAPPassword, Value = connectionParams.DeviceAPPassword };
			parameters.Add(deviceAPPassword);

			Parameter aPSSIDsForSearch = new Parameter();
			string aPSSIDs = string.Empty;
			if (connectionParams.APSSIDsForSearch.List.Any())
			{
				string aPSSIDLast = connectionParams.APSSIDsForSearch.List.Last();
				foreach (string aPSSID in connectionParams.APSSIDsForSearch.List)
				{
					aPSSIDs += aPSSID;
					if (aPSSID != aPSSIDLast)
					{
						aPSSIDs += "&";
					}
				}
			}

			aPSSIDsForSearch.Index = (int)ParamName.APSSIDsForSearch;
			aPSSIDsForSearch.Value = aPSSIDs;

			Parameters = parameters.ToArray();
		}
	}
}

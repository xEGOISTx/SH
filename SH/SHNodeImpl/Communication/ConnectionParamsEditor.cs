using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SH.Communication
{
	internal class ConnectionParamsEditor : IConnectionParamsEditor
	{
		private ConnectionParams _targetConnectionParams;
		private ConnectionParams _originalCopyConnectionParams;

		public ConnectionParamsEditor(ConnectionParams connectionParams)
		{
			_targetConnectionParams = connectionParams;
		}

		public bool IsEditing { get; private set; }

		public void ChangeAPSSIDsForSearch(string[] aPSSIDsForSearch)
		{
			if(IsEditing)
			{
				APSSIDs ssids = _targetConnectionParams.APSSIDsForSearch as APSSIDs;
				ssids.Clear();
				ssids.AddRange(aPSSIDsForSearch);
			}
		}

		public void ChangeDeviceAPPassword(string deviceAPPassword)
		{
			if(IsEditing)
			{
				_targetConnectionParams.DeviceAPPassword = deviceAPPassword;
			}
		}

		public void ChangeDeviceDafaultIP(IPAddress ip)
		{
			if(IsEditing)
			{
				_targetConnectionParams.DeviceDafaultIP = ip;
			}
		}

		public void ChangeRouterAPPassword(string routerAPPassword)
		{
			if(IsEditing)
			{
				(_targetConnectionParams.ConnectionParamsToRouter.ConnectionParams as Core.ConnectionParamsToAP).Password = routerAPPassword;
			}
		}

		public void ChangeRouterIP(IPAddress ip)
		{
			if (IsEditing)
			{
				(_targetConnectionParams.ConnectionParamsToRouter as ConnectionParamsToRouter).RouterIP = ip;
			}
		}

		public void ChangeRouterLogin(string routerLogin)
		{
			if(IsEditing)
			{
				(_targetConnectionParams.ConnectionParamsToRouter.Credentials as Core.Credentials).Login = routerLogin;
			}
		}

		public void ChangeRouterPassword(string routerPassword)
		{
			if (IsEditing)
			{
				(_targetConnectionParams.ConnectionParamsToRouter.Credentials as Core.Credentials).Password = routerPassword;
			}
		}

		public void ChangeRouterSsid(string routerSsid)
		{
			if (IsEditing)
			{
				(_targetConnectionParams.ConnectionParamsToRouter.ConnectionParams as Core.ConnectionParamsToAP).SSID = routerSsid;
			}
		}


		public void StartEditing()
		{
			if(!IsEditing)
			{
				_originalCopyConnectionParams = _targetConnectionParams.GetCopy();
				IsEditing = true;
			}
		}

		public void EndEditing(bool applyCancelChanges)
		{
			if(IsEditing)
			{
				if(applyCancelChanges)
				{
					if(OnApply().Cancel)
					{
						FillFromCopy();
					}
				}
				else
				{
					FillFromCopy();				
				}
			}

			_originalCopyConnectionParams = null;
			IsEditing = false;
		}


		private void FillFromCopy()
		{
			_targetConnectionParams.DeviceDafaultIP = _originalCopyConnectionParams.DeviceDafaultIP;
			_targetConnectionParams.DeviceAPPassword = _originalCopyConnectionParams.DeviceAPPassword;

			APSSIDs targetAPSSIDs = _targetConnectionParams.APSSIDsForSearch as APSSIDs;
			targetAPSSIDs.Clear();
			targetAPSSIDs.AddRange(_originalCopyConnectionParams.APSSIDsForSearch.List);

			(_targetConnectionParams.ConnectionParamsToRouter as ConnectionParamsToRouter).RouterIP =  _originalCopyConnectionParams.ConnectionParamsToRouter.RouterIP;

			Core.ConnectionParamsToAP paramsToRouter = _targetConnectionParams.ConnectionParamsToRouter.ConnectionParams as Core.ConnectionParamsToAP;
			paramsToRouter.SSID = _originalCopyConnectionParams.ConnectionParamsToRouter.ConnectionParams.SSID;
			paramsToRouter.Password = _originalCopyConnectionParams.ConnectionParamsToRouter.ConnectionParams.Password;

			Core.Credentials routerCreadentials = _targetConnectionParams.ConnectionParamsToRouter.Credentials as Core.Credentials;
			routerCreadentials.Login = _originalCopyConnectionParams.ConnectionParamsToRouter.Credentials.Login;
			routerCreadentials.Password = _originalCopyConnectionParams.ConnectionParamsToRouter.Credentials.Password;
		}

		private ApplyConnectionParamsEventArg OnApply()
		{
			ApplyConnectionParamsEventArg arg = new ApplyConnectionParamsEventArg(_targetConnectionParams);
			Apply?.Invoke(this, arg);
			return arg;
		}

		public event ApplyConnectionParamsEventHandler Apply;
	}
}

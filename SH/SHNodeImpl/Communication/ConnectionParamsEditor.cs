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

		internal ConnectionParams OriginalCopyConnectionParams => _originalCopyConnectionParams;

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
				_targetConnectionParams.RouterAPPassword = routerAPPassword;
			}
		}

		public void ChangeRouterUriToParse(string uriToParse)
		{
			if (IsEditing)
			{
				_targetConnectionParams.RouterUriToParse = new Uri(uriToParse);
			}
		}

		public void ChangeRouterLogin(string routerLogin)
		{
			if(IsEditing)
			{
				_targetConnectionParams.RouterLogin = routerLogin;
			}
		}

		public void ChangeRouterPassword(string routerPassword)
		{
			if (IsEditing)
			{
				_targetConnectionParams.RouterPassword = routerPassword;
			}
		}

		public void ChangeRouterSsid(string routerSsid)
		{
			if (IsEditing)
			{
				_targetConnectionParams.RouterSsid = routerSsid;
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
            if (IsEditing)
            {
                if (applyCancelChanges)
                {
                    if (OnApply().Cancel)
                    {
                        CancelChanges();
                    }
                }
                else
                {
                    CancelChanges();
                }

                _originalCopyConnectionParams = null;
                IsEditing = false;
            }
        }


		private void CancelChanges()
		{
			_targetConnectionParams.RouterUriToParse =  _originalCopyConnectionParams.RouterUriToParse;
			_targetConnectionParams.RouterSsid = _originalCopyConnectionParams.RouterSsid;
			_targetConnectionParams.RouterAPPassword = _originalCopyConnectionParams.RouterAPPassword;
			_targetConnectionParams.RouterLogin = _originalCopyConnectionParams.RouterLogin;
			_targetConnectionParams.RouterPassword = _originalCopyConnectionParams.RouterPassword;
			_targetConnectionParams.DeviceDafaultIP = _originalCopyConnectionParams.DeviceDafaultIP;
			_targetConnectionParams.DeviceAPPassword = _originalCopyConnectionParams.DeviceAPPassword;

			APSSIDs targetAPSSIDs = _targetConnectionParams.APSSIDsForSearch as APSSIDs;
			targetAPSSIDs.Clear();
			targetAPSSIDs.AddRange(_originalCopyConnectionParams.APSSIDsForSearch.List);
		}

		private ApplyConnectionParamsEventArgs OnApply()
		{
			ApplyConnectionParamsEventArgs args = new ApplyConnectionParamsEventArgs(_targetConnectionParams);
			Apply?.Invoke(this, args);
			return args;
		}

		public event ApplyConnectionParamsEventHandler Apply;
	}
}

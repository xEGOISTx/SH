using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using SH.Core;

namespace SH.Communication
{
	internal class ConnectionParamsToRouter
	{
		public Uri RouterUriToParse { get; set; } = new Uri("");

		public IConnectionParamsToAP ConnectionParams { get; set; } = new ConnectionParamsToAP();

		public Core.ICredentials Credentials { get; set; } = new Credentials();

		public ConnectionParamsToRouter GetCopy()
		{
			ConnectionParamsToRouter connPCopy = new ConnectionParamsToRouter
			{
				ConnectionParams = new ConnectionParamsToAP
				{
					Password = this.ConnectionParams.Password,
					SSID = this.ConnectionParams.SSID
				},

				Credentials = new Credentials
				{
					Login = this.Credentials.Login,
					Password = this.Credentials.Password
				}
			};

			//if(RouterUriToParse != null)
			//{
			connPCopy.RouterUriToParse = new Uri(this.RouterUriToParse.AbsoluteUri);
			//}

			return connPCopy;
		}
	}
}

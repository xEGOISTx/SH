//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SHToolKit
//{
//	public class Tools : ITools
//	{
//		public DevicesManagement.IDevicesFinder GetDevicesFinder(IRouterParser routerParser = null)
//		{
//			return new DevicesManagement.DevicesFinder(new Communication.Communicator(), new Communication.ConnectorByWiFi(), routerParser);
//		}

//		public Communication.ICommunicator GetCommunicator()
//		{
//			return new Communication.Communicator();
//		}

//		public IRouterParser GetRouterParser()
//		{
//			return new RouterParser();
//		}
//	}
//}

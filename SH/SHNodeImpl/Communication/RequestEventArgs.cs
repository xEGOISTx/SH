using System;
using System.Collections.Generic;
using System.Text;

namespace SH.Communication
{
    internal delegate void RequestEventHandler(object sender, RequestEventArgs e);

    internal class RequestEventArgs
    {
        public RequestEventArgs(DeviceRequest request)
        {
            Request = request;
        }

        public DeviceRequest Request { get; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SH.Communication;
using Windows.Networking.Sockets;

namespace SH.UWPToolKit
{
    public class DevicesRequestsListener : IDevicesRequestsListener
    {
        private const string PORT = "8000";
        private StreamSocketListener _listener;

        public async Task StartListening()
        {
            _listener = new StreamSocketListener();
            await _listener.BindServiceNameAsync(PORT);
            _listener.ConnectionReceived += Listener_ConnectionReceived;
        }

        public void StopListening()
        {
            _listener.ConnectionReceived -= Listener_ConnectionReceived;
            _listener.Dispose();
            _listener = null;
        }

        private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            string request = null;
            using (var streamReader = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
            {
                request = await streamReader.ReadLineAsync();
            }

            if (!string.IsNullOrEmpty(request))
            {
                OnDeviceRequest(request);
            }
        }

        private void OnDeviceRequest(string request)
        {
            DeviceRequest?.Invoke(this, new DeviceRequestEventArgs(request));
        }

        public event DeviceRequestEventHandler DeviceRequest;
    }
}

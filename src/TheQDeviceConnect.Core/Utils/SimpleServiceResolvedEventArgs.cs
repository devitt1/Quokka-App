using System;
namespace TheQDeviceConnect.Core.Utils
{
    public class SimpleServiceResolvedEventArgs : EventArgs
    {
        public string Host;
        public int Port;
        public SimpleServiceResolvedEventArgs(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }

}

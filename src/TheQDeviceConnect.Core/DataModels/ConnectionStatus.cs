using System;
using TheQDeviceConnect.Core.Constants;

namespace TheQDeviceConnect.Core.DataModels
{
    public class ConnectionStatus
    {
        //Code-snippet generated template for public fields
        public WifiNetworkConnectionState DeviceConnectionState { get; set; }

        public ConnectionStatus(WifiNetworkConnectionState status)
        {
            DeviceConnectionState = status;
        }
    }
}

using System;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;

namespace TheQDeviceConnect.Core.Services.Implementations
{
    public class DeviceConnectionService : IDeviceConnectionService
    {
        public DeviceConnectionService()
        {
            DebugHelper.Info(this, "created!");
        }

        public event EventHandler OnWifiNetworkChanged;

        public void ConnectToWifiNetwork(string ssid, string password)
        {
            DebugHelper.Info(this, "called!");
        }

        public string GetConnectedNetworkSSID()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public bool IsConnectedToHotspot()
        {
            throw new NotImplementedException();
        }
    }
}

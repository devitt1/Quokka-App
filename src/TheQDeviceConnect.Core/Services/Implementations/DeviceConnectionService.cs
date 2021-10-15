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

        public void ConnectToWifiNetwork(string ssid, string password)
        {
            DebugHelper.Info(this, "called!");
        }

        public void GetCurrentNetwork()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}

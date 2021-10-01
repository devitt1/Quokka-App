using System;
namespace TheQDeviceConnect.Core.Services.Interfaces
{
    public interface IDeviceConnectionService
    {
        void ConnectToWifiNetwork(string ssid, string password);
    }


}

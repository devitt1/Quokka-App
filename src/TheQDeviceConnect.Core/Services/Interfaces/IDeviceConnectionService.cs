using System;
namespace TheQDeviceConnect.Core.Services.Interfaces
{
    public interface IDeviceConnectionService
    {
        event EventHandler OnWifiNetworkChanged;
        void ConnectToWifiNetwork(string ssid, string password);
        void Initialize();
        string GetConnectedNetworkSSID();
        bool IsConnectedToHotspot();
    }


}

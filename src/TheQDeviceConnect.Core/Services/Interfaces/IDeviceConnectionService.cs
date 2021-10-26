using System;
namespace TheQDeviceConnect.Core.Services.Interfaces
{
    public interface IDeviceConnectionService
    {
        event EventHandler OnWifiNetworkChanged;
        event EventHandler OnConnectionTimerElapsed;
        void ConnectToWifiNetwork(string ssid, string password);
        void Initialize();
        string GetConnectedNetworkSSID();
        void OpenWifiSettings();
        bool IsConnectedToHotspot();

        void StartConnectionTimer();
        void StopConnectionTimer();
    }


}

using System;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.DataModels;

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

        Task<MvxObservableCollection<WifiNetwork>> GetNearbyWifiNetworksAsync();
        Task<bool> UpdateDeviceWifiNetworkCredential(string ssidArg, string passwordArg);
    }


}

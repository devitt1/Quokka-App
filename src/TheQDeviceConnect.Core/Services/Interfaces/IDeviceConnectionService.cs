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
        event EventHandler OnAndroidNsdResolved;
        void ConnectToWifiNetwork(string ssid, string password);
        void Initialize();
        string CurrentConnectedNetworkSSID { get; set; }
        void OpenWifiSettings();
        bool IsConnectedToHotspot();

        void StartConnectionTimer();
        void StopConnectionTimer();

        Task<bool> IsInternetReachable();
        void InitializeAndroidNsd();
        void DiscoverNeabymDNSServices();
        void StopDiscoverNearbymDNSServices();

        public string DeviceResolvedLocalAddress { get; set; }


        Task<MvxObservableCollection<WifiNetworkInfo>> GetNearbyWifiNetworksAsync();
        Task<bool> UpdateDeviceWifiNetworkCredential(string ssidArg,
            string passwordArg, string authMgntArg = "psk", 
            string peapUsernameArg = null, string peapPasswordArg = null);
    }


}

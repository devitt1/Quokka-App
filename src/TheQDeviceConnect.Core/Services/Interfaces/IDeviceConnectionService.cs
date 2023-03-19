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
        Task ConnectToTheQNetwork();

        void Initialize();

        string CurrentConnectedNetworkSSID { get; set; }
        string CurrentConnectedDeviceName { get; set; }
        Task<bool> IsDeviceOnlineAsync();

        Task OpenWifiSettings();

        /**
         * This function force the iOS device to to show 
         * Local Network Usage Description Pop Up
         * so that the user will give the permission to 
         * in order to make request to the Q local network API
         */
        Task ForcePermissionAsync(string ip_address_string, int port);

        bool IsConnectedToHotspot();

        void StartConnectionTimer();
        void StopConnectionTimer();

        Task<bool> IsInternetReachable(string url);
        void InitializeAndroidNsd();
        void DiscoverNeabymDNSServices();
        void StopDiscoverNearbymDNSServices();
       

        public string DeviceResolvedLocalAddress { get; set; }


        Task<MvxObservableCollection<WifiNetworkInfo>> GetNearbyWifiNetworksAsync();
        Task<string> GetDeviceName();
        Task<bool> UpdateDeviceWifiNetworkCredential(string ssidArg,
            string passwordArg, string authMgntArg = "psk", 
            string peapUsernameArg = null, string peapPasswordArg = null);
    }


}

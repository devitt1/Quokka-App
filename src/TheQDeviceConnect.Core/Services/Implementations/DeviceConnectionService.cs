using System;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.DataModels;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Rest.Interfaces;
using TheQDeviceConnect.Core.Services.Interfaces;

namespace TheQDeviceConnect.Core.Services.Implementations
{
    public class DeviceConnectionService : IDeviceConnectionService
    {
        private readonly IRestClient _restClient;
        public MvxObservableCollection<WifiNetwork> NearbyWifiNetwork { get; set; }
        public string DeviceResolvedLocalAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string CurrentConnectedNetworkSSID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DeviceConnectionService(IRestClient restClient)
        {
            DebugHelper.Info(this, "created!");
            _restClient = restClient;
        }

        public event EventHandler OnWifiNetworkChanged;
        public event EventHandler OnConnectionTimerElapsed;
        public event EventHandler OnAndroidNsdResolved;

        public void ConnectToWifiNetwork(string ssid, string password)
        {
            DebugHelper.Info(this, "called!");
        }

        public async Task<MvxObservableCollection<WifiNetwork>> GetNearbyWifiNetworksAsync()
        {
            if (NearbyWifiNetwork == null)
            {
                NearbyWifiNetwork = await _restClient.MakeApiCall<MvxObservableCollection<WifiNetwork>>("WifiNetwork/", HttpMethod.Get);
            }
            return NearbyWifiNetwork;
        }

        public async Task<bool> UpdateDeviceWifiNetworkCredential(string ssidArg, string passwordArg)
        {
            try
            {
                if (!string.IsNullOrEmpty(ssidArg) && !string.IsNullOrEmpty(passwordArg))
                {
                    object data = new { ssid = ssidArg, password = passwordArg };
                    await _restClient.MakeApiCall<WifiNetwork>("WifiNetwork/selected_wifi_network", HttpMethod.Put, data);
                }
                return true;

            } catch (Exception e)
            {
                DebugHelper.Error(this, e);
                return false;
            }
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

        public void OpenWifiSettings()
        {
            throw new NotImplementedException();
        }

        public void StartConnectionTimer()
        {
            throw new NotImplementedException();
        }

        public void StopConnectionTimer()
        {
            throw new NotImplementedException();
        }

        public void InitializeAndroidNsd()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsInternetReachable()
        {
            var result = await _restClient.GetInternetReachability();
            return result;
        }

        public void DiscoverNeabymDNSServices()
        {
            throw new NotImplementedException();
        }
        public bool IsDeviceAddressResolved()
        {
            throw new NotImplementedException();
        }

        public void UpdateDeviceLocalAddress(string address)
        {
            throw new NotImplementedException();
        }

        public void StopDiscoverNearbymDNSServices()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.DataModels;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Rest.Implementations;
using TheQDeviceConnect.Core.Rest.Interfaces;
using TheQDeviceConnect.Core.Services.Interfaces;

namespace TheQDeviceConnect.Core.Services.Implementations
{
    public class DeviceConnectionService : IDeviceConnectionService
    {
        private readonly IRestClient _restClient;
        public MvxObservableCollection<WifiNetworkInfo> NearbyWifiNetwork { get; set; }
        public string DeviceResolvedLocalAddress { get =>
                throw new NotImplementedException(); set =>
                throw new NotImplementedException(); }
        public string CurrentConnectedNetworkSSID { get; set; }
        public string CurrentConnectedDeviceName { get; set; }



        public DeviceConnectionService(IRestClient restClient)
        {
            DebugHelper.Info(this, "created!");
            _restClient = restClient;
            NearbyWifiNetwork = new MvxObservableCollection<WifiNetworkInfo>();
        }

        public event EventHandler OnWifiNetworkChanged;
        public event EventHandler OnConnectionTimerElapsed;
        public event EventHandler OnAndroidNsdResolved;


        public async Task<string> GetDeviceName()
        {
            try
            {
                var deviceName =
                await _restClient.MakeApiCall<string>("WifiNetwork/me", HttpMethod.Get);
                CurrentConnectedNetworkSSID = "The Q Hotspot";
                CurrentConnectedDeviceName = deviceName;
                return deviceName;
            }
            catch (Exception e)
            {
                DebugHelper.Error(this, e);
                return null;
            }
        }

        public async Task<MvxObservableCollection<WifiNetworkInfo>> GetNearbyWifiNetworksAsync()
        {
            try
            {
                if (NearbyWifiNetwork.Count == 0)
                {
                    NearbyWifiNetwork =
                   await _restClient.MakeApiCall<MvxObservableCollection<WifiNetworkInfo>>("WifiNetwork/", HttpMethod.Get);

                }

                return NearbyWifiNetwork;
            } catch (Exception e)
            {
                DebugHelper.Error(this, e);
                throw e;
            }
        }

        public async Task<bool> IsDeviceOnlineAsync()
        {
            var pingResponse = await _restClient.MakeApiCall<PingResponse>("qsim/ping", HttpMethod.Get, useHotspot: false);
            return pingResponse.result;
        }

        public async Task<bool> UpdateDeviceWifiNetworkCredential(string ssidArg,
            string passwordArg = "default", string authMgntArg = "psk", string peapUsernameArg = null,
            string peapPasswordArg = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(ssidArg))
                {
                    if (string.IsNullOrEmpty(passwordArg))
                    {
                        passwordArg = "default";
                    }
                    WifiNetworkCredential data = new WifiNetworkCredential { ssid = ssidArg, password = passwordArg,
                    auth_mgnt = authMgntArg, peap_username = peapUsernameArg,
                    peap_password = peapPasswordArg };
                    await _restClient.MakeApiCall<WifiNetworkCredential>("WifiNetwork/selected_wifi_network/", HttpMethod.Put, data);
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
            return CurrentConnectedNetworkSSID == "The Q Hotspot";
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

        public async Task<bool> IsInternetReachable(string url)
        {
            var result = await _restClient.GetInternetReachability(url);
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

        public void ConnectToWifiNetwork(string ssid, string password)
        {
            throw new NotImplementedException();
        }

        Task IDeviceConnectionService.OpenWifiSettings()
        {
            throw new NotImplementedException();
        }

        public Task ForcePermission()
        {
            throw new NotImplementedException();
        }

        public Task ForcePermissionAsync(string ip_address_string, int port)
        {
            throw new NotImplementedException();
        }

        public async Task ConnectToTheQNetwork()
        {
            throw new NotImplementedException();
        }

    }
}

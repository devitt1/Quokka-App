using System;
using System.Reflection;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using NetworkExtension;
using TheQDeviceConnect.Core.DataModels;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.iOS.Services.Implementations;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceConnectionService))]
namespace TheQDeviceConnect.iOS.Services.Implementations
{
    public class DeviceConnectionService : IDeviceConnectionService
    {

        NEHotspotHelper _wifiHelper;
        NEHotspotConfiguration _wifiConfig;
        NEHotspotConfigurationManager _wifiConfigManager;

        public string DeviceResolvedLocalAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler OnWifiNetworkChanged;
        public event EventHandler OnConnectionTimerElapsed;
        public event EventHandler OnAndroidNsdResolved;

        public DeviceConnectionService()
        {
            
        }

        public void Initialize()
        {
            _wifiHelper = new NEHotspotHelper();
            _wifiConfigManager = new NEHotspotConfigurationManager();

        }

        public void ConnectToWifiNetwork(string ssid, string password)
        {
            _wifiConfig = new NEHotspotConfiguration(ssid, password, false)
            {
                JoinOnce = true
            };

            var t = Task<bool>.Run(() =>
            {
                return ConnectWpa(ssid, password);
            }
            );
        }

        public async Task<bool> ConnectWpa(string ssid, string password)
        {
            try
            {

                _wifiConfigManager.RemoveConfiguration("Kogan_9EE1_5G");

                _wifiConfigManager.ApplyConfiguration(_wifiConfig, (error) =>
                {
                    if (error != null)
                    {
                        DebugHelper.Error(this, $"Error while connecting to WiFi network {ssid}: {error}");
                    }
                });


                return true;
            }
            catch (Exception e)
            {
                DebugHelper.Error(this, e, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public void GetCurrentNetwork()
        {
            throw new NotImplementedException();
        }

        public string GetConnectedNetworkSSID()
        {
            throw new NotImplementedException();
        }

        public void OpenWifiSettings()
        {
            throw new NotImplementedException();
        }

        public bool IsConnectedToHotspot()
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

        public Task<MvxObservableCollection<WifiNetwork>> GetNearbyWifiNetworksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDeviceWifiNetworkCredential(string ssidArg, string passwordArg)
        {
            throw new NotImplementedException();
        }

        public void InitializeAndroidNsd()
        {
            throw new NotImplementedException();
        }

        public bool IsInternetReachable()
        {
            throw new NotImplementedException();
        }

        public void DiscoverNeabymDNSServices()
        {
            throw new NotImplementedException();
        }

        public void UpdateDeviceLocalAddress(string address)
        {
            throw new NotImplementedException();
        }

        public bool IsDeviceAddressResolved()
        {
            throw new NotImplementedException();
        }

        public void StopDiscoverNearbymDNSServices()
        {
            throw new NotImplementedException();
        }
    }
}

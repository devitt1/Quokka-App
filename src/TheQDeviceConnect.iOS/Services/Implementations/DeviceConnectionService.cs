using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
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
        public Timer ConnectionTimer { get; set; }

        public string DeviceResolvedLocalAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //Code-snippet generated template for public fields
        private string _currentConnectedNetworkSSID;
        public string CurrentConnectedNetworkSSID
        {
            get
            {
                return _currentConnectedNetworkSSID;
            }
            set
            {
                (_currentConnectedNetworkSSID) = value;
                OnWifiNetworkChanged.Invoke(this, EventArgs.Empty);
            }

        }
        private readonly string THEQ_SSID = "The Q Hotspot";
        private readonly string THEQ_PASSWORD = "theqpassword";



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

            initTimer(10000);
        }

        private void initTimer(int timeoutLimit)
        {
            ConnectionTimer = new Timer(timeoutLimit);
            ConnectionTimer.Elapsed -= handleConnectionTimerElapsed;
            ConnectionTimer.Elapsed += handleConnectionTimerElapsed;
        }

        private void handleConnectionTimerElapsed(object sender, ElapsedEventArgs eventArgs)
        {
            OnConnectionTimerElapsed.Invoke(sender, eventArgs);
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

        public bool ConnectWpa(string ssid, string password)
        {
            try
            {
                _wifiConfigManager.ApplyConfiguration(_wifiConfig, (error) =>
                {
                    if (error != null)
                    {
                        DebugHelper.Error(this, $"Error while connecting to WiFi network {ssid}: {error}");
                        if (error.ToString() == "already associated.")
                        {
                            DebugHelper.Info(this, $"Current ssid = {ssid}");
                            CurrentConnectedNetworkSSID = ssid;
                        }
                    }
                    else
                    {
                        DebugHelper.Info(this, $"No issue occured. Successfully connected to Wifi netowork {ssid}");
                        DebugHelper.Info(this, $"Current ssid = {ssid}");
                        CurrentConnectedNetworkSSID = ssid;
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

        public void OpenWifiSettings()
        {
            DebugHelper.Info(this, "iOS: OpenWifiSettings() called");

            ConnectToWifiNetwork(THEQ_SSID, THEQ_PASSWORD);
            //UIKit.UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl("app-settings:WIFI"));
        }

        public bool IsConnectedToHotspot()
        {
            DebugHelper.Info(this, "iOS: IsConnectedToHotspot() called");
            if (CurrentConnectedNetworkSSID == "The Q Hotspot")
            {
                DebugHelper.Info(this, "Connected to the Q hotspot");
                return true;
            }
            else
            {
                DebugHelper.Info(this, "Not connected to the Q hotspot");
                return false;
            }
        }

        public void StartConnectionTimer()
        {
            if (ConnectionTimer != null)
            {
                DebugHelper.Info(this, "StartConnectionTimer");
                ConnectionTimer.Start();
            }
        }

        public void StopConnectionTimer()
        {
            if (ConnectionTimer != null)
            {
                DebugHelper.Info(this, "StopConnectionTimer");
                ConnectionTimer.Stop();
            }
        }

        public Task<MvxObservableCollection<WifiNetworkInfo>> GetNearbyWifiNetworksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDeviceWifiNetworkCredential(string ssidArg, string passwordArg)
        {
            throw new NotImplementedException();
        }

        public void InitializeAndroidNsd()
        {
            DebugHelper.Info(this, "InitializeAndroidNsd not implemented on iOS. " +
               "Device Platform automatically resolve hostname");
        }

        public Task<bool> IsInternetReachable()
        {
            throw new NotImplementedException();
        }

        public void DiscoverNeabymDNSServices()
        {
            DebugHelper.Info(this, "DiscoverNearbymDNSServices not implemented on iOS. " +
                "Device Platform automatically resolve hostname");
        }

        public void UpdateDeviceLocalAddress(string address)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsDeviceAddressResolved()
        {
            throw new NotImplementedException();
        }

        public void StopDiscoverNearbymDNSServices()
        {
            DebugHelper.Info(this, "StopDiscoverNearbymDNSServices not implemented on iOS. " +
             "Device Platform automatically resolve hostname");
        }

        public Task<bool> UpdateDeviceWifiNetworkCredential(string ssidArg, string passwordArg, string authMgntArg = "psk", string peapUsernameArg = null, string peapPasswordArg = null)
        {
            throw new NotImplementedException();
        }

        Task<MvxObservableCollection<WifiNetworkInfo>> IDeviceConnectionService.GetNearbyWifiNetworksAsync()
        {
            throw new NotImplementedException();
        }
    }
}

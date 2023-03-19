using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using MvvmCross.ViewModels;
using NetworkExtension;
using TheQDeviceConnect.Core.DataModels;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.iOS.Services.Implementations;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

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

        string CurrentConnectedDeviceName { get; set; }
        string IDeviceConnectionService.CurrentConnectedDeviceName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

            initTimer(5000);
        }

        public async Task ForcePermissionAsync(string ip_address_string, int port)
        {
            try
            {
                await CheckAndRequestPermissionAsync();
                IPAddress ipAddress = IPAddress.Parse(ip_address_string);
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, port);

            var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            await client.ConnectAsync(remoteEndPoint).ConfigureAwait(true);

            } catch (Exception e)
            {
                DebugHelper.Error(this, e);
                throw e;
            }
          
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

        public async Task<bool> ConnectWpa(string ssid, string password)
        {
            try
            {
                _wifiConfig = new NEHotspotConfiguration(ssid, password, false)
                {
                    JoinOnce = true
                };

                await _wifiConfigManager.ApplyConfigurationAsync(_wifiConfig);
                DebugHelper.Info(this, $"No issue occured. Successfully connected to Wifi netowork {ssid}");
                DebugHelper.Info(this, $"Current ssid = {ssid}");
                CurrentConnectedNetworkSSID = ssid;
                return true;
            }
            catch (Exception e)
            {
                DebugHelper.Error(this, $"Error while connecting to WiFi network {ssid}: {e}");
                if (e.ToString() == "already associated.")
                {
                    DebugHelper.Info(this, $"Current ssid = {ssid}");
                    CurrentConnectedNetworkSSID = ssid;
                }
                return false;
            }
        }

        public async Task OpenWifiSettings()
        {
            DebugHelper.Info(this, "iOS: OpenWifiSettings() called");
            //UIKit.UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl("app-settings:WIFI"));
        }


        public async Task<PermissionStatus> CheckAndRequestPermissionAsync()
        {
            BasePermission LocalNetworkAccessPermission = new Permissions.LocalNetworkAccess();
            var status = await LocalNetworkAccessPermission.RequestAsync();


            //DebugHelper.Info(this, status);

            return PermissionStatus.Granted;
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

        public Task<bool> IsInternetReachable(string url)
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

        public void ConnectToWifiNetwork(string ssid, string password)
        {
            throw new NotImplementedException();
        }

        public async Task ConnectToTheQNetwork()
        {
            await ConnectWpa(THEQ_SSID, THEQ_PASSWORD);
        }

        public Task<string> GetDeviceName()
        {
            throw new NotImplementedException();
        }

        public bool IsConnectedToHotspot()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsDeviceOnlineAsync()
        {
            throw new NotImplementedException();
        }
    }
}

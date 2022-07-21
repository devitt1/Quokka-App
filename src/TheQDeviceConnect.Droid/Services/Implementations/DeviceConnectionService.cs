using System;
using System.Reflection;
using System.Threading.Tasks;
using Android.Net.Wifi;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.Droid.Services.Implementations;
using AndroidApplication = Android.App.Application;
using AndroidContext = Android.Content.Context;
using Xamarin.Forms;
using Android.Net;
using System.Linq;
using System.Timers;
using Android.Content;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.DataModels;
using TheQDeviceConnect.Droid.Utils;
using MvvmCross;
using Xamarin.Essentials;

[assembly: Dependency(typeof(DeviceConnectionService))]
namespace TheQDeviceConnect.Droid.Services.Implementations
{
    public class DeviceConnectionService : IDeviceConnectionService
    {
        private WifiManager _wifiManager;
        private NetworkCallback _networkCallback;
        private ConnectivityManager _connectivityManager;
        private NsdHelper _nsdHelper;
        private IDeviceConnectionService _coreDeviceConnectionService;
        public event EventHandler OnWifiNetworkChanged;
        public event EventHandler OnConnectionTimerElapsed;
        public event EventHandler OnAndroidNsdResolved;
        public Timer ConnectionTimer { get; set; }
        private string _deviceResolvedLocalAddress;
        public string DeviceResolvedLocalAddress
        {
            get
            {
                return _deviceResolvedLocalAddress;
            }
            set
            {
                _deviceResolvedLocalAddress = value;
            }
        }


        public DeviceConnectionService()
        {
            DebugHelper.Info(this, "called!");
            _coreDeviceConnectionService = Mvx.IoCProvider.Resolve<IDeviceConnectionService>();
            if (SecureStorage.GetAsync("DeviceResolvedLocalAddress").Result != null)
            {
                _deviceResolvedLocalAddress = SecureStorage.GetAsync("DeviceResolvedLocalAddress").Result;
            }
        }

        public async Task<MvxObservableCollection<WifiNetworkInfo>> GetNearbyWifiNetworksAsync()
        {
            //This will never be implemented
            return null;
        }

        private void initTimer(int timeoutLimit)
        {
            ConnectionTimer = new Timer(timeoutLimit);
            ConnectionTimer.Elapsed -= handleConnectionTimerElapsed;
            ConnectionTimer.Elapsed += handleConnectionTimerElapsed;
        }

        public void StartConnectionTimer()
        {
            if (ConnectionTimer != null)
            {
                ConnectionTimer.Start();
            }
        }


        public void StopConnectionTimer()
        {
            if (ConnectionTimer != null)
            {
                ConnectionTimer.Stop();
            }
        }

        private void handleConnectionTimerElapsed(object sender, ElapsedEventArgs eventArgs)
        {
            OnConnectionTimerElapsed.Invoke(sender, eventArgs);
        }

        [Obsolete]
        public void OpenWifiSettings()
        {
            Intent intent = new Intent(Android.Provider.Settings.ActionWifiSettings);
            intent.SetFlags(ActivityFlags.NewTask);
            Forms.Context.StartActivity(intent);

        }

        public void Initialize()
        {
            DebugHelper.Info(this, "called");

            initTimer(10000);

            _wifiManager = AndroidApplication
                  .Context
                  .GetSystemService
                  (AndroidContext.WifiService)
                  as WifiManager;

            _connectivityManager = AndroidApplication
                .Context
                .GetSystemService(AndroidContext.ConnectivityService)
                as ConnectivityManager;

            _networkCallback = new NetworkCallback(_connectivityManager)
            {
                NetworkAvailable = network =>
                {
                    DebugHelper.Info(this, "Connected to  " + CurrentConnectedNetworkSSID, MethodBase.GetCurrentMethod().Name);
                    OnWifiNetworkChanged.Invoke(this, EventArgs.Empty);

                },
                NetworkUnavailable = () =>
                {
                    DebugHelper.Info(this, "Unconnected!" + CurrentConnectedNetworkSSID, MethodBase.GetCurrentMethod().Name);
                    OnWifiNetworkChanged.Invoke(this, EventArgs.Empty);
                }
            };

            _connectivityManager.RegisterDefaultNetworkCallback(_networkCallback);

        }

        private void handleNsdResolved(object sender, EventArgs e)
        {
            DebugHelper.Info(this, e);
            OnAndroidNsdResolved.Invoke(this, e);
        }

        public async void ConnectToWifiNetwork(string ssid, string password)
        {
            try
            {
                bool result = await ConnectWpa(ssid, password);
            }
            catch (Exception e)
            {
                DebugHelper.Error(this, e);
                throw e;
            }
        }

        public void InitializeAndroidNsd()
        {
            _nsdHelper = new NsdHelper(AndroidApplication.Context);
            _nsdHelper.Initialize();
            _nsdHelper.NsdResolved += handleNsdResolved;
        }
        public void DiscoverNeabymDNSServices()
        {
            _nsdHelper.DiscoverServices();
        }

        public void StopDiscoverNearbymDNSServices()
        {
            _nsdHelper.StopDiscovery();
        }

        public async Task<bool> ConnectWpa(string ssid, string password)
        {
            try
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
                { // if Android Version >= 10
                    WifiNetworkSpecifier wifiSpecifier = new WifiNetworkSpecifier
                   .Builder()
                   .SetSsid(ssid)
                   .SetWpa2Passphrase(password)
                   .Build();

                    NetworkRequest request = new NetworkRequest
                        .Builder()
                        .AddTransportType(TransportType.Wifi)
                        .RemoveCapability(NetCapability.WifiP2p)
                        .SetNetworkSpecifier(wifiSpecifier)
                        .Build();

                    _connectivityManager.RequestNetwork(request, _networkCallback);
                }
                else // if Android Version < 10
                {
                    #pragma warning disable CS0612 // Type or member is obsolete
                    handleConnectionOlderVersion(_wifiManager, ssid, password);
                    #pragma warning restore CS0612 // Type or member is obsolete
                }
                return true;
            }
            catch (Exception e)
            {
                DebugHelper.Error(this, e, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        [Obsolete]
        private void handleConnectionOlderVersion(WifiManager wifiManager, string ssid, string password)
        {
            try
            {
                var formattedSsid = $"\"{ssid}\"";
                var formattedPassword = $"\"{password}\"";

                var wifiConfig = new WifiConfiguration
                {
                    Ssid = formattedSsid,
                    PreSharedKey = formattedPassword
                };

                var addNetwork = wifiManager.AddNetwork(wifiConfig);

                var availableNetworks = wifiManager.ConfiguredNetworks;


                var network = wifiManager.ConfiguredNetworks
                     .FirstOrDefault(n => n.Ssid == formattedSsid);

                if (network == null)
                {
                    Console.WriteLine($"Cannot connect to network: {ssid}");
                    return;
                }

                wifiManager.Disconnect();
                var enableNetwork = wifiManager.EnableNetwork(network.NetworkId, true);

            }
            catch (Exception exception)
            {
                DebugHelper.Error(this, exception, MethodBase.GetCurrentMethod().Name);
            }

        }

        public void DisconnectWifi()
        {
            if (_networkCallback is null | _wifiManager is null)
                return;
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
            {
                _connectivityManager.UnregisterNetworkCallback(_networkCallback);
            }
        }

        public bool IsConnectedToHotspot()
        {
            if (CurrentConnectedNetworkSSID == "\"The Q Hotspot\"")
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

        //Code-snippet generated template for public fields
        private string _currentConnectedNetworkSSID = "";
        public string CurrentConnectedNetworkSSID
        {
            get
            {
                if (_wifiManager != null) {
                    _currentConnectedNetworkSSID = _wifiManager.ConnectionInfo.SSID;
                }
                return _currentConnectedNetworkSSID;
            }
            set
            {
                _currentConnectedNetworkSSID = value;
                (_currentConnectedNetworkSSID) = value;
            }
        }

        public Task<bool> UpdateDeviceWifiNetworkCredential(string ssidArg, string passwordArg)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInternetReachable()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDeviceWifiNetworkCredential(string ssidArg, string passwordArg, string authMgntArg = "psk", string peapUsernameArg = null, string peapPasswordArg = null)
        {
            throw new NotImplementedException();
        }

        private class NetworkCallback : ConnectivityManager.NetworkCallback
        {
            private ConnectivityManager _conn;
            public Action<Network> NetworkAvailable { get; set; }
            public Action NetworkUnavailable { get; set; }

            public NetworkCallback(ConnectivityManager connectivityManager)
            {
                _conn = connectivityManager;
            }

            public override void OnAvailable(Network network)
            {
                base.OnAvailable(network);
                // Need this to bind to network otherwise it is connected to wifi 
                // but traffic is not routed to the wifi specified

                _conn.BindProcessToNetwork(network);
                NetworkAvailable?.Invoke(network);
            }

            public override void OnUnavailable()
            {
                base.OnUnavailable();

                NetworkUnavailable?.Invoke();
            }

            public override void OnLost(Network network)
            {
                base.OnLost(network);
            }
        }
    }
}

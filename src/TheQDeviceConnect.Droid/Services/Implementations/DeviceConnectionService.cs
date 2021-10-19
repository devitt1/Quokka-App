using System;
using System.Reflection;
using System.Threading.Tasks;
using Android.Net.Wifi;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.Droid.Services.Implementations;
using AndroidAplication = Android.App.Application;
using AndroidContext = Android.Content.Context;
using Xamarin.Forms;
using Android.Net;
using System.Linq;

[assembly: Dependency(typeof(DeviceConnectionService))]
namespace TheQDeviceConnect.Droid.Services.Implementations
{
    public class DeviceConnectionService : IDeviceConnectionService
    {
        private WifiManager _wifiManager;
        private NetworkCallback _networkCallback;
        private ConnectivityManager _connectivityManager;
        public event EventHandler OnWifiNetworkChanged;

        public DeviceConnectionService()
        {
            DebugHelper.Info(this, "called!");
        }

        public void Initialize()
        {
            DebugHelper.Info(this, "called");
            _wifiManager = AndroidAplication
                  .Context
                  .GetSystemService
                  (AndroidContext.WifiService)
                  as WifiManager;

            _connectivityManager = AndroidAplication
                .Context
                .GetSystemService(AndroidContext.ConnectivityService)
                as ConnectivityManager;



            _networkCallback = new NetworkCallback(_connectivityManager)
            {
                NetworkAvailable = network =>
                {
                    DebugHelper.Info(this, "Connected to  " + GetConnectedNetworkSSID(), MethodBase.GetCurrentMethod().Name);
                    OnWifiNetworkChanged.Invoke(this, EventArgs.Empty);
                },
                NetworkUnavailable = () =>
                {
                    DebugHelper.Info(this, "Unconnected!" + GetConnectedNetworkSSID(), MethodBase.GetCurrentMethod().Name);
                    OnWifiNetworkChanged.Invoke(this, EventArgs.Empty);
                }
            };

            _connectivityManager.RegisterDefaultNetworkCallback(_networkCallback);

        }

        private void handleNetworkChanged(object sender, EventArgs e)
        {

        }

        public void ConnectToWifiNetwork(string ssid, string password)
        {
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
            if (GetConnectedNetworkSSID() == "\"The Q Hotspot\"")
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
        public string GetConnectedNetworkSSID()
        {
            var ssid = _wifiManager.ConnectionInfo.SSID;
            DebugHelper.Info(this, "Current connected network SSID is " + ssid);
            return ssid;
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

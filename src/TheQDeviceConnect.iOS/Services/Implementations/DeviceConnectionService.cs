using System;
using System.Reflection;
using System.Threading.Tasks;
using NetworkExtension;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.iOS.Services.Implementations;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceConnectionService))]
namespace TheQDeviceConnect.iOS.Services.Implementations
{
    public class DeviceConnectionService : IDeviceConnectionService
    {
        public DeviceConnectionService()
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
                var wifiHelper = new NEHotspotHelper();

                var configManager = new NEHotspotConfigurationManager();
                var wifiConfig = new NEHotspotConfiguration(ssid, password, false)
                {
                    JoinOnce = true
                };



                configManager.RemoveConfiguration("Kogan_9EE1_5G");

                configManager.ApplyConfiguration(wifiConfig, (error) =>
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
    }
}

using System;
namespace TheQDeviceConnect.Core.ViewModels.Connection
{
    public interface IWifiNetworkViewModel
    {
        string ssid { get; set; }
    }
    public class WifiNetworkViewModel : BaseViewModel, IWifiNetworkViewModel
    {
        public WifiNetworkViewModel()
        {
        }

        public string ssid { get; set ; }
    }
}

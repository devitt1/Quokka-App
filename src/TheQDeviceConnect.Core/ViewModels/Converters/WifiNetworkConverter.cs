using System;
using MvvmCross.Converters;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.DataModels;
using TheQDeviceConnect.Core.ViewModels.Connection;

namespace TheQDeviceConnect.Core.ViewModels.Converters
{
    public class WifiNetworkConverter : MvxValueConverter<WifiNetwork, WifiNetworkViewModel>
    {
        public WifiNetworkConverter()
        {
        }

        public static MvxObservableCollection<WifiNetworkViewModel> Convert(MvxObservableCollection<WifiNetwork> values)
        {
            var wifiNetworkItems = values;

            var viewModels = new MvxObservableCollection<WifiNetworkViewModel>();

            foreach (WifiNetwork network in wifiNetworkItems)
            {
                var vmToAdd = new WifiNetworkViewModel
                {
                    ssid = network.ssid,
                };
                viewModels.Add(vmToAdd);
            }

            return viewModels;
        }
    }
}

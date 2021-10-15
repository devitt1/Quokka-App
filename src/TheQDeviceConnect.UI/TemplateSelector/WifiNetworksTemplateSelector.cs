using System;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.ViewModels;
using TheQDeviceConnect.Core.ViewModels.Connection;
using Xamarin.Forms;

namespace TheQDeviceConnect.UI.TemplateSelector
{
    public class WifiNetworksTemplateSelector : DataTemplateSelector
    {
        public WifiNetworksTemplateSelector()
        {
            DebugHelper.Info(this, "Initialized!");
        }
        public DataTemplate DefaultWifiNetworksTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is IWifiNetworkViewModel)
            {
                return DefaultWifiNetworksTemplate;
            }

            else
            {
                return DefaultTemplate;
            }
        }
    }
}
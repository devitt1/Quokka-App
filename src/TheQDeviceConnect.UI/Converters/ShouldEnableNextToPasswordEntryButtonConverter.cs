using System;
using System.Globalization;
using TheQDeviceConnect.Core.ViewModels.Connection;
using Xamarin.Forms;

namespace TheQDeviceConnect.UI.Converters
{
    public class ShouldEnableNextToPasswordEntryButtonConverter : IValueConverter
    {
        public ShouldEnableNextToPasswordEntryButtonConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WifiNetworkViewModel selectedWifiNetworkVM = (WifiNetworkViewModel)value;
            if (selectedWifiNetworkVM != null)
            {
                return true;
            } else
            {
                return false;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

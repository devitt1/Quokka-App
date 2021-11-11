using System;
using System.Globalization;
using Xamarin.Forms;

namespace TheQDeviceConnect.UI.Converters
{
    public class ItemSelectedBackgroundColourConverter : IValueConverter
    {
        public ItemSelectedBackgroundColourConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = (bool) value;
            var bkgColour = isSelected ? "#DEE4F1" : "Transparent";
            return bkgColour;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

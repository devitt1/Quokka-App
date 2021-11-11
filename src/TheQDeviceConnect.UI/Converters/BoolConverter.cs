using System;
using System.Globalization;
using Xamarin.Forms;

namespace TheQDeviceConnect.UI.Converters
{
    public class BoolConverter : IValueConverter
    {
        public BoolConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool inverted_value = ! (bool)value;
            return inverted_value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

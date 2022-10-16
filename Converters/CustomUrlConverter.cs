using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChatOverlay
{
    public class CustomUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "Custom")
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Visible)
            {
                return "KapChat - Twitch";
            }
            else
            {
                return "Custom";
            }
        }
    }
}

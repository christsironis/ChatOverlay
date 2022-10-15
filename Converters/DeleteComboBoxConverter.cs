using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChatOverlay
{
    public class DeleteComboBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "Add New" || value.ToString() == "None")
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
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

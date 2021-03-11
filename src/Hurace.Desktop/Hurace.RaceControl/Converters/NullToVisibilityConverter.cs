using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hurace.RaceControl.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.TryParse((string)parameter, out bool reverse))
            {
                return value == null 
                    ? Visibility.Hidden 
                    : Visibility.Visible;
            }

            return value == null 
                ? Visibility.Visible 
                : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

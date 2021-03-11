using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hurace.Mvvm.Converters
{
    internal class FuncToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Func<bool>)value)() == true
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Hurace.Core.Exceptions;
using Hurace.Mvvm.Enums;

namespace Hurace.Mvvm.Converters
{
    internal class ButtonStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ButtonStyle)value)
            {
                case ButtonStyle.Raised:
                    return parameter.Equals("Raised") ? Visibility.Visible : Visibility.Hidden;
                case ButtonStyle.Flat:
                    return parameter.Equals("Flat") ? Visibility.Visible : Visibility.Hidden;
                default: 
                    throw new CaseNotImplementedException<ButtonStyle>((ButtonStyle)value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

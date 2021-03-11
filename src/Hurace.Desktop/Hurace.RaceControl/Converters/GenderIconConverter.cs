using System;
using System.Globalization;
using System.Windows.Data;
using Hurace.Core.Enums;
using Hurace.Core.Exceptions;

namespace Hurace.RaceControl.Converters
{
    public class GenderIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Gender)value)
            {
                case Gender.Male: return "GenderMale";
                case Gender.Female: return "GenderFemale";
                default: throw new CaseNotImplementedException<Gender>((Gender)value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;
using Hurace.Core.Enums;
using Hurace.Core.Exceptions;

namespace Hurace.RaceControl.Converters
{
    public class RaceStateIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((RaceState)value)
            {
                case RaceState.NotStarted: return "SquareSmall";
                case RaceState.Running: return "AccessPoint";
                case RaceState.Canceled: return "FlagRemove";
                case RaceState.Done: return "FlagCheckered";
                default: throw new CaseNotImplementedException<RaceState>((RaceState)value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

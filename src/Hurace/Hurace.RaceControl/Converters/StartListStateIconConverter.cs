using System;
using System.Globalization;
using System.Windows.Data;
using Hurace.Core.Enums;
using Hurace.Core.Exceptions;

namespace Hurace.RaceControl.Converters
{
    public class StartListStateIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((StartListState)value)
            {
                case StartListState.NotStarted: return "SquareSmall";
                case StartListState.Running: return "AccessPoint";
                case StartListState.Disqualified: return "FlagRemove";
                case StartListState.Done: return "FlagCheckered";
                default: throw new CaseNotImplementedException<StartListState>((StartListState)value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

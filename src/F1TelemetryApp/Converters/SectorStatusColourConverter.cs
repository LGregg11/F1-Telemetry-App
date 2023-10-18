namespace F1TelemetryApp.Converters;

using Enums;

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

public class SectorStatusColourConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TimeStatus)
            return Brushes.White;

        return (TimeStatus)value switch
        {
            TimeStatus.NotPersonalBest => Brushes.White,
            TimeStatus.PersonalBest => Brushes.Green,
            TimeStatus.BestOfSession => Brushes.Purple,
            _ => Brushes.White
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

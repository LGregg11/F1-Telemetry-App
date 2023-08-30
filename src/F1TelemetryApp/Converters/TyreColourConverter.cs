namespace F1TelemetryApp.Converters;

using F1GameTelemetry.Enums;

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

public class TyreColourConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TyreVisual)
            return Brushes.White;

        return (TyreVisual)value switch
        {
            TyreVisual.Soft => Brushes.Red,
            TyreVisual.Medium => Brushes.Yellow,
            TyreVisual.Hard => Brushes.White,
            TyreVisual.Intermediate => Brushes.Green,
            TyreVisual.Wet => Brushes.Blue,
            _ => Brushes.White
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

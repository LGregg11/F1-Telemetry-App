namespace F1TelemetryApp.Converters;

using F1GameTelemetry.Enums;

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

public class EnumConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value.GetType() == typeof(GameVersion))
            return GetEnumDescription((GameVersion)value)!;

        if (value.GetType() == typeof(TyreVisual))
            return GetEnumDescription((TyreVisual)value)!;

        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }


    public static string? GetEnumDescription<T>(T value)
        where T : Enum
    {
        if (!typeof(T).IsEnum)
            return null;

        var description = value.ToString();
        var fieldInfo = value.GetType().GetField(description);

        if (fieldInfo != null)
        {
            var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                description = ((DescriptionAttribute)attrs.FirstOrDefault()!).Description;
            }    
        }

        return description;
    }
}

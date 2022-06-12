namespace F1TelemetryApp.Converters;

using System;
using System.Globalization;
using System.Windows.Data;

internal class TelemetryConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value.GetType() == typeof(int))
            return ToTelemetryTime((int)value);
        else if (value.GetType() == typeof(float))
            return ToTelemetryTime((float)value);
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Convert float to a standard telemetry time format.
    /// </summary>
    /// <param name="time">Time given in ms.</param>
    /// <returns>The time in a standard telemetry format.</returns>
    public static string ToTelemetryTime(float milliseconds)
    {
        if (milliseconds <= 0)
            return "--:--:--";
        TimeSpan t = TimeSpan.FromMilliseconds(milliseconds);
        string timeStr = string.Format("{0:D2}:{1:D2}:{2:D3}", t.Minutes, t.Seconds, t.Milliseconds);
        if (t.Hours > 0)
            timeStr = string.Format("{0:D2}:", t.Hours) + timeStr;
        return timeStr;
    }

    /// <summary>
    /// Convert ushort to a standard telemetry time format.
    /// </summary>
    /// <param name="time">Time given in seconds.</param>
    /// <returns>The time in a standard telemetry format.</returns>
    public static string ToTelemetryTime(int milliseconds)
    {
        double time = System.Convert.ToDouble(milliseconds);
        if (time <= 0)
            return "--:--";
        TimeSpan t = TimeSpan.FromMilliseconds(time);
        string timeStr = string.Format("{0:D2}:{1:D3}", t.Seconds, t.Milliseconds);
        if (t.Minutes > 0)
            timeStr = string.Format("{0:D2}:", t.Minutes) + timeStr;
        if (t.Hours > 0)
            timeStr = string.Format("{0:D2}:", t.Hours) + timeStr;
        return timeStr;
    }
}

namespace F1TelemetryApp.Model;

using F1TelemetryApp.Enums;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

public class TelemetryDriverLapData : INotifyPropertyChanged
{
    private readonly SolidColorBrush colour;

    public TelemetryDriverLapData(int lap, string title, int thickness, SolidColorBrush colour)
    {
        this.colour = colour;
        Lap = lap;

        DataTypeSeriesMap = new Dictionary<GraphDataType, LineSeriesHandler>();
        foreach(GraphDataType type in EnumCollections.GraphDataTypes)
            DataTypeSeriesMap.Add(type, new LineSeriesHandler(title, thickness, colour));
    }

    public int Lap { get; }

    public Dictionary<GraphDataType, LineSeriesHandler> DataTypeSeriesMap { get; private set; }

    public LineSeries GetLineSeries(GraphDataType type) => DataTypeSeriesMap[type].LineSeries;

    public void UpdateVisibility(bool visible)
    {
        foreach (GraphDataType type in DataTypeSeriesMap.Keys)
        {
            var series = DataTypeSeriesMap[type];
            series.LineSeries.Stroke = visible ? colour : Brushes.Transparent;
            DataTypeSeriesMap[type] = series;
        }
        NotifyPropertyChanged();
    }

    public void UpdateGraphPoint(GraphDataType type, double? x, double? y)
    {
        var series = DataTypeSeriesMap[type];
        var point = series.Point;
        if (x != null)
            point.X = x;
        if (y != null)
            point.Y = y;

        series.Point = new GraphPoint(point.X, point.Y);
        DataTypeSeriesMap[type] = series;
        NotifyPropertyChanged();
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}

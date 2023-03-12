namespace F1TelemetryApp.Model;

using Enums;

using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.ComponentModel;
using System.Windows.Media;

public class GraphPointCollection : INotifyPropertyChanged
{
    public GraphPointCollection(DataGraphType type)
    {
        GraphType = type;
        Series = new SeriesCollection
        {
            new LineSeries
            {
                Title = Enum.GetName(type),
                Configuration = new CartesianMapper<GraphPoint>().X(p => (double)p.X!).Y(p => (double)p.Y!),
                Values = new ChartValues<GraphPoint>(),
                LineSmoothness = 0,
                PointForeground = null,
                PointGeometry = null,
                StrokeThickness = 2,
                Fill = new SolidColorBrush() { Opacity = 0.0, Color = default }
            }
        };
    }

    public DataGraphType GraphType { get; set; }

    public SeriesCollection Series { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private double lastLapDistance;

    private GraphPoint point = new();
    public GraphPoint Point
    {
        get => point;
        set
        {
            point = value;
            if (IsPointReady())
                UpdateSeries();
        }
    }

    private bool IsPointReady() => point.X != null && point.Y != null && (double)point.X! > lastLapDistance;

    private void UpdateSeries()
    {
        Series[0].Values.Add(point);
        lastLapDistance = (double)point.X!;
        point = new();
        NotifyPropertyChanged();
    }

    private void NotifyPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public struct GraphPoint
{
    public double? X { get; set; }
    public double? Y { get; set; }
}

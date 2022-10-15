namespace F1TelemetryApp.Model;

using Enums;

using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

public class LineSeriesHandler : INotifyPropertyChanged
{
    public LineSeriesHandler(string title, int thickness, SolidColorBrush stroke)
    {
        Point = new();
        LineSeries = new LineSeries
        {
            Title = title,
            StrokeThickness = thickness,
            Stroke = stroke,
            Configuration = new CartesianMapper<GraphPoint>()
            .X(p => (double)p.X!)
            .Y(p => (double)p.Y!),
            Values = new ChartValues<GraphPoint>(),
            LineSmoothness = 0,
            PointForeground = null,
            PointGeometry = null,
            Fill = new SolidColorBrush() { Opacity = 0.0, Color = default }
        };
        PreviousXPoint = 0.0;
    }
    public LineSeries LineSeries { get; set; }
    public double? PreviousXPoint { get; set; }

    private GraphPoint point;

    public GraphPoint Point
    {
        get => point;
        set
        {
            point = value;
            if (IsPointReady())
                AddPoint(point);
        }
    }

    private bool IsPointReady() => Point.X != null && Point.Y != null && (double)Point.X! > PreviousXPoint;

    private void AddPoint(GraphPoint point)
    {
        LineSeries.Values.Add(point);
        Point = new();
        PreviousXPoint = point.X;

        NotifyPropertyChanged();
    }

    private void AddPoints(List<GraphPoint> points)
    {
        // Change to have 'cached points' eventually
        LineSeries.Values.AddRange((IEnumerable<object>)points);
        Point = new();
        PreviousXPoint = points.Max(p => p.X);

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

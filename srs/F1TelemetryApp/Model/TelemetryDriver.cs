namespace F1TelemetryApp.Model;

using Enums;

using System.Windows.Media;
using System.Collections.Generic;
using System.ComponentModel;
using LiveCharts.Wpf;

public class TelemetryDriver : INotifyPropertyChanged
{
    public TelemetryDriver(string name, SolidColorBrush idColor, int currentLap, bool isUser = false)
    {
        Name = name;
        IdColor = idColor;
        CurrentLap = currentLap;
        IsUser = isUser;
        LapDataTypeSeriesHandlerMap = new();
        LapDataTypeSeriesHandlerMap.Add(CurrentLap, CreateDefaultDataTypeSeriesMap());
    }

    public string Name { get; }
    public SolidColorBrush IdColor { get; }
    public int CurrentLap { get; private set; }
    public bool IsUser { get; }
    public Dictionary<int, Dictionary<GraphDataType, LineSeriesHandler>> LapDataTypeSeriesHandlerMap { get; private set; }
    public Dictionary<GraphDataType, LineSeriesHandler> CurrentLapDataTypeSeriesHandlerMap => LapDataTypeSeriesHandlerMap[CurrentLap];

    public LineSeries GetLineSeries(int lap, GraphDataType type) => LapDataTypeSeriesHandlerMap[lap][type].LineSeries;

    public void UpdateVisibility(bool visible)
    {
        foreach (var lap in LapDataTypeSeriesHandlerMap.Keys)
        {
            var typeSeriesMap = LapDataTypeSeriesHandlerMap[lap];
            foreach (GraphDataType type in typeSeriesMap.Keys)
            {
                var typeSeries = typeSeriesMap[type];
                typeSeries.LineSeries.Stroke = visible ? IdColor : Brushes.Transparent;
                typeSeriesMap[type] = typeSeries;
            }
            LapDataTypeSeriesHandlerMap[lap] = typeSeriesMap;
        }

        NotifyPropertyChanged();
    }

    public void UpdateGraphPoint(GraphDataType type, double? x, double? y)
    {
        var point = CurrentLapDataTypeSeriesHandlerMap[type].Point;
        if (x != null)
            point.X = x;
        if (y != null)
            point.Y = y;

        LapDataTypeSeriesHandlerMap[CurrentLap][type].Point = new GraphPoint(point.X, point.Y);
        NotifyPropertyChanged();
    }

    public void UpdateLapNumber(int lapNum)
    {
        AddLap(lapNum);
        CurrentLap = lapNum;
        NotifyPropertyChanged();
    }

    public void AddLap(int lapNum)
    {
        if (lapNum <= 0 || LapDataTypeSeriesHandlerMap.ContainsKey(lapNum)) return;
        LapDataTypeSeriesHandlerMap.Add(lapNum, CreateDefaultDataTypeSeriesMap());
        NotifyPropertyChanged();
    }

    private Dictionary<GraphDataType, LineSeriesHandler> CreateDefaultDataTypeSeriesMap()
    {
        var dataTypeSeriesMap = new Dictionary<GraphDataType, LineSeriesHandler>();
        foreach (var type in EnumCollections.GraphDataTypes)
            dataTypeSeriesMap.Add(type, CreateLineSeriesHandler(type));

        return dataTypeSeriesMap;
    }

    private LineSeriesHandler CreateLineSeriesHandler(GraphDataType type) => new LineSeriesHandler(type, Name, IsUser ? 2 : 1, IdColor);

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}

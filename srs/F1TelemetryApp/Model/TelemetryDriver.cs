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
        LapDataMap = new() {{ CurrentLap, CreateDefaultDriverLapData(currentLap) }};
    }

    public string Name { get; }
    public SolidColorBrush IdColor { get; }
    public bool IsUser { get; }
    public int CurrentLap { get; private set; }
    public Dictionary<int, TelemetryDriverLapData> LapDataMap { get; private set; }
    public TelemetryDriverLapData CurrentLapData => LapDataMap[CurrentLap];

    public LineSeries GetLineSeries(int lap, GraphDataType type) => LapDataMap[lap].GetLineSeries(type);

    public void UpdateVisibility(bool visible)
    {
        foreach (var lapData in LapDataMap.Values)
            lapData.UpdateVisibility(visible);
        NotifyPropertyChanged();
    }

    public void UpdateGraphPoint(GraphDataType type, double? x, double? y)
    {
        CurrentLapData.UpdateGraphPoint(type, x, y);
        NotifyPropertyChanged();
    }

    public void UpdateCurrentLapNumber(int lapNum)
    {
        if (lapNum == CurrentLap) return;
        CurrentLap = lapNum;
        NotifyPropertyChanged();
    }

    public void AddLap(int lapNum)
    {
        if (lapNum < 1 || LapDataMap.ContainsKey(lapNum)) return;
        LapDataMap.Add(lapNum, CreateDefaultDriverLapData(lapNum));
        NotifyPropertyChanged();
    }

    private TelemetryDriverLapData CreateDefaultDriverLapData(int lap) => new(lap, Name, IsUser ? 2 : 1, IdColor);

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}

namespace F1TelemetryApp.Model;

using Timesheet;

using F1GameTelemetry.Enums;
using F1GameTelemetry.Models;

using System.ComponentModel;

public class TimesheetDriver : INotifyPropertyChanged
{
    private readonly int _numberOfSectors = 3;

    public TimesheetDriver(string name, byte arrayIndex)
    {
        Name = name;
        ArrayIndex = arrayIndex;
        LapData = new(_numberOfSectors);
        CurrentTyre = TyreVisual.Soft;
    }

    public string Name { get; init; }
    public byte ArrayIndex { get; init; }
    public int Position { get; set; }
    public TyreVisual CurrentTyre { get; set; }
    public LapDataCollection LapData { get; set; }

    public void UpdateLapHistoryData(int numLaps, LapHistoryData[] data)
    {
        if (numLaps == 0)
        {
            LapData.Clear();
            return;
        }

        bool isNewLap = numLaps != LapData.Count;
        if (isNewLap)
        {
            LapData.NewLap();

            if (numLaps > 1)
                LapData.UpdateLapData(numLaps - 2, data[numLaps - 2]);
        }

        LapData.UpdateLapData(numLaps - 1, data[numLaps - 1]);
    }

    public void SetPosition(int carPosition)
    {
        if (Position == carPosition)
            return;

        Position = carPosition;
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

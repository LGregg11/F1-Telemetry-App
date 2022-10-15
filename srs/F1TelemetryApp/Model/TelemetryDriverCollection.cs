namespace F1TelemetryApp.Model;

using Enums;
using Misc;

using System.Collections.Generic;
using System.Linq;

public class TelemetryDriverCollection
{
    public TelemetryDriverCollection()
    {
        Drivers = new();
    }

    public List<TelemetryDriver> Drivers { get; set; }

    public int Size => Drivers.Count;

    public void AddDriver(string name, int userIndex)
    {
        var numDrivers = Drivers.Count;
        int? latestLap = GetLatestLap();
        if (latestLap == null)
            latestLap = 0;
        Drivers.Add(new TelemetryDriver(name, GraphColors.Colors[numDrivers], (int)latestLap, numDrivers == userIndex));
    }

    public void AddLap(Lap lap)
    {
        foreach (var driver in Drivers)
            driver.AddLap(lap.LapNumber);
    }

    public void UpdateDriverVisibility(string name, bool visibile)
    {
        var driver = GetDriver(name);
        if (driver == null) return;

        driver.UpdateVisibility(visibile);
    }

    public GraphPointCollection CreateGraphPointCollection(int lap, GraphDataType type)
    {
        var collection = new GraphPointCollection(type);
        foreach (var driver in Drivers)
            collection.Series.Add(driver.GetLineSeries(lap, type));

        return collection;
    }

    public Dictionary<GraphDataType, GraphPointCollection> GetGraphPointCollectionMap(int lap)
    {
        var map = new Dictionary<GraphDataType, GraphPointCollection>();
        foreach (var type in EnumCollections.GraphDataTypes)
            map.Add(type, CreateGraphPointCollection(lap, type));

        return map;
    }

    public TelemetryDriver? GetDriver(string name) => Drivers.Any(d => d.Name == name) ? Drivers.First(d => d.Name == name) : null;

    public TelemetryDriver? GetDriver(int i) => Size - 1 > i && i >= 0 ? Drivers[i] : null;

    public List<int> GetLatestLaps() => Drivers.Select(d => d.CurrentLap).ToList();

    public int? GetLatestLap() => Drivers.Count > 0 ? Drivers.Max(d => d.CurrentLap) : null;

    public bool ContainsDriver(string name) => Drivers.Any(d => d.Name == name);
}

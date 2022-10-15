namespace F1TelemetryApp.Enums;

using System;
using System.Collections.Generic;

public enum GraphDataType
{
    Throttle,
    Brake,
    Gear,
    Speed,
    Steer
}

public static class EnumCollections
{
    public static IEnumerable<GraphDataType> GraphDataTypes => (IEnumerable<GraphDataType>)Enum.GetValues(typeof(GraphDataType));
}

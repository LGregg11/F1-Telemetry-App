using System;
using System.Collections.Generic;

namespace F1TelemetryApp.Enums;

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

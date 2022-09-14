namespace F1TelemetryApp.Model;

using Enums;

using LiveCharts;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public class GraphPointCollection
{
    public GraphPointCollection(GraphDataType type)
    {
        GraphType = type;
        Series = new();
    }

    public GraphDataType GraphType { get; set; }

    public SeriesCollection Series { get; set; }
}

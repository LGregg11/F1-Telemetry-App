namespace F1TelemetryApp.Model;

public struct GraphPoint
{
    public GraphPoint(double? x = null, double? y = null)
    {
        X = x;
        Y = y;
    }

    public double? X, Y;
}
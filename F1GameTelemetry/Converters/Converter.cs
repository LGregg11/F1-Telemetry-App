namespace F1GameTelemetry.Converters
{
    using F1GameTelemetry.Packets;

    using System;

    public static class Converter
    {
        public static double GetMagnitudeFromVectorData(VectorData vector)
        {
            return Math.Round(Math.Sqrt(Math.Pow(vector.x, 2) + Math.Pow(vector.y, 2) + Math.Pow(vector.z, 2)), 3);
        }
    }
}

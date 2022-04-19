namespace F1TelemetryApp.Model
{
    using F1GameTelemetry.Enums;

    public struct SessionMessage
    {
        public Track Track { get; set; }
        public Weather Weather { get; set; }
        public int TotalLaps { get; set; }
        public sbyte TrackTemperature { get; set; }
        public sbyte AirTemperature { get; set; }
        public byte AiDifficulty { get; set; }
    }
}

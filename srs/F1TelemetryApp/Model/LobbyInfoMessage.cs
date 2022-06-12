namespace F1TelemetryApp.Model
{
    using F1GameTelemetry.Enums;

    public struct LobbyInfoMessage
    {
        public int Players { get; set; }
        public string Name { get; set; }
        public Team Team { get; set; }
        public Nationality Nationality { get; set; }
    }
}

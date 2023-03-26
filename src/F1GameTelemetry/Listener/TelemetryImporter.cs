namespace F1GameTelemetry.Listener;

public class TelemetryImporter : TelemetryListener
{
    public TelemetryImporter(string filepath) : base(12345, new ImporterClient(filepath))
    {
    }
}

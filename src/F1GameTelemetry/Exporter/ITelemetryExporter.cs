namespace F1GameTelemetry.Exporter;

using F1GameTelemetry.Enums;

public interface ITelemetryExporter
{
    string Filepath { get; }
    float SessionUID { get; set; }
    void SetupNewFilePath(GameVersion gameVersion, ulong sessionUID);
    void ExportDataLine(byte[] data);
}
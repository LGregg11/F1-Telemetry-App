namespace F1GameTelemetry.Exporter;

using Enums;

using System;
using System.IO;

public class TelemetryExporterFileExistsException : Exception
{
    public TelemetryExporterFileExistsException()
    {
    }

    public TelemetryExporterFileExistsException(string message)
        : base(message)
    {
    }

    public TelemetryExporterFileExistsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

public class TelemetryExporter : ITelemetryExporter
{
    public static readonly string TELEMETRY_EXPORTER_DIRECTORY = $"{Environment.CurrentDirectory}\\Export_Data";
    public TelemetryExporter()
    {
        Filepath = TELEMETRY_EXPORTER_DIRECTORY;
    }

    public string Filepath { get; private set; }
    public float SessionUID { get; set; }

    public void SetupNewFilePath(GameVersion gameVersion, ulong sessionUID)
    {
        // Create the new file
        string directory = $"{TELEMETRY_EXPORTER_DIRECTORY}\\{Enum.GetName(gameVersion)}";
        string filePath = $"{directory}\\Data_{sessionUID}.txt";
        if (File.Exists(filePath))
            throw new TelemetryExporterFileExistsException($"{filePath} exists");

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        SessionUID = sessionUID;
        Filepath = filePath;
    }

    public void ExportDataLine(byte[] data)
    {
        using StreamWriter f = File.AppendText(Filepath);
        f.WriteLine(@$"{{ {string.Join(", ", data)} }}");
    }
}

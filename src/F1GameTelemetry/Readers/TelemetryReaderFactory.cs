namespace F1GameTelemetry.Readers;

using System.Collections.Generic;
using F1GameTelemetry.Readers.F12021;
using F1GameTelemetry.Enums;
using F1GameTelemetry.Listener;

public class TelemetryReaderFactory
{
    private readonly Dictionary<ReaderVersion, ITelemetryReader> _readerMap;

    public TelemetryReaderFactory(ITelemetryListener listener)
    {
        _readerMap = new Dictionary<ReaderVersion, ITelemetryReader>
        {
            { ReaderVersion.F12021, new TelemetryReader2021(listener) }
        };
    }

    public ITelemetryReader? GetTelemetryReader(ReaderVersion version)
    {
        if (_readerMap.ContainsKey(version))
            return _readerMap[version];

        return null;
    }
}

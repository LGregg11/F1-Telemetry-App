namespace F1GameTelemetry.Readers;

using F1GameTelemetry.Enums;
using F1GameTelemetry.Listener;
using F1GameTelemetry.Readers.F12021;
using System.Collections.Generic;

public class TelemetryReaderFactory
{
    private readonly Dictionary<GameVersion, ITelemetryReader> _readerMap;

    public TelemetryReaderFactory(ITelemetryListener listener)
    {
        _readerMap = new Dictionary<GameVersion, ITelemetryReader>
        {
            { GameVersion.F12021, new TelemetryReader2021(listener) }
        };
    }

    public ITelemetryReader? GetTelemetryReader(GameVersion version)
    {
        if (_readerMap.ContainsKey(version))
            return _readerMap[version];

        return null;
    }
}

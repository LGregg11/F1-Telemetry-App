namespace F1TelemetryApp.DataHandlers;

using Model;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Models;

internal static class GeneralDataHandler
{
    public static ObservableString SessionTime = new();

    public static void UpdateHeader(Header header)
    {
        InvokeDispatch.InvokeAsync(() =>
        {
            SessionTime.Value = (header.sessionTime * 1000).ToTelemetryTime();
        });
    }
}

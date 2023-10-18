namespace F1TelemetryApp.DataHandlers;

using System;
using System.Threading.Tasks;

internal static class InvokeDispatch
{
    public static void Invoke(Action action)
    {
        App.Current.Dispatcher.Invoke(action);
    }

    public static void InvokeAsync(Action action)
    {
        App.Current.Dispatcher.Invoke(async () =>
        {
            await Task.Run(action);
        });
    }
}
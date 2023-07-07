namespace F1GameTelemetry.Listener;

using F1GameTelemetry.Events;

public interface ITelemetryListener
{
    event TelemetryEventHandler TelemetryReceived;
    bool IsListenerRunning { get; }
    void Start();
    void Stop();
}

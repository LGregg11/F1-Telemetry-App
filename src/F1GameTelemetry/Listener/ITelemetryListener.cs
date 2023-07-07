namespace F1GameTelemetry.Listener;

using Events;

public interface ITelemetryListener
{
    event TelemetryEventHandler TelemetryReceived;
    bool IsListenerRunning { get; }
    void Start();
    void Stop();
}

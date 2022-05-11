namespace F1GameTelemetry.Listener;

using System.Threading;

public interface ITelemetryListener
{
    event TelemetryEventHandler TelemetryReceived;
    IUdpClient? Client { get; }
    Thread? ListenerThread { get; }
    int Port { get; }
    bool IsListenerRunning { get; }
    void Start();
    void Stop();
    void TelemetrySubscriber();
    Thread CreateThread();
    IUdpClient CreateClient();
}

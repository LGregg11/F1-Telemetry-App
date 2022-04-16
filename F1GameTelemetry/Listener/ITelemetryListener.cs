namespace F1GameTelemetry.Listener
{
    using System.Threading;

    public interface ITelemetryListener
    {
        event TelemetryEventHandler TelemetryReceived;
        int Port { get; }
        Thread ListenerThread { get; }
        IUdpClient Client { get; }
        bool IsListenerRunning { get; }
        void Start();
        void Stop();
        void TelemetrySubscriber();
        Thread CreateThread();
        IUdpClient CreateClient();
    }
}

namespace F1GameTelemetry.Listener
{
    using System.Net.Sockets;
    using System.Threading;

    public interface ITelemetryListener
    {
        event TelemetryEventHandler TelemetryReceived;
        int Port { get; }
        Thread ListenerThread { get; }
        UdpClient Client { get; }
        void Start();
        void Stop();
        void TelemetrySubscriber();
    }
}

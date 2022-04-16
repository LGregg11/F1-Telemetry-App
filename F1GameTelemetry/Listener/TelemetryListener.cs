namespace F1GameTelemetry.Listener
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class TelemetryListener : ITelemetryListener
    {
        public TelemetryListener(int port)
        {
            Port = port;
            Client = new UdpClient(Port);
            ListenerThread = new Thread(new ThreadStart(TelemetrySubscriber))
            {
                Name = "Telemetry Listener Thread"
            };
        }

        public event TelemetryEventHandler? TelemetryReceived;

        public int Port { get; }

        public Thread ListenerThread { get; private set; }

        public UdpClient Client { get; private set; }

        public bool IsListenerRunning => ListenerThread != null && ListenerThread.IsAlive;

        public void Start()
        {
            if (IsListenerRunning)
                return;

            ListenerThread.Start();
        }

        public void Stop()
        {
            if (!IsListenerRunning)
                return;

            Client?.Close();
            ListenerThread?.Join();
        }

        public void TelemetrySubscriber()
        {
            IPEndPoint ep = new(IPAddress.Any, Port);

            while (true)
            {
                try
                {
                    byte[]? receiveBytes = Client?.Receive(ref ep);
                    if (receiveBytes != null && receiveBytes.Length > 0)
                        TelemetryReceived?.Invoke(this, new TelemetryEventArgs(receiveBytes));
                }
                catch (SocketException)
                {
                    // Client closed - stop the thread
                    break;
                }
            }
        }
    }
}

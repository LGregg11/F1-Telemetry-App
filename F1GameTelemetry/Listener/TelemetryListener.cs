namespace F1GameTelemetry.Listener
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class TelemetryListener : ITelemetryListener
    {
        public TelemetryListener(int port) : this(port, null)
        {
        }

        public TelemetryListener(int port, IUdpClient? client)
        {
            Port = port;
            if (client == null)
                client = CreateClient();
            Client = client;
            ListenerThread = CreateThread();
        }

        public event TelemetryEventHandler? TelemetryReceived;

        public int Port { get; }

        public Thread? ListenerThread { get; private set; }

        public IUdpClient? Client { get; private set; }

        public bool IsListenerRunning => ListenerThread != null && ListenerThread.IsAlive;

        public void Start()
        {
            if (IsListenerRunning)
                return;

            if (Client == null)
                Client = CreateClient();
            ListenerThread = CreateThread();
            ListenerThread.Start();
        }

        public void Stop()
        {
            if (!IsListenerRunning)
                return;

            Client?.Close();
            ListenerThread?.Join();
            Client = null;
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

        public Thread CreateThread()
        {
            return new Thread(new ThreadStart(TelemetrySubscriber))
            {
                Name = "Telemetry Listener Thread"
            };
        }

        public IUdpClient CreateClient()
        {
            return new TelemetryUdpClient(Port);
        }
    }
}

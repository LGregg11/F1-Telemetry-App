namespace F1GameTelemetry.Listener
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class TelemetryListener : ITelemetryListener
    {
        private int _port;
        private Thread _listenerThread;
        private UdpClient _client;

        public TelemetryListener(int port)
        {
            _port = port;
        }

        public event TelemetryEventHandler TelemetryReceived;

        public int Port => _port;

        public Thread ListenerThread => _listenerThread;

        public UdpClient Client => _client;

        public bool IsListenerRunning => ListenerThread != null && ListenerThread.IsAlive;

        public void Start()
        {
            if (IsListenerRunning)
                return;

            _client = new UdpClient(Port);
            _listenerThread = new Thread(new ThreadStart(TelemetrySubscriber))
            {
                Name = "Telemetry Listener Thread"
            };

            _listenerThread.Start();
        }

        public void Stop()
        {
            if (!IsListenerRunning)
                return;

            _client?.Close();
            _listenerThread?.Join();
            _listenerThread = null;
            _client = null;
        }

        public void TelemetrySubscriber()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, _port);

            while (true)
            {
                try
                {
                    byte[] receiveBytes = _client.Receive(ref ep);
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

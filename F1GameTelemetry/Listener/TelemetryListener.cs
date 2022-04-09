namespace F1GameTelemetry.Listener
{
    using log4net;

    using System;
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

            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public event TelemetryEventHandler TelemetryReceived;

        public ILog Log { get; set; }

        public int Port => _port;

        public Thread ListenerThread => _listenerThread;

        public UdpClient Client => _client;

        public bool IsListenerRunning => ListenerThread != null && ListenerThread.IsAlive;

        public void Start()
        {
            if (IsListenerRunning)
            {
                Log?.Debug("Listener already running");
                return;
            }

            _client = new UdpClient(Port);
            _listenerThread = new Thread(new ThreadStart(TelemetrySubscriber))
            {
                Name = "Telemetry Listener Thread"
            };

            Log?.Info("Starting Telemetry listener");
            _listenerThread.Start();
        }

        public void Stop()
        {
            if (!IsListenerRunning)
            {
                Log?.Debug("Listener is not running");
                return;
            }

            Log?.Info("Stopping Telemetry listener");
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
                    Log?.Debug("Ending TelemetrySubscriber");
                    break;
                }
            }
        }
    }
}

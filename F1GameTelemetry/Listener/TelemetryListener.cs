namespace F1GameTelemetry.Listener
{
    using log4net;

    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class TelemetryListener : ITelemetryListener
    {
        private int port;
        private Thread listenerThread;
        private UdpClient client;

        public TelemetryListener(int port)
        {
            this.port = port;
            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public event TelemetryEventHandler TelemetryReceived;

        public ILog Log { get; set; }

        public int Port => port;

        public Thread ListenerThread => listenerThread;

        public UdpClient Client => client;

        public void Start()
        {
            if (ListenerThread != null && ListenerThread.IsAlive)
            {
                Log?.Debug("Client already started");
                return;
            }

            if (Client != null)
            {
                Log?.Debug("Client already started");
                return;
            }
            
            client = new UdpClient(Port);
            listenerThread = new Thread(new ThreadStart(TelemetrySubscriber))
            {
                Name = "Telemetry Listener Thread"
            };

            Log?.Info("Starting Telemetry listener");
            listenerThread.Start();
        }

        public void Stop()
        {
            if (ListenerThread == null || !ListenerThread.IsAlive)
            {
                Log?.Debug("Thread is not active");
                return;
            }
            
            if (Client == null)
            {
                Log?.Debug("Client is null");
                return;
            }

            Log?.Info("Stopping Telemetry listener");
            client.Close();
            listenerThread.Join();
            listenerThread = null;
            client = null;
        }

        public void TelemetrySubscriber()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);

            while (true)
            {
                try
                {
                    byte[] receiveBytes = client.Receive(ref ep);
                    if (receiveBytes != null && receiveBytes.Length > 0)
                        TelemetryReceived?.Invoke(this, new TelemetryEventArgs(receiveBytes));
                }
                catch (SocketException)
                {
                    Log?.Debug("Closing TelemetrySubscriber");
                    break;
                }
            }
        }
    }
}

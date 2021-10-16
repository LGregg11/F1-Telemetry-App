namespace UdpTelemetryFeed
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class UdpTelemetryFeed : IUdpTelemetryFeed
    {
        private int port;
        private Thread listenerThread;
        private UdpClient client;

        public UdpTelemetryFeed(int port)
        {
            this.port = port;
            listenerThread = new Thread(new ThreadStart(TelemetryListener))
            {
                Name = "Telemetry Listener Thread"
            };
        }

        public int Port => port;

        public Thread ListenerThread => listenerThread;

        public UdpClient Client => client;

        public void Start()
        {
            if (ListenerThread.IsAlive)
                return;

            listenerThread.Start();
        }

        public void Stop()
        {
            if (!ListenerThread.IsAlive)
                return;
            
            client.Close();
            listenerThread.Interrupt();
            listenerThread.Join(5000);
            client = null;
        }

        public void TelemetryListener()
        {
            client = new UdpClient(port);
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);

            while (true)
            {
                try
                {
                    byte[] receiveBytes = client.Receive(ref ep);
                    if (receiveBytes != null && receiveBytes.Length > 0)
                        continue;
                        // Send an event here
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode != 10060)
                        throw ex;
                    Stop();
                }
            }
        }
    }
}

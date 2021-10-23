namespace F1GameTelemetry.Listener
{
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
        }

        public event TelemetryEventHandler TelemetryReceived;

        public int Port => port;

        public Thread ListenerThread => listenerThread;

        public UdpClient Client => client;

        public void Start()
        {
            if (ListenerThread != null && ListenerThread.IsAlive)
                return;

            if (Client == null)
                client = new UdpClient(Port);

            listenerThread = new Thread(new ThreadStart(TelemetrySubscriber))
            {
                Name = "Telemetry Listener Thread"
            };
            listenerThread.Start();
        }

        public void Stop()
        {
            if (ListenerThread == null || !ListenerThread.IsAlive)
                return;
            
            if (Client != null)
                client.Close();
            listenerThread.Abort();
            listenerThread.Join(5000);
            listenerThread = null;
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
                        TelemetryReceived(this, new TelemetryEventArgs(receiveBytes));
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}

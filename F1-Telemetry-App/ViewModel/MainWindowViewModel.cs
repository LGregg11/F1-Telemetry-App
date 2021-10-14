namespace F1_Telemetry_App.ViewModel
{
    using GalaSoft.MvvmLight;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using log4net;

    public class MainWindowViewModel : ViewModelBase
    {
        private const int port = 20777;

        private bool isTelemetryFeedRunning = false;
        private int nMessages = 0;
        private UdpClient client;
        private Thread listenerThread;

        public MainWindowViewModel()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public ILog Log { get; set; }

        public int NMessages
        {
            get => nMessages;
            set
            {
                if (value != nMessages)
                {
                    nMessages = value;
                    RaisePropertyChanged("NMessages");
                }
            }
        }

        public bool IsTelemetryFeedRunning
        {
            get => isTelemetryFeedRunning;
            set
            {
                if (value != isTelemetryFeedRunning)
                {
                    isTelemetryFeedRunning = !isTelemetryFeedRunning;
                    RaisePropertyChanged("IsTelemetryFeedRunning");
                }
            }
        }

        public void StartTelemetryFeed()
        {
            if (IsTelemetryFeedRunning)
                return;

            Log.Debug("Starting Telemetry Feed");
            listenerThread = new Thread(new ThreadStart(TelemetryListener))
            {
                Name = "Listener Thread"
            };
            listenerThread.Start();
            Log.Info("Started Telemetry Feed");
            IsTelemetryFeedRunning = true;
        }

        public void StopTelemetryFeed()
        {
            if (!IsTelemetryFeedRunning)
                return;

            Log.Debug("Stopping Telemetry Feed");
            client.Close();
            listenerThread.Abort();
            listenerThread.Join(5000);
            listenerThread = null;
            Log.Info("Stopped Telemetry Feed");
            IsTelemetryFeedRunning = false;
        }

        private void TelemetryListener()
        {
            Log.Debug($"Creating UDP Client to listen to UDP on port {port}");
            client = new UdpClient(port);
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);

            while (true)
            {
                try
                {
                    byte[] receiveBytes = client.Receive(ref ep);
                    string message = Encoding.ASCII.GetString(receiveBytes);
                    NMessages++;
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode != 10060)
                        Log.Error($"TelemetryListener - {ex.ErrorCode}");
                    else
                        Log.Warn("TelemetryListener - Expected timeout");
                }
            }
        }

        
    }
}

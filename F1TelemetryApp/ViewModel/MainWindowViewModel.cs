namespace F1TelemetryApp.ViewModel
{
    using F1_Telemetry_App.Model;
    using GalaSoft.MvvmLight;
    using log4net;
    using UdpTelemetryFeed;
    using System.Linq;
    using UdpPackets;
    using System;
    using System.Collections.ObjectModel;

    public class MainWindowViewModel : ViewModelBase
    {
        private const int port = 20777;

        private ObservableCollection<TelemetryMessages> nMessages;
        private IUdpTelemetryFeed telemetryFeed;

        public MainWindowViewModel()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            var packetIds = Enum.GetValues(typeof(TelemetryReader.PacketIds));
            nMessages = new ObservableCollection<TelemetryMessages>();
            foreach(TelemetryReader.PacketIds id in packetIds)
                nMessages.Add(new TelemetryMessages { PacketId = id, Messages = 0 });

            telemetryFeed = new UdpTelemetryFeed(port);
            telemetryFeed.TelemetryReceived += OnTelemetryReceived;
        }

        public ILog Log { get; set; }

        public ObservableCollection<TelemetryMessages> NMessages
        {
            get => nMessages;
            set
            {
                if (value != nMessages)
                {
                    nMessages = value;
                }
            }
        }

        public void StartTelemetryFeed()
        {
            Log.Debug("Starting Telemetry Feed");
            telemetryFeed.Start();
            Log.Info("Started Telemetry Feed");
        }

        public void StopTelemetryFeed()
        {
            Log.Debug("Stopping Telemetry Feed");
            telemetryFeed.Stop();
            Log.Info("Stopped Telemetry Feed");
        }

        private void OnTelemetryReceived(object source, UdpTelemetryEventArgs e)
        {
            UdpPacketHeader udpPacketHeader = TelemetryReader.ByteArrayToUdpPacketHeader(e.Message);
            byte[] data = e.Message.Skip(TelemetryReader.TELEMETRY_HEADER_SIZE).ToArray();
            var telemetryMessage = NMessages.Where(t => t.PacketId == (TelemetryReader.PacketIds)udpPacketHeader.packetId).FirstOrDefault();
            telemetryMessage.Messages++;

            App.Current.Dispatcher.Invoke(delegate
            {
                UpdateNMessages(telemetryMessage);
            });
        }

        private void UpdateNMessages(TelemetryMessages message)
        {
            for (int i = 0; i < NMessages.Count; i++)
            {
                if (NMessages[i].PacketId == message.PacketId)
                {
                    NMessages[i] = message;
                    break;
                }
            }
        }
    }
}

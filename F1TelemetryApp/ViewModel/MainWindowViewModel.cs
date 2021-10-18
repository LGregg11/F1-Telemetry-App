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
    using static F1_Telemetry_App.Model.TelemetryReader;

    public class MainWindowViewModel : ViewModelBase
    {
        private const int port = 20777;

        private ObservableCollection<HeaderMessage> headerMessages;
        private ObservableCollection<EventMessage> eventMessages;
        private IUdpTelemetryFeed telemetryFeed;

        public MainWindowViewModel()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            PopulateHeaderMessages();
            PopulateEventMessages();

            telemetryFeed = new UdpTelemetryFeed(port);
            telemetryFeed.TelemetryReceived += OnTelemetryReceived;
        }

        public ILog Log { get; set; }

        public ObservableCollection<HeaderMessage> HeaderMessages
        {
            get => headerMessages;
            set
            {
                if (value != headerMessages)
                {
                    headerMessages = value;
                }
            }
        }

        public ObservableCollection<EventMessage> EventMessages
        {
            get => eventMessages;
            set
            {
                if (value != eventMessages)
                {
                    eventMessages = value;
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

        #region Telemetry Handler

        private void OnTelemetryReceived(object source, UdpTelemetryEventArgs e)
        {
            UdpPacketHeader udpPacketHeader = ByteArrayToUdpPacketStruct<UdpPacketHeader>(e.Message);
            byte[] remainingPacket = e.Message.Skip(TELEMETRY_HEADER_SIZE).ToArray();

            App.Current.Dispatcher.Invoke(delegate
            {
                UpdateNMessages(udpPacketHeader);

                switch ((PacketIds)udpPacketHeader.packetId)
                {
                    case PacketIds.Event:
                        UpdateEvents(remainingPacket);
                        break;
                    case PacketIds.Motion:
                    case PacketIds.Session:
                    case PacketIds.LapData:
                    case PacketIds.Participants:
                    case PacketIds.CarSetups:
                    case PacketIds.CarTelemetry:
                    case PacketIds.CarStatus:
                    case PacketIds.FinalClassification:
                    case PacketIds.LobbyInfo:
                    case PacketIds.CarDamage:
                    case PacketIds.SessionHistory:
                    default:
                        break;
                }
            });

        }

        #endregion

        #region Populators
        private void PopulateHeaderMessages()
        {
            var packetIds = Enum.GetValues(typeof(PacketIds));
            headerMessages = new ObservableCollection<HeaderMessage>();
            foreach (PacketIds id in packetIds)
                headerMessages.Add(new HeaderMessage { PacketId = id, Total = 0 });
        }

        private void PopulateEventMessages()
        {
            var eventTypes = Enum.GetValues(typeof(EventType));
            eventMessages = new ObservableCollection<EventMessage>();
            foreach (EventType eventType in eventTypes)
                eventMessages.Add(new EventMessage { EventType = eventType, Total = 0 });
        }
        #endregion

        #region Update Fields

        private void UpdateNMessages(UdpPacketHeader udpPacketHeader)
        {
            for (int i = 0; i < HeaderMessages.Count; i++)
            {
                if (HeaderMessages[i].PacketId == (PacketIds)udpPacketHeader.packetId)
                {
                    HeaderMessage headerMessage = HeaderMessages[i];
                    headerMessage.Total++;

                    HeaderMessages[i] = headerMessage;
                    break;
                }
            }
        }

        private void UpdateEvents(byte[] eventPacket)
        {
            EventType eventType = GetEventType(eventPacket);
            for (int i = 0; i < EventMessages.Count; i++)
            {
                if (EventMessages[i].EventType == eventType)
                {
                    EventMessage eventMessage = EventMessages[i];
                    eventMessage.Total++;

                    EventMessages[i] = eventMessage;
                    break;
                }
            }
        }

        #endregion
    }
}

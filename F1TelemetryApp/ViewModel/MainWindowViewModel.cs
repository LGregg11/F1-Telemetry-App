namespace F1TelemetryApp.ViewModel
{
    using F1GameTelemetry.Packets;
    using F1GameTelemetry.Listener;
    using F1TelemetryApp.Model;

    using GalaSoft.MvvmLight;
    using log4net;
    using System.Linq;
    using System;
    using System.Collections.ObjectModel;

    using static F1GameTelemetry.Reader.TelemetryReader;

    public class MainWindowViewModel : ViewModelBase
    {
        private const int port = 20777;

        private ObservableCollection<HeaderMessage> headerMessages;
        private ObservableCollection<EventMessage> eventMessages;
        private TelemetryListener telemetryListener;

        public MainWindowViewModel()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            PopulateHeaderMessages();
            PopulateEventMessages();

            telemetryListener = new TelemetryListener(port);
            telemetryListener.TelemetryReceived += OnTelemetryReceived;
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
            telemetryListener.Start();
            Log.Info("Started Telemetry Feed");
        }

        public void StopTelemetryFeed()
        {
            Log.Debug("Stopping Telemetry Feed");
            telemetryListener.Stop();
            Log.Info("Stopped Telemetry Feed");
        }

        #region Telemetry Handler

        private void OnTelemetryReceived(object source, TelemetryEventArgs e)
        {
            Header header = ByteArrayToUdpPacketStruct<Header>(e.Message);
            byte[] remainingPacket = e.Message.Skip(TELEMETRY_HEADER_SIZE).ToArray();

            App.Current.Dispatcher.Invoke(delegate
            {
                UpdateNMessages(header);

                switch ((PacketIds)header.packetId)
                {
                    case PacketIds.Event:
                        UpdateEvents(remainingPacket);
                        break;
                    case PacketIds.Motion:
                        Log.Debug($"same Motion packet - new byte[] {{ {string.Join(", ", remainingPacket)} }}");
                        break;
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

        private void UpdateNMessages(Header udpPacketHeader)
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

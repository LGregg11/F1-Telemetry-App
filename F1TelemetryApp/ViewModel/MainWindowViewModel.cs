namespace F1TelemetryApp.ViewModel
{
    using F1GameTelemetry.Listener;
    using F1TelemetryApp.Model;
    using F1GameTelemetry.Packets;
    using F1GameTelemetry.Packets.Enums;

    using GalaSoft.MvvmLight;
    using log4net;
    using System.Linq;
    using System;
    using System.Collections.ObjectModel;

    using static F1GameTelemetry.Reader.TelemetryReader;
    using static F1GameTelemetry.Converters.Converter;

    public class MainWindowViewModel : ViewModelBase
    {
        private const int port = 20777;

        private ObservableCollection<HeaderMessage> headerMessages;
        private ObservableCollection<EventMessage> eventMessages;
        private MotionMessage motionMessage;
        private TelemetryMessage telemetryMessage;

        private TelemetryListener telemetryListener;

        public MainWindowViewModel()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            PopulateMessages();

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

        public string LocalSpeed => $"{motionMessage.Speed:#0.00} m/s";

        public string Speed => $"{telemetryMessage.Speed} km/h";

        public string Throttle => $"{telemetryMessage.Throttle}";

        public string Brake => $"{telemetryMessage.Brake}";

        public string Gear => $"{telemetryMessage.Gear}";

        public string Steer => $"{telemetryMessage.Steer}";

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
                        UpdateMotion(remainingPacket);
                        break;
                    case PacketIds.CarTelemetry:
                        UpdateTelemetry(remainingPacket);
                        break;
                    case PacketIds.CarStatus:
                        // Log.Debug($"Car Status packet - new byte[] {{ {string.Join(", ", remainingPacket)} }}");
                        // break;
                    case PacketIds.Session:
                    case PacketIds.LapData:
                    case PacketIds.Participants:
                    case PacketIds.CarSetups:
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

        private void PopulateMessages()
        {
            PopulateHeaderMessages();
            PopulateEventMessages();
            PopulateMotionMessage();
            PopulateTelemetryMessage();
        }

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

        private void PopulateMotionMessage()
        {
            motionMessage = new MotionMessage { Speed = 0.0d };
        }

        private void PopulateTelemetryMessage()
        {
            telemetryMessage = new TelemetryMessage { Speed = 0, Brake = 0.0f, Throttle = 0.0f, Gear = 0, Steer = 0.0f };
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

        private void UpdateMotion(byte[] motionPacket)
        {
            var motion = GetMotionStruct(motionPacket);
            motionMessage.Speed = GetMagnitudeFromVectorData(motion.extraCarMotionData.localVelocity);
            RaisePropertyChanged(nameof(LocalSpeed));
        }

        private void UpdateTelemetry(byte[] carTelemetryPacket)
        {
            var carTelemetry = GetCarTelemetryStruct(carTelemetryPacket);
            telemetryMessage.Speed = carTelemetry.carTelemetryData[0].speed;
            telemetryMessage.Throttle = carTelemetry.carTelemetryData[0].throttle;
            telemetryMessage.Brake = carTelemetry.carTelemetryData[0].brake;
            telemetryMessage.Gear = carTelemetry.carTelemetryData[0].gear;
            telemetryMessage.Steer = carTelemetry.carTelemetryData[0].steer;
            RaisePropertyChanged(nameof(Speed));
            RaisePropertyChanged(nameof(Throttle));
            RaisePropertyChanged(nameof(Brake));
            RaisePropertyChanged(nameof(Gear));
            RaisePropertyChanged(nameof(Steer));
        }

        #endregion
    }
}

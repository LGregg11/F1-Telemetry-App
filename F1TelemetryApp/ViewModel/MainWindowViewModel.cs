namespace F1TelemetryApp.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using GalaSoft.MvvmLight;
    using log4net;

    using F1GameTelemetry.Listener;
    using F1TelemetryApp.Model;
    using F1GameTelemetry.Packets;
    using F1GameTelemetry.Packets.Enums;
    using F1GameTelemetry.Converters;

    using static F1GameTelemetry.Reader.TelemetryReader;
    using System.Collections.Generic;

    public class MainWindowViewModel : ViewModelBase
    {
        private const int port = 20777;

        private ObservableCollection<HeaderMessage> headerMessages;
        private ObservableCollection<EventMessage> eventMessages;
        private MotionMessage motionMessage;
        private TelemetryMessage telemetryMessage;
        private LapDataMessage lapDataMessage;
        private SessionMessage sessionMessage;
        private ParticipantMessage participantMessage;
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

        public bool IsListenerRunning => telemetryListener.IsListenerRunning;

        public string LocalSpeed => $"{motionMessage.Speed:#0.00} m/s";

        public string Speed => $"{telemetryMessage.Speed} km/h";

        public string Throttle => $"{telemetryMessage.Throttle}";

        public string Brake => $"{telemetryMessage.Brake}";

        public string Gear => $"{telemetryMessage.Gear}";

        public string Steer => $"{telemetryMessage.Steer}";

        public string LastLapTime => lapDataMessage.LastLapTime.ToTelemetryTime();

        public string TrackName => Enum.GetName(typeof(Track), sessionMessage.Track);

        public string WeatherStatus => Enum.GetName(typeof(Weather), sessionMessage.Weather);

        public string TrackTemperature => $"{Convert.ToInt32(sessionMessage.TrackTemperature)}";

        public string AirTemperature => $"{Convert.ToInt32(sessionMessage.AirTemperature)}";

        public string TotalLaps => $"{sessionMessage.TotalLaps}";

        public string AiDifficulty => $"{Convert.ToUInt32(sessionMessage.AiDifficulty)}";

        public Dictionary<string, string> Participants => participantMessage.Participants;


        public void StartTelemetryFeed()
        {
            if (!telemetryListener.IsListenerRunning)
            {
                Log?.Info("Starting Telemetry feed");
                telemetryListener.Start();
            }
        }

        public void StopTelemetryFeed()
        {
            if (telemetryListener.IsListenerRunning)
            {
                Log?.Info("Stopping Telemetry feed");
                telemetryListener.Stop();
            }
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
                        break;
                    case PacketIds.FinalClassification:
                        break;
                    case PacketIds.LapData:
                        UpdateLapData(remainingPacket);
                        break;
                    case PacketIds.Session:
                        UpdateSession(remainingPacket);
                        break;
                    case PacketIds.Participants:
                        UpdateParticipants(remainingPacket);
                        break;
                    case PacketIds.CarSetups:
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
            PopulateLapDataMessage();
            PopulateSessionMessage();
            PopulateParticipantMessage();
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

        private void PopulateLapDataMessage()
        {
            lapDataMessage = new LapDataMessage { LastLapTime = 0 };
        }

        private void PopulateSessionMessage()
        {
            sessionMessage = new SessionMessage { Track = Track.Unknown, Weather = Weather.Unknown, TotalLaps = 0, TrackTemperature = 0, AirTemperature = 0, AiDifficulty = 0 };
        }

        private void PopulateParticipantMessage()
        {
            participantMessage = new ParticipantMessage { Participants = new Dictionary<string, string>() };
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
            motionMessage.Speed = Converter.GetMagnitudeFromVectorData(motion.extraCarMotionData.localVelocity);
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

        private void UpdateLapData(byte[] lapDataPacket)
        {
            var lapData = GetLapDataStruct(lapDataPacket);
            // TODO: 0 isn't the index of 'my' car - is there a way of knowing?
            lapDataMessage.LastLapTime = lapData.carLapData[0].lastLapTime;
            RaisePropertyChanged(nameof(LastLapTime));
        }

        private void UpdateSession(byte[] sessionPacket)
        {
            var session = GetSessionStruct(sessionPacket);
            sessionMessage.Track = session.trackId;
            sessionMessage.Weather = session.weather;
            sessionMessage.TrackTemperature = session.trackTemperature;
            sessionMessage.AirTemperature = session.airTemperature;
            sessionMessage.AiDifficulty = session.aiDifficulty;
            sessionMessage.TotalLaps = session.totalLaps;
            RaisePropertyChanged(nameof(TrackName));
            RaisePropertyChanged(nameof(WeatherStatus));
            RaisePropertyChanged(nameof(TotalLaps));
            RaisePropertyChanged(nameof(TrackTemperature));
            RaisePropertyChanged(nameof(AirTemperature));
            RaisePropertyChanged(nameof(AiDifficulty));
        }

        private void UpdateParticipants(byte[] participantPacket)
        {
            var participant = GetParticipantStruct(participantPacket);
            var participants = new Dictionary<string, string>();
            foreach (var p in participant.participants)
            {
                if (!string.IsNullOrEmpty(p.name))
                    participants.Add(p.name, Enum.GetName(typeof(Nationality), p.nationality));
            }

            participantMessage.Participants = participants;
            RaisePropertyChanged(nameof(Participants));
        }

        #endregion
    }
}

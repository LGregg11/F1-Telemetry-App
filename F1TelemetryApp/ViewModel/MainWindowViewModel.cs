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
    using F1GameTelemetry.Enums;
    using F1GameTelemetry.Converters;

    using static F1GameTelemetry.Reader.TelemetryReader;
    using System.Collections.Generic;
    using System.Data;

    public class MainWindowViewModel : ViewModelBase
    {
        private const int port = 20777;

        private ObservableCollection<HeaderMessage> headerMessages;
        private ObservableCollection<EventMessage> eventMessages;
        private MotionMessage motionMessage;
        private TelemetryMessage telemetryMessage;
        private LapDataMessage lapDataMessage;
        private SessionMessage sessionMessage;
        private DataTable sessionHistoryTable;
        private ParticipantMessage participantMessage;
        private LobbyInfoMessage lobbyInfoMessage;
        private CarDamageMessage carDamageMessage;
        private CarSetupMessage carSetupMessage;
        private TelemetryListener telemetryListener;
        private ReaderVersion _readerVersion = ReaderVersion.F12021;
        private int _myCarIndex = -1;

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

        public List<ReaderVersion> Versions => new List<ReaderVersion>((IEnumerable<ReaderVersion>)Enum.GetValues(typeof(ReaderVersion)));

        public ReaderVersion Version
        {
            get => _readerVersion;

            set
            {
                if (_readerVersion != value)
                {
                    _readerVersion = value;
                    RaisePropertyChanged(nameof(Version));
                }
            }
        }

        #region Gui Test Properties

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

        public DataTable SessionHistory => sessionHistoryTable;

        public string LobbyPlayers => $"{lobbyInfoMessage.Players}";

        public string LobbyName => lobbyInfoMessage.Name;

        public string LobbyTeam => Enum.GetName(typeof(Team), lobbyInfoMessage.Team);

        public string LobbyNationality => Enum.GetName(typeof(Nationality), lobbyInfoMessage.Nationality);

        public string RLTyreWear => $"{carDamageMessage.TyreWear[0]:F1}%";

        public string RRTyreWear => $"{carDamageMessage.TyreWear[1]:F1}%";

        public string FLTyreWear => $"{carDamageMessage.TyreWear[2]:F1}%";

        public string FRTyreWear => $"{carDamageMessage.TyreWear[3]:F1}%";

        public string FrontLeftWingDamage => $"{carDamageMessage.FrontLeftWingDamage:F1}%";

        public string FrontRightWingDamage => $"{carDamageMessage.FrontRightWingDamage:F1}%";

        public string BrakeBias => $"{carSetupMessage.BrakeBias}";

        public string FuelLoad => $"{carSetupMessage.FuelLoad:F2}";

        #endregion

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
            Header header = BytesToPacket<Header>(e.Message);
            byte[] remainingPacket = e.Message.Skip(TELEMETRY_HEADER_SIZE).ToArray();
            if (_myCarIndex < 0)
                _myCarIndex = header.playerCarIndex;

            App.Current.Dispatcher.Invoke(delegate
            {
                UpdateNMessages(header);

                switch ((PacketId)header.packetId)
                {
                    case PacketId.Motion:
                        UpdateMotion(remainingPacket);
                        break;
                    case PacketId.Session:
                        UpdateSession(remainingPacket);
                        break;
                    case PacketId.LapData:
                        UpdateLapData(remainingPacket);
                        break;
                    case PacketId.Event:
                        UpdateEvents(remainingPacket);
                        break;
                    case PacketId.Participants:
                        UpdateParticipants(remainingPacket);
                        break;
                    case PacketId.CarSetups:
                        UpdateCarSetup(remainingPacket);
                        break;
                    case PacketId.CarTelemetry:
                        UpdateTelemetry(remainingPacket);
                        break;
                    case PacketId.CarStatus:
                        break;
                    case PacketId.FinalClassification:
                        break;
                    case PacketId.LobbyInfo:
                        UpdateLobbyInfo(remainingPacket);
                        break;
                    case PacketId.CarDamage:
                        UpdateCarDamage(remainingPacket);
                        break;
                    case PacketId.SessionHistory:
                        UpdateSessionHistory(remainingPacket);
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
            PopulateSessionHistoryMessage();
            PopulateLobbyInfoMessage();
            PopulateCarDamageMessage();
        }

        private void PopulateHeaderMessages()
        {
            var packetIds = Enum.GetValues(typeof(PacketId));
            headerMessages = new ObservableCollection<HeaderMessage>();
            foreach (PacketId id in packetIds)
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

        private void PopulateLobbyInfoMessage()
        {
            lobbyInfoMessage = new LobbyInfoMessage { Players = 0, Name = "", Nationality = Nationality.Unknown, Team = Team.Unknown };
        }

        private void PopulateSessionHistoryMessage()
        {
            sessionHistoryTable = new DataTable();
            sessionHistoryTable.Columns.Add("Pos", typeof(int));
            sessionHistoryTable.Columns.Add("Name", typeof(string));
            sessionHistoryTable.Columns.Add("Laps", typeof(int));
            sessionHistoryTable.Columns.Add("Sector1", typeof(float));
            sessionHistoryTable.Columns.Add("Sector2", typeof(float));
            sessionHistoryTable.Columns.Add("Sector3", typeof(float));
            sessionHistoryTable.Columns.Add("LastLap", typeof(string));
        }

        private void PopulateCarDamageMessage()
        {
            carDamageMessage = new CarDamageMessage { TyreWear = new float[4] { 0f, 0f, 0f, 0f }, FrontLeftWingDamage = 0, FrontRightWingDamage = 0 };
        }

        private void PopularCarSetupMessage()
        {
            carSetupMessage = new CarSetupMessage { BrakeBias = 0, FuelLoad = 0f };
        }
        #endregion

        #region Update Fields

        private void UpdateNMessages(Header udpPacketHeader)
        {
            for (int i = 0; i < HeaderMessages.Count; i++)
            {
                if (HeaderMessages[i].PacketId == (PacketId)udpPacketHeader.packetId)
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
            var motion = GetMotion(motionPacket);
            motionMessage.Speed = Converter.GetMagnitudeFromVectorData(motion.extraCarMotionData.localVelocity);
            RaisePropertyChanged(nameof(LocalSpeed));
        }

        private void UpdateTelemetry(byte[] carTelemetryPacket)
        {
            var carTelemetry = GetCarTelemetry(carTelemetryPacket);
            telemetryMessage.Speed = carTelemetry.carTelemetryData[_myCarIndex].speed;
            telemetryMessage.Throttle = carTelemetry.carTelemetryData[_myCarIndex].throttle;
            telemetryMessage.Brake = carTelemetry.carTelemetryData[_myCarIndex].brake;
            telemetryMessage.Gear = carTelemetry.carTelemetryData[_myCarIndex].gear;
            telemetryMessage.Steer = carTelemetry.carTelemetryData[_myCarIndex].steer;
            RaisePropertyChanged(nameof(Speed));
            RaisePropertyChanged(nameof(Throttle));
            RaisePropertyChanged(nameof(Brake));
            RaisePropertyChanged(nameof(Gear));
            RaisePropertyChanged(nameof(Steer));
        }

        private void UpdateLapData(byte[] lapDataPacket)
        {
            var lapData = GetLapData(lapDataPacket);
            lapDataMessage.LastLapTime = lapData.carLapData[_myCarIndex].lastLapTime;
            RaisePropertyChanged(nameof(LastLapTime));
        }

        private void UpdateSession(byte[] sessionPacket)
        {
            var session = GetSession(sessionPacket);
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
            var participant = GetParticipant(participantPacket);
            var participants = new Dictionary<string, string>();
            foreach (var p in participant.participants)
            {
                if (!string.IsNullOrEmpty(p.name) && !participants.ContainsKey(p.name))
                    participants.Add(p.name, Enum.GetName(typeof(Nationality), p.nationality));
            }

            participantMessage.Participants = participants;
            RaisePropertyChanged(nameof(Participants));
        }

        private void UpdateSessionHistory(byte[] sessionHistoryPacket)
        {
            var history = GetSessionHistory(sessionHistoryPacket);
            // TODO: Add converter for sector time like this (00.000)

            var name = ((int)history.carIdx).ToString();
            if (_myCarIndex == (int)history.carIdx)
            {
                name = "your CAR";
            }

            
            DataRow row = sessionHistoryTable.Select($"Name='{name}'").FirstOrDefault();
            if (row == null)
            {
                row = sessionHistoryTable.NewRow();
                row["LastLap"] = 0f.ToTelemetryTime();
                sessionHistoryTable.Rows.Add(row);
            }

            row["Pos"] = 0;
            row["Name"] = name;
            row["Laps"] = history.numLaps;
            var i = history.numLaps - 1;
            var sector = (float)history.lapHistoryData[i].sector1Time;
            if (sector > 0)
                row["Sector1"] = sector/1000;

            sector = history.lapHistoryData[i].sector2Time;
            if (sector > 0)
                row["Sector2"] = sector / 1000;

            if (i < 1) return;

            i--;
            sector = history.lapHistoryData[i].sector3Time;
            if (sector > 0)
                row["Sector3"] = sector / 1000;

            sector = history.lapHistoryData[i].lapTime;
            if (sector > 0)
                row["LastLap"] = sector.ToTelemetryTime();

            RaisePropertyChanged(nameof(SessionHistory));
        }

        private void UpdateLobbyInfo(byte[] infoPacket)
        {
            var info = GetLobbyInfo(infoPacket);
            lobbyInfoMessage.Players = info.numPlayers;
            lobbyInfoMessage.Name = info.lobbyPlayers.FirstOrDefault().name;
            lobbyInfoMessage.Nationality = info.lobbyPlayers.FirstOrDefault().nationality;
            lobbyInfoMessage.Team = info.lobbyPlayers.FirstOrDefault().teamId;
            RaisePropertyChanged(nameof(LobbyPlayers));
            RaisePropertyChanged(nameof(LobbyName));
            RaisePropertyChanged(nameof(LobbyNationality));
            RaisePropertyChanged(nameof(LobbyTeam));
        }

        private void UpdateCarDamage(byte[] damagePacket)
        {
            var damage = GetCarDamage(damagePacket);
            carDamageMessage.TyreWear = damage.carDamageData[_myCarIndex].tyreWear;
            carDamageMessage.FrontLeftWingDamage = damage.carDamageData[_myCarIndex].frontLeftWingDamage;
            carDamageMessage.FrontRightWingDamage = damage.carDamageData[_myCarIndex].frontRightWingDamage;
            RaisePropertyChanged(nameof(FLTyreWear));
            RaisePropertyChanged(nameof(FRTyreWear));
            RaisePropertyChanged(nameof(RLTyreWear));
            RaisePropertyChanged(nameof(RRTyreWear));
            RaisePropertyChanged(nameof(FrontLeftWingDamage));
            RaisePropertyChanged(nameof(FrontRightWingDamage));
        }

        private void UpdateCarSetup(byte[] setupPacket)
        {
            var setup = GetCarSetup(setupPacket);
            carSetupMessage.BrakeBias = setup.carSetupData[_myCarIndex].brakeBias;
            carSetupMessage.FuelLoad = setup.carSetupData[_myCarIndex].fuelLoad;
            RaisePropertyChanged(nameof(BrakeBias));
            RaisePropertyChanged(nameof(FuelLoad));
        }
        #endregion
    }
}

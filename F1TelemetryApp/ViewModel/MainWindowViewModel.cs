﻿namespace F1TelemetryApp.ViewModel
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
            sessionHistoryTable.Columns.Add("Sector 1", typeof(float));
            sessionHistoryTable.Columns.Add("Sector 2", typeof(float));
            sessionHistoryTable.Columns.Add("Sector 3", typeof(float));
            sessionHistoryTable.Columns.Add("Last Lap", typeof(string));
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
            var lapData = GetLapData(lapDataPacket);
            // TODO: 0 isn't the index of 'my' car - is there a way of knowing?
            lapDataMessage.LastLapTime = lapData.carLapData[0].lastLapTime;
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
                if (!string.IsNullOrEmpty(p.name))
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

            
            DataRow row = sessionHistoryTable.Select($"Name='{name}'").FirstOrDefault();
            if (row == null)
            {
                row = sessionHistoryTable.NewRow();
                row["Last Lap"] = 0f.ToTelemetryTime();
                sessionHistoryTable.Rows.Add(row);
            }

            row["Pos"] = 0;
            row["Name"] = name;
            row["Laps"] = history.numLaps;
            var i = history.numLaps - 1;
            var sector = (float)history.lapHistoryData[i].sector1Time;
            if (sector > 0)
                row["Sector 1"] = sector/1000;

            sector = history.lapHistoryData[i].sector2Time;
            if (sector > 0)
                row["Sector 2"] = sector / 1000;

            if (i < 2) return;

            i = i - 1;
            sector = history.lapHistoryData[i].sector3Time;
            if (sector > 0)
                row["Sector 3"] = sector / 1000;

            sector = history.lapHistoryData[i].lapTime;
            if (sector > 0)
                row["Last Lap"] = sector.ToTelemetryTime();

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
            carDamageMessage.TyreWear = damage.carDamageData[19].tyreWear;
            carDamageMessage.FrontLeftWingDamage = damage.carDamageData[19].frontLeftWingDamage;
            carDamageMessage.FrontRightWingDamage = damage.carDamageData[19].frontRightWingDamage;
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
            carSetupMessage.BrakeBias = setup.carSetupData[19].brakeBias;
            carSetupMessage.FuelLoad = setup.carSetupData[19].fuelLoad;
            RaisePropertyChanged(nameof(BrakeBias));
            RaisePropertyChanged(nameof(FuelLoad));
        }
        #endregion
    }
}

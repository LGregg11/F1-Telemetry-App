﻿namespace F1TelemetryApp.ViewModel
{
    using log4net;
    using Prism.Mvvm;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;

    using F1GameTelemetry.Listener;
    using F1TelemetryApp.Model;
    using F1GameTelemetry.Enums;
    using F1GameTelemetry.Converters;
    using F1GameTelemetry.Readers;
    using System.Windows;
    using F1TelemetryApp.Converters;

    public class MainWindowViewModel : BindableBase
    {
        private const int port = 20777;

        private readonly TelemetryReaderFactory readerFactory;
        private readonly ITelemetryListener telemetryListener;
        private ITelemetryReader telemetryReader;
        private string readerWarningMessage = "No reader specified";
        private Visibility readerWarningMessageVisibility = Visibility.Hidden;

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
        private ReaderVersion readerVersion;
        private int myCarIndex = -1;

        public MainWindowViewModel()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

            PopulateMessages();

            telemetryListener = new TelemetryListener(port);
            readerFactory = new TelemetryReaderFactory(telemetryListener);
            Version = ReaderVersion.F12021;
            telemetryReader = readerFactory.GetTelemetryReader(Version)!;
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
            get => readerVersion;

            set
            {
                if (readerVersion != value)
                {
                    readerVersion = value;
                    SetTelemetryReader(readerVersion);
                    RaisePropertyChanged(nameof(Version));
                }
            }
        }

        public string ReaderWarningMessage
        {
            get => readerWarningMessage;
            set
            {
                if (readerWarningMessage != value)
                {
                    readerWarningMessage = value;
                    RaisePropertyChanged(nameof(ReaderWarningMessage));
                }
            }
        }

        public Visibility ReaderWarningMessageVisibility
        {
            get => readerWarningMessageVisibility;
            set
            {
                if (readerWarningMessageVisibility != value)
                {
                    readerWarningMessageVisibility = value;
                    RaisePropertyChanged(nameof(ReaderWarningMessageVisibility));
                }
            }
        }

        private void SetTelemetryReader(ReaderVersion reader)
        {
            if (telemetryReader != null)
            {
                telemetryReader.HeaderPacket.Received -= OnHeaderReceived;
                telemetryReader.MotionPacket.Received -= OnMotionReceived;
                telemetryReader.CarDamagePacket.Received -= OnCarDamageReceived;
                telemetryReader.CarSetupPacket.Received -= OnCarSetupReceived;
                //_telemetryReader.CarStatusPacket.Received -= OnCarStatusReceived;
                telemetryReader.CarTelemetryPacket.Received -= OnCarTelemetryReceived;
                //_telemetryReader.FinalClassificationPacket.Received -= OnFinalClassificationReceived;
                telemetryReader.LapDataPacket.Received -= OnLapDataReceived;
                telemetryReader.LobbyInfoPacket.Received -= OnLobbyInfoReceived;
                telemetryReader.ParticipantPacket.Received -= OnParticipantReceived;
                telemetryReader.SessionHistoryPacket.Received -= OnSessionHistoryReceived;
                telemetryReader.SessionPacket.Received -= OnSessionReceived;
            }
            telemetryReader = readerFactory.GetTelemetryReader(reader)!;
            if (telemetryReader != null && !telemetryReader.IsSupported)
            {
                // TODO: Create a method that checks if the reader is supported, if it is then enable the start telemetry feed button and hide the warning text!
                var msg = $"{telemetryReader.Name} is not supported";
                Log?.Warn(msg);
                ReaderWarningMessage = msg;
            }

            if (telemetryReader == null)
            {
                var msg = $"{EnumConverter.GetEnumDescription(Version)} reader not found";
                Log?.Warn(msg);
                ReaderWarningMessage = msg;
                ReaderWarningMessageVisibility = Visibility.Visible;
                return;
            }

            ReaderWarningMessageVisibility = Visibility.Hidden;
            telemetryReader.HeaderPacket.Received += OnHeaderReceived;
            telemetryReader.MotionPacket.Received += OnMotionReceived;
            telemetryReader.CarDamagePacket.Received += OnCarDamageReceived;
            telemetryReader.CarSetupPacket.Received += OnCarSetupReceived;
            //_telemetryReader.CarStatusPacket.Received += OnCarStatusReceived;
            telemetryReader.CarTelemetryPacket.Received += OnCarTelemetryReceived;
            //_telemetryReader.FinalClassificationPacket.Received += OnFinalClassificationReceived;
            telemetryReader.LapDataPacket.Received += OnLapDataReceived;
            telemetryReader.LobbyInfoPacket.Received += OnLobbyInfoReceived;
            telemetryReader.ParticipantPacket.Received += OnParticipantReceived;
            telemetryReader.SessionHistoryPacket.Received += OnSessionHistoryReceived;
            telemetryReader.SessionPacket.Received += OnSessionReceived;

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

        public string TrackName => Enum.GetName(typeof(Track), sessionMessage.Track)!;

        public string WeatherStatus => Enum.GetName(typeof(Weather), sessionMessage.Weather)!;

        public string TrackTemperature => $"{Convert.ToInt32(sessionMessage.TrackTemperature)}";

        public string AirTemperature => $"{Convert.ToInt32(sessionMessage.AirTemperature)}";

        public string TotalLaps => $"{sessionMessage.TotalLaps}";

        public string AiDifficulty => $"{Convert.ToUInt32(sessionMessage.AiDifficulty)}";

        public Dictionary<string, string> Participants => participantMessage.Participants;

        public DataTable SessionHistory => sessionHistoryTable;

        public string LobbyPlayers => $"{lobbyInfoMessage.Players}";

        public string LobbyName => lobbyInfoMessage.Name;

        public string LobbyTeam => Enum.GetName(typeof(Team), lobbyInfoMessage.Team)!;

        public string LobbyNationality => Enum.GetName(typeof(Nationality), lobbyInfoMessage.Nationality)!;

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

        #region Event Handlers

        private void OnHeaderReceived(object? sender, EventArgs e)
        {
            var header = ((HeaderEventArgs)e).Header;
            if (myCarIndex < 0)
                myCarIndex = (int)header.playerCarIndex;

            App.Current.Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < HeaderMessages.Count; i++)
                {
                    if (HeaderMessages[i].PacketId == (PacketId)header.packetId)
                    {
                        HeaderMessage headerMessage = HeaderMessages[i];
                        headerMessage.Total++;

                        HeaderMessages[i] = headerMessage;
                        break;
                    }
                }
            });
        }

        private void UpdateEvents(byte[] eventPacket)
        {
            if (telemetryReader == null) return;

            EventType eventType = telemetryReader.GetEventType(eventPacket);
            App.Current.Dispatcher.Invoke(() =>
            {
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
            });
        }

        private void OnMotionReceived(object? sender, EventArgs e)
        {
            var motion = ((MotionEventArgs)e).Motion;
            motionMessage.Speed = Converter.GetMagnitudeFromVectorData(motion.extraCarMotionData.localVelocity);
            RaisePropertyChanged(nameof(LocalSpeed));
        }

        private void OnCarTelemetryReceived(object? sender, EventArgs e)
        {
            var carTelemetry = ((CarTelemetryEventArgs)e).CarTelemetry;
            telemetryMessage.Speed = carTelemetry.carTelemetryData[myCarIndex].speed;
            telemetryMessage.Throttle = carTelemetry.carTelemetryData[myCarIndex].throttle;
            telemetryMessage.Brake = carTelemetry.carTelemetryData[myCarIndex].brake;
            telemetryMessage.Gear = carTelemetry.carTelemetryData[myCarIndex].gear;
            telemetryMessage.Steer = carTelemetry.carTelemetryData[myCarIndex].steer;
            RaisePropertyChanged(nameof(Speed));
            RaisePropertyChanged(nameof(Throttle));
            RaisePropertyChanged(nameof(Brake));
            RaisePropertyChanged(nameof(Gear));
            RaisePropertyChanged(nameof(Steer));
        }

        private void OnLapDataReceived(object? sender, EventArgs e)
        {
            var lapData = ((LapDataEventArgs)e).LapData;
            lapDataMessage.LastLapTime = lapData.carLapData[myCarIndex].lastLapTime;
            RaisePropertyChanged(nameof(LastLapTime));
        }

        private void OnSessionReceived(object? sender, EventArgs e)
        {
            var session = ((SessionEventArgs)e).Session;
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

        private void OnParticipantReceived(object? sender, EventArgs e)
        {
            var participant = ((ParticipantEventArgs)e).Participant;
            var participants = new Dictionary<string, string>();
            foreach (var p in participant.participants)
            {
                if (!string.IsNullOrEmpty(p.name) && !participants.ContainsKey(p.name))
                    participants.Add(p.name, Enum.GetName(typeof(Nationality), p.nationality)!);
            }

            participantMessage.Participants = participants;
            RaisePropertyChanged(nameof(Participants));
        }

        private void OnSessionHistoryReceived(object? sender, EventArgs e)
        {
            var history = ((SessionHistoryEventArgs)e).SessionHistory;
            // TODO: Add converter for sector time like this (00.000)

            var name = ((int)history.carIdx).ToString();
            if (string.IsNullOrEmpty(name))
            {
                Log?.Warn("Name is blank in SessionHistory");
                return;
            }

            if (myCarIndex == (int)history.carIdx)
            {
                name = "your CAR";
            }

            
            DataRow? row = sessionHistoryTable.Select($"Name='{name}'").FirstOrDefault();
            if (row == null)
            {
                row = sessionHistoryTable.NewRow();
                row["LastLap"] = 0f.ToTelemetryTime();
                sessionHistoryTable.Rows.Add(row);
            }

            row["Pos"] = 0;
            row["Name"] = name;
            row["Laps"] = (int)history.numLaps;
            var i = (int)history.numLaps - 1;
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

        private void OnLobbyInfoReceived(object? sender, EventArgs e)
        {
            var info = ((LobbyInfoEventArgs)e).LobbyInfo;
            lobbyInfoMessage.Players = info.numPlayers;
            lobbyInfoMessage.Name = info.lobbyPlayers.FirstOrDefault().name;
            lobbyInfoMessage.Nationality = info.lobbyPlayers.FirstOrDefault().nationality;
            lobbyInfoMessage.Team = info.lobbyPlayers.FirstOrDefault().teamId;
            RaisePropertyChanged(nameof(LobbyPlayers));
            RaisePropertyChanged(nameof(LobbyName));
            RaisePropertyChanged(nameof(LobbyNationality));
            RaisePropertyChanged(nameof(LobbyTeam));
        }

        private void OnCarDamageReceived(object? sender, EventArgs e)
        {
            var damage = ((CarDamageEventArgs)e).CarDamage;
            carDamageMessage.TyreWear = damage.carDamageData[myCarIndex].tyreWear;
            carDamageMessage.FrontLeftWingDamage = damage.carDamageData[myCarIndex].frontLeftWingDamage;
            carDamageMessage.FrontRightWingDamage = damage.carDamageData[myCarIndex].frontRightWingDamage;
            RaisePropertyChanged(nameof(FLTyreWear));
            RaisePropertyChanged(nameof(FRTyreWear));
            RaisePropertyChanged(nameof(RLTyreWear));
            RaisePropertyChanged(nameof(RRTyreWear));
            RaisePropertyChanged(nameof(FrontLeftWingDamage));
            RaisePropertyChanged(nameof(FrontRightWingDamage));
        }

        private void OnCarSetupReceived(object? sender, EventArgs e)
        {
            var setup = ((CarSetupEventArgs)e).CarSetup;
            carSetupMessage.BrakeBias = setup.carSetupData[myCarIndex].brakeBias;
            carSetupMessage.FuelLoad = setup.carSetupData[myCarIndex].fuelLoad;
            RaisePropertyChanged(nameof(BrakeBias));
            RaisePropertyChanged(nameof(FuelLoad));
        }
        #endregion
    }
}

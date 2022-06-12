namespace F1TelemetryApp.ViewModel;

using log4net;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    private ObservableCollection<Driver> drivers = new();
    private ObservableCollection<HeaderMessage> headerMessages;
    private ObservableCollection<EventMessage> eventMessages;
    private MotionMessage motionMessage;
    private TelemetryMessage telemetryMessage;
    private LapDataMessage lapDataMessage;
    private SessionMessage sessionMessage;
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

    public ObservableCollection<Driver> Drivers
    {
        get => drivers;
        set
        {
            if (drivers != value)
            {
                drivers = value;
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
        PopularCarSetupMessage();
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
            myCarIndex = header.playerCarIndex;

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
        App.Current.Dispatcher.Invoke(() =>
        {
            motionMessage.Speed = Converter.GetMagnitudeFromVectorData(motion.extraCarMotionData.localVelocity);
        });
        RaisePropertyChanged(nameof(LocalSpeed));
    }

    private void OnCarTelemetryReceived(object? sender, EventArgs e)
    {
        var carTelemetry = ((CarTelemetryEventArgs)e).CarTelemetry;
        App.Current.Dispatcher.Invoke(() =>
        {
            telemetryMessage.Speed = carTelemetry.carTelemetryData[myCarIndex].speed;
            telemetryMessage.Throttle = carTelemetry.carTelemetryData[myCarIndex].throttle;
            telemetryMessage.Brake = carTelemetry.carTelemetryData[myCarIndex].brake;
            telemetryMessage.Gear = carTelemetry.carTelemetryData[myCarIndex].gear;
            telemetryMessage.Steer = carTelemetry.carTelemetryData[myCarIndex].steer;
        });
        RaisePropertyChanged(nameof(Speed));
        RaisePropertyChanged(nameof(Throttle));
        RaisePropertyChanged(nameof(Brake));
        RaisePropertyChanged(nameof(Gear));
        RaisePropertyChanged(nameof(Steer));
    }

    private void OnLapDataReceived(object? sender, EventArgs e)
    {
        var lapData = ((LapDataEventArgs)e).LapData;
        App.Current.Dispatcher.Invoke(() =>
        {
            for (int i = 0; i < lapData.carLapData.Length; i++)
            {
                Driver driver;
                if (!TryGetDriver(i, out driver!)) continue;

                driver.ApplyCarLapData(lapData.carLapData[i]); // Should change the dictionary driver value too (I think)
                Drivers[i] = driver;
            }
        });
        RaisePropertyChanged(nameof(Drivers));

        App.Current.Dispatcher.Invoke(() =>
        {
            lapDataMessage.LastLapTime = lapData.carLapData[myCarIndex].lastLapTime;
        });
        RaisePropertyChanged(nameof(LastLapTime));
    }

    private void OnSessionReceived(object? sender, EventArgs e)
    {
        var session = ((SessionEventArgs)e).Session;
        App.Current.Dispatcher.Invoke(() =>
        {
            sessionMessage.Track = session.trackId;
            sessionMessage.Weather = session.weather;
            sessionMessage.TrackTemperature = session.trackTemperature;
            sessionMessage.AirTemperature = session.airTemperature;
            sessionMessage.AiDifficulty = session.aiDifficulty;
            sessionMessage.TotalLaps = session.totalLaps;
        });
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
        App.Current.Dispatcher.Invoke(() =>
        {
            int i = 0;
            foreach (var p in participant.participants.Where(p => p.name != string.Empty))
            {
                if (!string.IsNullOrEmpty(p.name) && !participants.ContainsKey(p.name))
                    participants.Add(p.name, Enum.GetName(typeof(Nationality), p.nationality)!);

                // Assume this is the first packet that is received per driver
                // Also Assume this doesn't change during a session
                if (!GetDriverIndexes().Contains(i))
                    Drivers.Add(new Driver(i, p));

                i++;
            }
            participantMessage.Participants = participants;
        });

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

        Driver driver;
        if (!TryGetDriver(history.carIdx, out driver!)) return;

        App.Current.Dispatcher.Invoke(() =>
        {
            driver.ApplySessionHistory(history);
            Drivers[history.carIdx] = driver;
        });
        RaisePropertyChanged(nameof(Drivers));
    }

    private void OnLobbyInfoReceived(object? sender, EventArgs e)
    {
        var info = ((LobbyInfoEventArgs)e).LobbyInfo;
        App.Current.Dispatcher.Invoke(() =>
        {
            lobbyInfoMessage.Players = info.numPlayers;
            lobbyInfoMessage.Name = info.lobbyPlayers.FirstOrDefault().name;
            lobbyInfoMessage.Nationality = info.lobbyPlayers.FirstOrDefault().nationality;
            lobbyInfoMessage.Team = info.lobbyPlayers.FirstOrDefault().teamId;
        });
        RaisePropertyChanged(nameof(LobbyPlayers));
        RaisePropertyChanged(nameof(LobbyName));
        RaisePropertyChanged(nameof(LobbyNationality));
        RaisePropertyChanged(nameof(LobbyTeam));
    }

    private void OnCarDamageReceived(object? sender, EventArgs e)
    {
        var damage = ((CarDamageEventArgs)e).CarDamage;
        App.Current.Dispatcher.Invoke(() =>
        {
            carDamageMessage.TyreWear = damage.carDamageData[myCarIndex].tyreWear;
            carDamageMessage.FrontLeftWingDamage = damage.carDamageData[myCarIndex].frontLeftWingDamage;
            carDamageMessage.FrontRightWingDamage = damage.carDamageData[myCarIndex].frontRightWingDamage;
        });
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
        App.Current.Dispatcher.Invoke(() =>
        {
            carSetupMessage.BrakeBias = setup.carSetupData[myCarIndex].brakeBias;
            carSetupMessage.FuelLoad = setup.carSetupData[myCarIndex].fuelLoad;
        });
        RaisePropertyChanged(nameof(BrakeBias));
        RaisePropertyChanged(nameof(FuelLoad));
    }
    #endregion

    private int[] GetDriverIndexes() => Drivers.Select(d => d.Index).ToArray();

    private bool TryGetDriver(int index, out Driver? driver)
    {
        driver = Drivers.FirstOrDefault(d => d.Index == index);
        if (driver == null)
            return false;
        return true;
    }
}

namespace F1TelemetryApp.ViewModel;

using Model;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Enums;

using log4net;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using F1GameTelemetry.Readers;
using F1GameTelemetry.Events;
using F1GameTelemetry.Models;

public class TestWindowViewModel : BasePageViewModel
{
    private int _myCarIndex = -1; // Not really but for example sake this is fine
    private MotionMessage _motionMessage;
    private TelemetryMessage _telemetryMessage;
    private LapDataMessage _lapDataMessage;
    private SessionMessage _sessionMessage;
    private ParticipantMessage _participantMessage;
    private LobbyInfoMessage _lobbyInfoMessage;
    private CarDamageMessage _carDamageMessage;
    private CarSetupMessage _carSetupMessage;

    public TestWindowViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        PopulateMessages();
    }

    private ObservableCollection<HeaderMessage> headerMessages;
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


    private ObservableCollection<EventMessage> eventMessages;
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

    public override void SetTelemetryReader()
    {
        SingletonTelemetryReader.MotionReceived += OnMotionReceived;
        SingletonTelemetryReader.CarDamageReceived += OnCarDamageReceived;
        SingletonTelemetryReader.CarTelemetryReceived += OnCarTelemetryReceived;
        SingletonTelemetryReader.LobbyInfoReceived += OnLobbyInfoReceived;
        SingletonTelemetryReader.SessionReceived += OnSessionReceived;
    }

    #region Gui Test Properties

    public string LocalSpeed => $"{_motionMessage.Speed:#0.00} m/s";

    public string Speed => $"{_telemetryMessage.Speed} km/h";

    public string Throttle => $"{_telemetryMessage.Throttle}";

    public string Brake => $"{_telemetryMessage.Brake}";

    public string Gear => $"{_telemetryMessage.Gear}";

    public string Steer => $"{_telemetryMessage.Steer}";

    public string LastLapTime => _lapDataMessage.LastLapTime.ToTelemetryTime();

    public string TrackName => Enum.GetName(typeof(Track), _sessionMessage.Track)!;

    public string WeatherStatus => Enum.GetName(typeof(Weather), _sessionMessage.Weather)!;

    public string TrackTemperature => $"{Convert.ToInt32(_sessionMessage.TrackTemperature)}";

    public string AirTemperature => $"{Convert.ToInt32(_sessionMessage.AirTemperature)}";

    public string TotalLaps => $"{_sessionMessage.TotalLaps}";

    public string AiDifficulty => $"{Convert.ToUInt32(_sessionMessage.AiDifficulty)}";

    public Dictionary<string, string> Participants => _participantMessage.Participants;

    public string LobbyPlayers => $"{_lobbyInfoMessage.Players}";

    public string LobbyName => _lobbyInfoMessage.Name;

    public string LobbyTeam => Enum.GetName(typeof(Team), _lobbyInfoMessage.Team)!;

    public string LobbyNationality => Enum.GetName(typeof(Nationality), _lobbyInfoMessage.Nationality)!;

    public string RLTyreWear => $"{_carDamageMessage.TyreWear.rearLeft:F1}%";

    public string RRTyreWear => $"{_carDamageMessage.TyreWear.rearRight:F1}%";

    public string FLTyreWear => $"{_carDamageMessage.TyreWear.frontLeft:F1}%";

    public string FRTyreWear => $"{_carDamageMessage.TyreWear.frontRight:F1}%";

    public string FrontLeftWingDamage => $"{_carDamageMessage.FrontLeftWingDamage:F1}%";

    public string FrontRightWingDamage => $"{_carDamageMessage.FrontRightWingDamage:F1}%";

    public string BrakeBias => $"{_carSetupMessage.BrakeBias}";

    public string FuelLoad => $"{_carSetupMessage.FuelLoad:F2}";

    #endregion

    #region Populators

    private void PopulateMessages()
    {
        PopulateHeaderMessages();
        PopulateEventMessages();

        _motionMessage = new MotionMessage { Speed = 0.0d };
        _telemetryMessage = new TelemetryMessage { Speed = 0, Brake = 0.0f, Throttle = 0.0f, Gear = 0, Steer = 0.0f };
        _lapDataMessage = new LapDataMessage { LastLapTime = 0 };
        _sessionMessage = new SessionMessage { Track = Track.Unknown, Weather = Weather.Unknown, TotalLaps = 0, TrackTemperature = 0, AirTemperature = 0, AiDifficulty = 0 };
        _participantMessage = new ParticipantMessage { Participants = new Dictionary<string, string>() };
        _lobbyInfoMessage = new LobbyInfoMessage { Players = 0, Name = "", Nationality = Nationality.Unknown, Team = Team.Unknown };
        _carDamageMessage = new CarDamageMessage { TyreWear = new FourAxleFloat(0f, 0f, 0f, 0f), FrontLeftWingDamage = 0, FrontRightWingDamage = 0 };
        _carSetupMessage = new CarSetupMessage { BrakeBias = 0, FuelLoad = 0f };
        
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
    #endregion

    #region Event Handlers

    //private void UpdateEvents(byte[] eventPacket)
    //{
    //    var telemetryReader = MainWindowViewModel.TelemetryReader;
    //    if (telemetryReader == null) return;

    //    EventType eventType = SingletonTelemetryReader.GetEventType(eventPacket);
    //    App.Current.Dispatcher.Invoke(() =>
    //    {
    //        for (int i = 0; i < EventMessages.Count; i++)
    //        {
    //            if (EventMessages[i].EventType == eventType)
    //            {
    //                EventMessage eventMessage = EventMessages[i];
    //                eventMessage.Total++;

    //                EventMessages[i] = eventMessage;
    //                break;
    //            }
    //        }
    //    });
    //}

    private void OnMotionReceived(object? sender, PacketEventArgs<Motion> e)
    {
        var header = e.Header;
        if (_myCarIndex < 0)
            _myCarIndex = header.playerCarIndex;

        var motion = e.Packet;
        App.Current.Dispatcher.Invoke(() =>
        {
            _motionMessage.Speed = Converter.GetMagnitudeFromVectorData(motion.carMotionData[_myCarIndex].worldVelocity);
        });
        RaisePropertyChanged(nameof(LocalSpeed));
    }

    private void OnCarTelemetryReceived(object? sender, PacketEventArgs<CarTelemetry> e)
    {
        var carTelemetry = e.Packet;
        App.Current.Dispatcher.Invoke(() =>
        {
            _telemetryMessage.Speed = carTelemetry.carTelemetryData[_myCarIndex].speed;
            _telemetryMessage.Throttle = carTelemetry.carTelemetryData[_myCarIndex].throttle;
            _telemetryMessage.Brake = carTelemetry.carTelemetryData[_myCarIndex].brake;
            _telemetryMessage.Gear = carTelemetry.carTelemetryData[_myCarIndex].gear;
            _telemetryMessage.Steer = carTelemetry.carTelemetryData[_myCarIndex].steer;
        });
        RaisePropertyChanged(nameof(Speed));
        RaisePropertyChanged(nameof(Throttle));
        RaisePropertyChanged(nameof(Brake));
        RaisePropertyChanged(nameof(Gear));
        RaisePropertyChanged(nameof(Steer));
    }

    private void OnSessionReceived(object? sender, PacketEventArgs<Session> e)
    {
        var header = e.Header;
        if (_myCarIndex < 0)
            _myCarIndex = header.playerCarIndex;

        var session = e.Packet;
        App.Current.Dispatcher.Invoke(() =>
        {
            _sessionMessage.Track = session.trackId;
            _sessionMessage.Weather = session.weather;
            _sessionMessage.TrackTemperature = session.trackTemperature;
            _sessionMessage.AirTemperature = session.airTemperature;
            _sessionMessage.TotalLaps = session.totalLaps;
        });
        RaisePropertyChanged(nameof(TrackName));
        RaisePropertyChanged(nameof(WeatherStatus));
        RaisePropertyChanged(nameof(TotalLaps));
        RaisePropertyChanged(nameof(TrackTemperature));
        RaisePropertyChanged(nameof(AirTemperature));
        RaisePropertyChanged(nameof(AiDifficulty));
    }

    private void OnLobbyInfoReceived(object? sender, PacketEventArgs<LobbyInfo> e)
    {
        var header = e.Header;
        if (_myCarIndex < 0)
            _myCarIndex = header.playerCarIndex;

        var info = e.Packet;
        App.Current.Dispatcher.Invoke(() =>
        {
            _lobbyInfoMessage.Players = info.numPlayers;
            _lobbyInfoMessage.Name = info.lobbyPlayers.FirstOrDefault().name;
            _lobbyInfoMessage.Nationality = info.lobbyPlayers.FirstOrDefault().nationality;
            _lobbyInfoMessage.Team = info.lobbyPlayers.FirstOrDefault().teamId;
        });
        RaisePropertyChanged(nameof(LobbyPlayers));
        RaisePropertyChanged(nameof(LobbyName));
        RaisePropertyChanged(nameof(LobbyNationality));
        RaisePropertyChanged(nameof(LobbyTeam));
    }

    private void OnCarDamageReceived(object? sender, PacketEventArgs<CarDamage> e)
    {
        var header = e.Header;
        if (_myCarIndex < 0)
            _myCarIndex = header.playerCarIndex;

        var damage = e.Packet;
        App.Current.Dispatcher.Invoke(() =>
        {
            _carDamageMessage.TyreWear = damage.carDamageData[_myCarIndex].tyreWear;
            _carDamageMessage.FrontLeftWingDamage = damage.carDamageData[_myCarIndex].frontLeftWingDamage;
            _carDamageMessage.FrontRightWingDamage = damage.carDamageData[_myCarIndex].frontRightWingDamage;
        });
        RaisePropertyChanged(nameof(FLTyreWear));
        RaisePropertyChanged(nameof(FRTyreWear));
        RaisePropertyChanged(nameof(RLTyreWear));
        RaisePropertyChanged(nameof(RRTyreWear));
        RaisePropertyChanged(nameof(FrontLeftWingDamage));
        RaisePropertyChanged(nameof(FrontRightWingDamage));
    }
    #endregion
}

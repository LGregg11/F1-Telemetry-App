namespace F1GameTelemetry.Converters;

using Converters;
using Enums;
using Models;
using Packet = Packets.F12022;

using System;


public class TelemetryConverter2022 : BaseTelemetryConverter
{
    private const short _WEATHER_FORECAST_SAMPLES_MAX = 56;
    private const short _LAP_HISTORY_MAX = 100;
    private const short _TYRE_STINT_HISTORY_MAX = 8;

    public TelemetryConverter2022() : base()
    {
    }

    public override string Name => "F1 2022 Telemetry Converter";
    public override GameVersion GameVersion => GameVersion.F12022;
    public override int PacketHeaderSize => 24;
    public override Header ConvertBytesToHeader(byte[] bytes)
    {
        Packet.Header packet = Converter.BytesToPacket<Packet.Header>(bytes);
        return new Header(
            packet.packetId,
            packet.sessionUID,
            packet.sessionTime,
            packet.frameIdentifier,
            packet.playerCarIndex
            );
    }

    public override object? ConvertBytesToStandardPacket(PacketId packetType, byte[] bytes)
    {
        return packetType switch
        {
            PacketId.Motion => ConvertBytesToMotionPacket(bytes),
            PacketId.Session => ConvertBytesToSessionPacket(bytes),
            PacketId.LapData => ConvertBytesToLapDataPacket(bytes),
            PacketId.Event => null,
            PacketId.Participant => ConvertBytesToParticipantPacket(bytes),
            PacketId.CarSetup => null,
            PacketId.CarTelemetry => ConvertBytesToCarTelemetryPacket(bytes),
            PacketId.CarStatus => ConvertBytesToCarStatusPacket(bytes),
            PacketId.FinalClassification => ConvertBytesToFinalClassificationPacket(bytes),
            PacketId.LobbyInfo => ConvertBytesToLobbyInfoPacket(bytes),
            PacketId.CarDamage => ConvertBytesToCarDamagePacket(bytes),
            PacketId.SessionHistory => ConvertBytesToSessionHistoryPacket(bytes),
            _ => throw new NotImplementedException($"Unknown PacketId: {Enum.GetName(packetType)}")
        };
    }

    Motion ConvertBytesToMotionPacket(byte[] bytes)
    {
        Packet.Motion packet = Converter.BytesToPacket<Packet.Motion>(bytes);

        CarMotionData[] carMotionDatas = new CarMotionData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            carMotionDatas[i] = new CarMotionData(
                new Vector3d(packet.carMotionData[i].worldPosition),
                new Vector3d(packet.carMotionData[i].worldVelocity)
                );
        }

        return new Motion(carMotionDatas);
    }

    Session ConvertBytesToSessionPacket(byte[] bytes)
    {
        Packet.Session packet = Converter.BytesToPacket<Packet.Session>(bytes);
        WeatherForecastSample[] weatherForecastSamples = new WeatherForecastSample[_WEATHER_FORECAST_SAMPLES_MAX];

        for (int i = 0; i < _WEATHER_FORECAST_SAMPLES_MAX; i++)
        {
            var sample = packet.weatherForecastSamples[i];

            weatherForecastSamples[i] = new WeatherForecastSample(
                sample.sessionType,
                sample.timeOffset,
                sample.weather,
                sample.trackTemperature,
                sample.rainPercentage
                );
        }

        return new Session(
            packet.weather,
            packet.trackTemperature,
            packet.airTemperature,
            packet.totalLaps,
            packet.trackLength,
            packet.sessionType,
            packet.trackId,
            packet.sessionDuration,
            packet.safetyCarStatus,
            packet.numWeatherForecastSamples,
            weatherForecastSamples,
            packet.pitStopWindowIdealLap,
            packet.pitStopWindowLatestLap,
            packet.pitStopRejoinPosition
            );
    }

    LapData ConvertBytesToLapDataPacket(byte[] bytes)
    {
        Packet.LapData packet = Converter.BytesToPacket<Packet.LapData>(bytes);

        CarLapData[] carLapDatas = new CarLapData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            var data = packet.carLapData[i];

            carLapDatas[i] = new CarLapData(
                data.lastLapTime,
                data.currentLapTime,
                data.sector1Time,
                data.sector2Time,
                data.lapDistance,
                data.carPosition,
                data.currentLapNum,
                data.sector,
                data.currentLapInvalid,
                data.resultStatus
                );
        }

        return new LapData(carLapDatas);
    }

    Participant ConvertBytesToParticipantPacket(byte[] bytes)
    {
        Packet.Participant packet = Converter.BytesToPacket<Packet.Participant>(bytes);

        ParticipantData[] participants = new ParticipantData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            var participant = packet.participants[i];

            participants[i] = new ParticipantData(
                participant.driverId,
                participant.teamId,
                participant.raceNumber,
                participant.nationality,
                participant.name
                );
        }

        return new Participant(packet.numActiveCars, participants);
    }

    CarTelemetry ConvertBytesToCarTelemetryPacket(byte[] bytes)
    {
        Packet.CarTelemetry packet = Converter.BytesToPacket<Packet.CarTelemetry>(bytes);

        CarTelemetryData[] carTelemetryData = new CarTelemetryData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            var data = packet.carTelemetryData[i];

            carTelemetryData[i] = new CarTelemetryData(
                data.speed,
                data.throttle,
                data.steer,
                data.brake,
                data.gear,
                data.drs,
                new FourAxleUnsignedShort(data.brakesTemperature),
                new FourAxleByte(data.tyresSurfaceTemperature),
                new FourAxleByte(data.tyresInnerTemperature)
                );
        }

        return new CarTelemetry(carTelemetryData);
    }

    CarStatus ConvertBytesToCarStatusPacket(byte[] bytes)
    {
        Packet.CarStatus packet = Converter.BytesToPacket<Packet.CarStatus>(bytes);

        CarStatusData[] carStatusData = new CarStatusData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            var data = packet.carStatusData[i];

            carStatusData[i] = new CarStatusData(
                data.fuelRemainingLaps,
                data.visualTyreCompound,
                data.tyresAgeLaps
                );
        }

        return new CarStatus(carStatusData);
    }

    FinalClassification ConvertBytesToFinalClassificationPacket(byte[] bytes)
    {
        Packet.FinalClassification packet = Converter.BytesToPacket<Packet.FinalClassification>(bytes);

        FinalClassificationData[] finalClassificationData = new FinalClassificationData[packet.numberCars];
        for (int i = 0; i < packet.numberCars; i++)
        {
            var data = packet.finalClassificationData[i];

            finalClassificationData[i] = new FinalClassificationData(
                data.position,
                data.numberLaps,
                data.gridPosition,
                data.points,
                data.numberPitStops,
                data.resultStatus,
                data.bestLapTime,
                data.totalRaceTime,
                data.penalitesTime,
                data.numberPenalties,
                data.numberTyreStints,
                data.tyreStintsVisual
                );
        }

        return new FinalClassification(packet.numberCars, finalClassificationData);
    }

    LobbyInfo ConvertBytesToLobbyInfoPacket(byte[] bytes)
    {
        Packet.LobbyInfo packet = Converter.BytesToPacket<Packet.LobbyInfo>(bytes);

        LobbyInfoData[] lobbyInfoData = new LobbyInfoData[packet.numPlayers];
        for (int i = 0; i < packet.numPlayers; i++)
        {
            var player = packet.lobbyPlayers[i];

            lobbyInfoData[i] = new LobbyInfoData(
                player.aiControlled,
                player.teamId,
                player.nationality,
                player.name);
        }

        return new LobbyInfo(packet.numPlayers, lobbyInfoData);
    }

    CarDamage ConvertBytesToCarDamagePacket(byte[] bytes)
    {
        Packet.CarDamage packet = Converter.BytesToPacket<Packet.CarDamage>(bytes);

        CarDamageData[] carDamageData = new CarDamageData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            var data = packet.carDamageData[i];

            carDamageData[i] = new CarDamageData(
                new FourAxleFloat(data.tyreWear),
                new FourAxleByte(data.tyreDamage),
                new FourAxleByte(data.brakeDamage),
                data.frontLeftWingDamage,
                data.frontRightWingDamage,
                data.rearWingDamage);
        }

        return new CarDamage(carDamageData);
    }

    SessionHistory ConvertBytesToSessionHistoryPacket(byte[] bytes)
    {
        Packet.SessionHistory packet = Converter.BytesToPacket<Packet.SessionHistory>(bytes);

        LapHistoryData[] lapHistoryData = new LapHistoryData[_LAP_HISTORY_MAX];
        TyreStintHistoryData[] tyreStintHistoryData = new TyreStintHistoryData[_TYRE_STINT_HISTORY_MAX];

        for (int i = 0; i < _LAP_HISTORY_MAX; i++)
        {
            if (i < _TYRE_STINT_HISTORY_MAX)
            {
                tyreStintHistoryData[i] = new TyreStintHistoryData(
                    packet.tyreStintHistoryData[i].endLap,
                    packet.tyreStintHistoryData[i].tyreVisualCompound
                    );
            }

            lapHistoryData[i] = new LapHistoryData(
                packet.lapHistoryData[i].lapTime,
                packet.lapHistoryData[i].sector1Time,
                packet.lapHistoryData[i].sector2Time,
                packet.lapHistoryData[i].sector3Time
                );
        }

        return new SessionHistory(
            packet.carIdx,
            packet.numLaps,
            packet.numTyreStints,
            packet.bestLapTimeLapNum,
            packet.bestSector1LapNum,
            packet.bestSector2LapNum,
            packet.bestSector3LapNum,
            lapHistoryData,
            tyreStintHistoryData
            );
    }

    public object? GetEvent(byte[] remainingPacket)
    {
        EventType eventType = GetEventType(remainingPacket);
        return eventType switch
        {
            EventType.BUTN => Converter.BytesToPacket<Packet.Buttons>(remainingPacket),
            EventType.FLBK => Converter.BytesToPacket<Packet.Flashback>(remainingPacket),
            EventType.STLG => Converter.BytesToPacket<Packet.StartLights>(remainingPacket),
            EventType.LGOT => Converter.BytesToPacket<Packet.StartLights>(remainingPacket),
            EventType.SPTP => Converter.BytesToPacket<Packet.SpeedTrap>(remainingPacket),
            EventType.FTLP => Converter.BytesToPacket<Packet.FastestLap>(remainingPacket),
            EventType.TMPT => Converter.BytesToPacket<Packet.TeamMateInPits>(remainingPacket),
            EventType.RCWN => Converter.BytesToPacket<Packet.RaceWinner>(remainingPacket),
            EventType.RTMT => Converter.BytesToPacket<Packet.Retirement>(remainingPacket),
            EventType.PENA => Converter.BytesToPacket<Packet.Penalty>(remainingPacket),
            EventType.SGSV => Converter.BytesToPacket<Packet.StopGoPenaltyServed>(remainingPacket),
            EventType.DTSV => Converter.BytesToPacket<Packet.DriveThroughPenaltyServed>(remainingPacket),
            _ => null,
        };
    }
}

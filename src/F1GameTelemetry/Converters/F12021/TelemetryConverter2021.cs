namespace F1GameTelemetry.Converters.F12021;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Enums;
using F1GameTelemetry.Packets.F12021;

using System.Linq;
using System;

public class TelemetryConverter2021 : BaseTelemetryConverter
{
    private const short MARSHAL_ZONES_MAX = 21;
    private const short WEATHER_FORECAST_SAMPLES_MAX = 56;
    private const short LAP_HISTORY_MAX = 100;
    private const short TYRE_STINT_HISTORY_MAX = 8;

    public TelemetryConverter2021() : base()
    {
    }

    public override string Name => "F1 2021 Telemetry Reader";
    public override bool IsSupported => true;
    public override GameVersion GameVersion => GameVersion.F12021;
    public override Packets.Standard.Header ConvertBytesToHeader(byte[] bytes)
    {
        Header packet = Converter.BytesToPacket<Header>(bytes);
        return new Packets.Standard.Header(
            packet.packetFormat,
            packet.gameMajorVersion,
            packet.gameMinorVersion,
            packet.packetVersion,
            packet.packetId,
            packet.sessionUID,
            packet.sessionTime,
            packet.frameIdentifier,
            packet.playerCarIndex,
            packet.secondaryPlayerCarIndex
        );
    }

    public override object? ConvertBytesToStandardPacket(PacketId packetType, byte[] bytes)
    {
        byte[] remainingPacket = bytes.Skip(HeaderPacketSize).ToArray();
        return packetType switch
        {
            PacketId.Motion => ConvertBytesToMotionPacket(remainingPacket),
            PacketId.Session => ConvertBytesToSessionPacket(remainingPacket),
            PacketId.LapData => ConvertBytesToLapDataPacket(remainingPacket),
            //PacketId.Event
            PacketId.Participant => ConvertBytesToParticipantPacket(remainingPacket),
            PacketId.CarSetup => ConvertBytesToCarSetupPacket(remainingPacket),
            PacketId.CarTelemetry => ConvertBytesToCarTelemetryPacket(remainingPacket),
            PacketId.CarStatus => ConvertBytesToCarStatusPacket(remainingPacket),
            PacketId.FinalClassification => ConvertBytesToFinalClassificationPacket(remainingPacket),
            PacketId.LobbyInfo => ConvertBytesToLobbyInfoPacket(remainingPacket),
            PacketId.CarDamage => ConvertBytesToCarDamagePacket(remainingPacket),
            PacketId.SessionHistory => ConvertBytesToSessionHistoryPacket(remainingPacket),
            PacketId.Event => null,
            _ => throw new NotImplementedException($"Unknown PacketId: {Enum.GetName(packetType)}")
        };
    }

    Packets.Standard.Motion ConvertBytesToMotionPacket(byte[] bytes)
    {
        Motion packet = Converter.BytesToPacket<Motion>(bytes);

        Packets.Standard.CarMotionData[] carMotionDatas = new Packets.Standard.CarMotionData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            carMotionDatas[i] = new Packets.Standard.CarMotionData(
                packet.carMotionData[i].worldPosition,
                packet.carMotionData[i].worldVelocity,
                packet.carMotionData[i].worldForwardDir,
                packet.carMotionData[i].worldRightDir,
                packet.carMotionData[i].gForce,
                packet.carMotionData[i].rotation
                );
        }

        return new Packets.Standard.Motion(
            carMotionDatas,
            new Packets.Standard.ExtraCarMotionData(
                packet.extraCarMotionData.suspensionPosition,
                packet.extraCarMotionData.suspensionVelocity,
                packet.extraCarMotionData.suspensionAcceleration,
                packet.extraCarMotionData.wheelSpeed,
                packet.extraCarMotionData.wheelSlip,
                packet.extraCarMotionData.localVelocity,
                packet.extraCarMotionData.angularVelocity,
                packet.extraCarMotionData.angularAcceleration)
        );
    }

    Packets.Standard.Session ConvertBytesToSessionPacket(byte[] bytes)
    {
        Session packet = Converter.BytesToPacket<Session>(bytes);

        Packets.Standard.MarshalZones[] marshalZones = new Packets.Standard.MarshalZones[MARSHAL_ZONES_MAX];
        Packets.Standard.WeatherForecastSample[] weatherForecastSamples = new Packets.Standard.WeatherForecastSample[WEATHER_FORECAST_SAMPLES_MAX];

        for (int i = 0; i < WEATHER_FORECAST_SAMPLES_MAX; i++)
        {
            if (i < MARSHAL_ZONES_MAX)
            {
                marshalZones[i] = new Packets.Standard.MarshalZones(
                    packet.marshalZones[i].zoneStart,
                    packet.marshalZones[i].zoneFlag);
            }

            weatherForecastSamples[i] = new Packets.Standard.WeatherForecastSample(
                packet.weatherForecastSamples[i].sessionType,
                packet.weatherForecastSamples[i].timeOffset,
                packet.weatherForecastSamples[i].weather,
                packet.weatherForecastSamples[i].trackTemperature,
                packet.weatherForecastSamples[i].trackTemperatureChange,
                packet.weatherForecastSamples[i].airTemperature,
                packet.weatherForecastSamples[i].airTemperatureChange,
                packet.weatherForecastSamples[i].rainPercentage);
        }

        return new Packets.Standard.Session(
            packet.weather,
            packet.trackTemperature,
            packet.airTemperature,
            packet.totalLaps,
            packet.trackLength,
            packet.sessionType,
            packet.trackId,
            packet.formula,
            packet.sessionTimeLeft,
            packet.sessionDuration,
            packet.pitSpeedLimit,
            packet.gamePaused,
            packet.isSpectating,
            packet.spectatorCarIndex,
            packet.sliPriNativeSupport,
            packet.numMarshalZones,
            marshalZones,
            packet.safetyCarStatus,
            packet.networkGame,
            packet.numWeatherForecastSamples,
            weatherForecastSamples,
            packet.forecastAccuracy,
            packet.aiDifficulty,
            packet.seasonLinkIdentifier,
            packet.weekendLinkIdentifier,
            packet.sessionLinkIdentifier,
            packet.pitStopWindowIdealLap,
            packet.pitStopWindowLatestLap,
            packet.pitStopRejoinPosition,
            packet.steeringAssist,
            packet.brakingAssist,
            packet.gearboxAssist,
            packet.pitAssist,
            packet.pitReleaseAssist,
            packet.ERSAssist,
            packet.DRSAssist,
            packet.dynamicRacingLine,
            packet.dynamicRacingLineType);
    }

    Packets.Standard.LapData ConvertBytesToLapDataPacket(byte[] bytes)
    {
        LapData packet = Converter.BytesToPacket<LapData>(bytes);

        Packets.Standard.CarLapData[] carLapDatas = new Packets.Standard.CarLapData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            carLapDatas[i] = new Packets.Standard.CarLapData(
                packet.carLapData[i].lastLapTime,
                packet.carLapData[i].currentLapTime,
                packet.carLapData[i].sector1Time,
                packet.carLapData[i].sector2Time,
                packet.carLapData[i].lapDistance,
                packet.carLapData[i].totalDistance,
                packet.carLapData[i].safetyCarDelta,
                packet.carLapData[i].carPosition,
                packet.carLapData[i].currentLapNum,
                packet.carLapData[i].pitStatus,
                packet.carLapData[i].numPitStops,
                packet.carLapData[i].sector,
                packet.carLapData[i].currentLapInvalid,
                packet.carLapData[i].penalties,
                packet.carLapData[i].warnings,
                packet.carLapData[i].numUnservedDriveThroughPenalties,
                packet.carLapData[i].numUnservedStopGoPenalties,
                packet.carLapData[i].gridPosition,
                packet.carLapData[i].driverStatus,
                packet.carLapData[i].resultStatus,
                packet.carLapData[i].pitLaneTimerActive,
                packet.carLapData[i].pitLaneTimeInLane,
                packet.carLapData[i].pitStopTimer,
                packet.carLapData[i].pitStopShouldServePen);
        }

        return new Packets.Standard.LapData(carLapDatas);
    }

    // Event ConvertBytesToEventPacket(byte[] bytes)
    //{
    //}

    Packets.Standard.Participant ConvertBytesToParticipantPacket(byte[] bytes)
    {
        Participant packet = Converter.BytesToPacket<Participant>(bytes);

        Packets.Standard.ParticipantData[] participants = new Packets.Standard.ParticipantData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            participants[i] = new Packets.Standard.ParticipantData(
                packet.participants[i].aiControlled,
                packet.participants[i].driverId,
                packet.participants[i].networkId,
                packet.participants[i].teamId,
                packet.participants[i].myTeam,
                packet.participants[i].raceNumber,
                packet.participants[i].nationality,
                packet.participants[i].name,
                packet.participants[i].yourTelemetry);
        }

        return new Packets.Standard.Participant(packet.numActiveCars, participants);
    }

    Packets.Standard.CarSetup ConvertBytesToCarSetupPacket(byte[] bytes)
    {
        CarSetup packet = Converter.BytesToPacket<CarSetup>(bytes);

        Packets.Standard.CarSetupData[] carSetups = new Packets.Standard.CarSetupData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            carSetups[i] = new Packets.Standard.CarSetupData(
                packet.carSetupData[i].frontWing,
                packet.carSetupData[i].rearWing,
                packet.carSetupData[i].onThrottle,
                packet.carSetupData[i].offThrottle,
                packet.carSetupData[i].frontCamber,
                packet.carSetupData[i].rearCamber,
                packet.carSetupData[i].frontToe,
                packet.carSetupData[i].rearToe,
                packet.carSetupData[i].frontSuspension,
                packet.carSetupData[i].rearSuspension,
                packet.carSetupData[i].frontAntiRollBar,
                packet.carSetupData[i].rearAntiRollBar,
                packet.carSetupData[i].frontSuspensionHeight,
                packet.carSetupData[i].rearSuspensionHeight,
                packet.carSetupData[i].brakePressure,
                packet.carSetupData[i].brakeBias,
                packet.carSetupData[i].rearLeftTyrePressure,
                packet.carSetupData[i].rearRightTyrePressure,
                packet.carSetupData[i].frontLeftTyrePressure,
                packet.carSetupData[i].frontRightTyrePressure,
                packet.carSetupData[i].ballast,
                packet.carSetupData[i].fuelLoad);
        }

        return new Packets.Standard.CarSetup(carSetups);
    }

    Packets.Standard.CarTelemetry ConvertBytesToCarTelemetryPacket(byte[] bytes)
    {
        CarTelemetry packet = Converter.BytesToPacket<CarTelemetry>(bytes);

        Packets.Standard.CarTelemetryData[] carTelemetryData = new Packets.Standard.CarTelemetryData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            carTelemetryData[i] = new Packets.Standard.CarTelemetryData(
                packet.carTelemetryData[i].speed,
                packet.carTelemetryData[i].throttle,
                packet.carTelemetryData[i].steer,
                packet.carTelemetryData[i].brake,
                packet.carTelemetryData[i].clutch,
                packet.carTelemetryData[i].gear,
                packet.carTelemetryData[i].engineRPM,
                packet.carTelemetryData[i].drs,
                packet.carTelemetryData[i].revLightsPercent,
                packet.carTelemetryData[i].revLightsBitValue,
                packet.carTelemetryData[i].brakesTemperature,
                packet.carTelemetryData[i].tyresSurfaceTemperature,
                packet.carTelemetryData[i].tyresInnerTemperature,
                packet.carTelemetryData[i].engineTemperature,
                packet.carTelemetryData[i].tyrePressure,
                packet.carTelemetryData[i].surfaceType);
        }

        return new Packets.Standard.CarTelemetry(
            carTelemetryData,
            packet.mfdPanelIndex,
            packet.mfdPanelIndexSecondaryPlayer,
            packet.suggestedGear);
    }

    Packets.Standard.CarStatus ConvertBytesToCarStatusPacket(byte[] bytes)
    {
        CarStatus packet = Converter.BytesToPacket<CarStatus>(bytes);

        Packets.Standard.CarStatusData[] carStatusData = new Packets.Standard.CarStatusData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            carStatusData[i] = new Packets.Standard.CarStatusData(
                packet.carStatusData[i].trackionControl,
                packet.carStatusData[i].antiLockBrakes,
                packet.carStatusData[i].fuelMix,
                packet.carStatusData[i].frontBrakeBias,
                packet.carStatusData[i].pitLimiterStatus,
                packet.carStatusData[i].fuelInTank,
                packet.carStatusData[i].fuelCapacity,
                packet.carStatusData[i].fuelRemainingLaps,
                packet.carStatusData[i].maxRPM,
                packet.carStatusData[i].idleRPM,
                packet.carStatusData[i].maxGears,
                packet.carStatusData[i].drsAllowed,
                packet.carStatusData[i].drsActivationDistance,
                packet.carStatusData[i].actualTyreCompound,
                packet.carStatusData[i].visualTyreCompound,
                packet.carStatusData[i].tyresAgeLaps,
                packet.carStatusData[i].vehicleFiaFlags,
                packet.carStatusData[i].ersStoreEnergy,
                packet.carStatusData[i].ersDeployMode,
                packet.carStatusData[i].ersHarvestedThisLapMGUK,
                packet.carStatusData[i].ersHarvestedThisLapMGUH,
                packet.carStatusData[i].ersDeployedThisLap,
                packet.carStatusData[i].networkPaused
                );
        }

        return new Packets.Standard.CarStatus(carStatusData);
    }

    Packets.Standard.FinalClassification ConvertBytesToFinalClassificationPacket(byte[] bytes)
    {
        FinalClassification packet = Converter.BytesToPacket<FinalClassification>(bytes);

        Packets.Standard.FinalClassificationData[] finalClassificationData = new Packets.Standard.FinalClassificationData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            finalClassificationData[i] = new Packets.Standard.FinalClassificationData(
                packet.finalClassificationData[i].position,
                packet.finalClassificationData[i].numberLaps,
                packet.finalClassificationData[i].gridPosition,
                packet.finalClassificationData[i].points,
                packet.finalClassificationData[i].numberPitStops,
                packet.finalClassificationData[i].resultStatus,
                packet.finalClassificationData[i].bestLapTime,
                packet.finalClassificationData[i].totalRaceTime,
                packet.finalClassificationData[i].penalitesTime,
                packet.finalClassificationData[i].numberPenalties,
                packet.finalClassificationData[i].numberTyreStints,
                packet.finalClassificationData[i].tyreStintsActual,
                packet.finalClassificationData[i].tyreStintsVisual);
        }

        return new Packets.Standard.FinalClassification(packet.numberCars, finalClassificationData);
    }

    Packets.Standard.LobbyInfo ConvertBytesToLobbyInfoPacket(byte[] bytes)
    {
        LobbyInfo packet = Converter.BytesToPacket<LobbyInfo>(bytes);

        Packets.Standard.LobbyInfoData[] lobbyInfoData = new Packets.Standard.LobbyInfoData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            lobbyInfoData[i] = new Packets.Standard.LobbyInfoData(
                packet.lobbyPlayers[i].aiControlled,
                packet.lobbyPlayers[i].teamId,
                packet.lobbyPlayers[i].nationality,
                packet.lobbyPlayers[i].name,
                packet.lobbyPlayers[i].carNumber,
                packet.lobbyPlayers[i].readyStatus);
        }

        return new Packets.Standard.LobbyInfo(packet.numPlayers, lobbyInfoData);
    }

    Packets.Standard.CarDamage ConvertBytesToCarDamagePacket(byte[] bytes)
    {
        CarDamage packet = Converter.BytesToPacket<CarDamage>(bytes);

        Packets.Standard.CarDamageData[] carDamageData = new Packets.Standard.CarDamageData[MaxCarsPerRace];
        for (int i = 0; i < MaxCarsPerRace; i++)
        {
            carDamageData[i] = new Packets.Standard.CarDamageData(
                packet.carDamageData[i].tyreWear,
                packet.carDamageData[i].tyreDamage,
                packet.carDamageData[i].brakeDamage,
                packet.carDamageData[i].frontLeftWingDamage,
                packet.carDamageData[i].frontRightWingDamage,
                packet.carDamageData[i].rearWingDamage,
                packet.carDamageData[i].floorDamage,
                packet.carDamageData[i].diffuserDamage,
                packet.carDamageData[i].sidepodDamage,
                packet.carDamageData[i].drsFault,
                packet.carDamageData[i].gearBoxDamage,
                packet.carDamageData[i].engineDamage,
                packet.carDamageData[i].engineMGUHWear,
                packet.carDamageData[i].engineESWear,
                packet.carDamageData[i].engineCEWear,
                packet.carDamageData[i].engineICEWear,
                packet.carDamageData[i].engineMGUKWear,
                packet.carDamageData[i].engineICWear);
        }

        return new Packets.Standard.CarDamage(carDamageData);
    }

    Packets.Standard.SessionHistory ConvertBytesToSessionHistoryPacket(byte[] bytes)
    {
        SessionHistory packet = Converter.BytesToPacket<SessionHistory>(bytes);

        Packets.Standard.LapHistoryData[] lapHistoryData = new Packets.Standard.LapHistoryData[LAP_HISTORY_MAX];
        Packets.Standard.TyreStintHistoryData[] tyreStintHistoryData = new Packets.Standard.TyreStintHistoryData[TYRE_STINT_HISTORY_MAX];

        for (int i = 0; i < LAP_HISTORY_MAX; i++)
        {
            if (i < TYRE_STINT_HISTORY_MAX)
            {
                tyreStintHistoryData[i] = new Packets.Standard.TyreStintHistoryData(
                    packet.tyreStintHistoryData[i].endLap,
                    packet.tyreStintHistoryData[i].tyreActualCompound,
                    packet.tyreStintHistoryData[i].tyreVisualCompound);
            }

            lapHistoryData[i] = new Packets.Standard.LapHistoryData(
                packet.lapHistoryData[i].lapTime,
                packet.lapHistoryData[i].sector1Time,
                packet.lapHistoryData[i].sector2Time,
                packet.lapHistoryData[i].sector3Time,
                packet.lapHistoryData[i].lapValidBitFlags);
        }

        return new Packets.Standard.SessionHistory(
            packet.carIdx,
            packet.numLaps,
            packet.numTyreStints,
            packet.bestLapTimeLapNum,
            packet.bestSector1LapNum,
            packet.bestSector2LapNum,
            packet.bestSector3LapNum,
            lapHistoryData,
            tyreStintHistoryData);
    }

    public object? GetEvent(byte[] remainingPacket)
    {
        EventType eventType = GetEventType(remainingPacket);
        return eventType switch
        {
            EventType.BUTN => Converter.BytesToPacket<Buttons>(remainingPacket),
            EventType.FLBK => Converter.BytesToPacket<Flashback>(remainingPacket),
            EventType.STLG => Converter.BytesToPacket<StartLights>(remainingPacket),
            EventType.LGOT => Converter.BytesToPacket<StartLights>(remainingPacket),
            EventType.SPTP => Converter.BytesToPacket<SpeedTrap>(remainingPacket),
            EventType.FTLP => Converter.BytesToPacket<FastestLap>(remainingPacket),
            EventType.TMPT => Converter.BytesToPacket<TeamMateInPits>(remainingPacket),
            EventType.RCWN => Converter.BytesToPacket<RaceWinner>(remainingPacket),
            EventType.RTMT => Converter.BytesToPacket<Retirement>(remainingPacket),
            EventType.PENA => Converter.BytesToPacket<Penalty>(remainingPacket),
            EventType.SGSV => Converter.BytesToPacket<StopGoPenaltyServed>(remainingPacket),
            EventType.DTSV => Converter.BytesToPacket<DriveThroughPenaltyServed>(remainingPacket),
            _ => null,
        };
    }
}

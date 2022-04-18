namespace F1TelemetryAppTests.F1TelemetryAppTests;

using F1GameTelemetry.Packets.F12021;
using F1GameTelemetry.Enums;
using NUnit.Framework;
using F1TelemetryApp.Model;

internal class DriverTests
{
    private ParticipantData participantData;

    [SetUp]
    public void Setup()
    {
        participantData = new ParticipantData
        {
            name = "TEST name",
            nationality = Nationality.Welsh,
            teamId = Team.McLaren,
            raceNumber = 11,
            aiControlled = AiControlled.Human,
            driverId = DriverId.Human,
            yourTelemetry = UdpSetting.Public
        };
    }

    [TearDown]
    public void Teardown()
    {
    }

    [Test]
    public void Ctor_CreatesHumanDriver_WhenHumanDataIsValid()
    {
        // Arrange & Act
        var result = new Driver(0, participantData);

        // Assert
        Assert.AreEqual(participantData.name, result.Name);
        Assert.AreEqual(participantData.nationality, result.Nationality);
        Assert.AreEqual(participantData.raceNumber, result.RaceNumber);
    }

    [Test]
    public void ApplyLapData_AppliesLapData_WhenDataIsValid()
    {
        // Arrange
        var driver = new Driver(0, participantData);
        var lap1Sector1Data = new LapData
        {
            carLapData = new CarLapData[]
            {
                new CarLapData
                {
                    gridPosition = 8,
                    carPosition = 8,
                    currentLapNum = 1,
                    lastLapTime = 0,
                    sector1Time = 0,
                    sector2Time = 0,
                    sector = Sector.Sector1,
                    currentLapTime = 24000
                }
            }
        };
        var lap1Sector2Data = new LapData
        {
            carLapData = new CarLapData[]
            {
                new CarLapData
                {
                    gridPosition = 8,
                    carPosition = 7,
                    currentLapNum = 1,
                    lastLapTime = 0,
                    sector1Time = 25000,
                    sector2Time = 0,
                    sector = Sector.Sector2,
                    currentLapTime = 50000 // Something close but not quite the full lap time
                }
            }
        };
        var lap1Sector3Data = new LapData
        {
            carLapData = new CarLapData[]
            {
                new CarLapData
                {
                    gridPosition = 8,
                    carPosition = 7,
                    currentLapNum = 1,
                    lastLapTime = 0,
                    sector1Time = 25000,
                    sector2Time = 25900,
                    sector = Sector.Sector3,
                    currentLapTime = 72100 // Something close but not quite the full lap time
                }
            }
        };
        var lap2Sector1Data = new LapData
        {
            carLapData = new CarLapData[]
             {
                new CarLapData {
                    gridPosition = 8,
                    carPosition = 6,
                    currentLapNum = 2,
                    lastLapTime = 72325, // 01:12:325
                    sector = Sector.Sector1,
                    sector1Time = 0,
                    sector2Time = 0,
                    currentLapTime = 23900
                }
             }
        };
        var lap2Sector2Data = new LapData
        {
            carLapData = new CarLapData[]
             {
                new CarLapData {
                    gridPosition = 8,
                    carPosition = 6,
                    currentLapNum = 2,
                    lastLapTime = 72325, // 01:12:325
                    sector = Sector.Sector2,
                    sector1Time = 24900,
                    sector2Time = 0,
                    currentLapTime = 48000
                }
             }
        };
        var lap2Sector3Data = new LapData
        {
            carLapData = new CarLapData[]
             {
                new CarLapData {
                    gridPosition = 8,
                    carPosition = 6,
                    currentLapNum = 2,
                    lastLapTime = 72325, // 01:12:325
                    sector = Sector.Sector3,
                    sector1Time = 24900,
                    sector2Time = 26000,
                    currentLapTime = 70000
                }
             }
        };
        var lap3Sector1Data = new LapData
        {
            carLapData = new CarLapData[]
            {
                new CarLapData {
                    gridPosition = 8,
                    carPosition = 5,
                    currentLapNum = 3,
                    lastLapTime = 72125, // 01:12:125
                    sector1Time = 0,
                    sector2Time = 0,
                    currentLapTime = 20000
                }
            }
        };
        var lap3Sector2Data = new LapData
        {
            carLapData = new CarLapData[]
            {
                new CarLapData {
                    gridPosition = 8,
                    carPosition = 5,
                    currentLapNum = 3,
                    lastLapTime = 72125, // 01:12:125
                    sector1Time = 25100,
                    sector2Time = 0,
                    currentLapTime = 48000 // 01:12:400
                }
            }
        };
        var lap3Sector3Data = new LapData
        {
            carLapData = new CarLapData[]
            {
                new CarLapData {
                    gridPosition = 8,
                    carPosition = 5,
                    currentLapNum = 3,
                    lastLapTime = 72125, // 01:12:125
                    sector1Time = 25100,
                    sector2Time = 26100,
                    currentLapTime = 70000
                }
            }
        };
        var raceFinishedData = new LapData
        {
            carLapData = new CarLapData[]
            {
                new CarLapData {
                    gridPosition = 8,
                    carPosition = 5,
                    currentLapNum = 3,
                    resultStatus = ResultStatus.Finished,
                    lastLapTime = 72125, // 01:12:125
                    sector1Time = 25100,
                    sector2Time = 26100,
                    currentLapTime = 72400 // 01:12:400
                }
            }
        };

        // Act
        driver.ApplyLapData(lap1Sector1Data);
        driver.ApplyLapData(lap1Sector2Data);
        driver.ApplyLapData(lap1Sector3Data);
        driver.ApplyLapData(lap2Sector1Data);
        driver.ApplyLapData(lap2Sector2Data);
        driver.ApplyLapData(lap2Sector3Data);
        driver.ApplyLapData(lap3Sector1Data);
        driver.ApplyLapData(lap3Sector2Data);
        driver.ApplyLapData(lap3Sector3Data);
        driver.ApplyLapData(raceFinishedData);

        // Assert
        Assert.AreEqual(ResultStatus.Finished, driver.ResultStatus);
        Assert.AreEqual(2, driver.BestSector1);
        Assert.AreEqual(1, driver.BestSector2);
        Assert.AreEqual(3, driver.BestSector3);
        Assert.AreEqual(2, driver.BestFullLap);
        Assert.AreEqual(72400, driver.LapTimes[driver.Laps - 1].TotalLapTime);
    }
}

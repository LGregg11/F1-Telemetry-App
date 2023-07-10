namespace F1TelemetryAppTests.F1TelemetryAppTests;

using F1GameTelemetry.Enums;
using F1GameTelemetry.Models;

using F1TelemetryApp.Model;

using NUnit.Framework;

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
            driverId = DriverId.Human
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
        var raceFinishedData = new CarLapData
        {
            carPosition = 5,
            currentLapNum = 3,
            resultStatus = ResultStatus.Finished,
            sector1Time = 25100,
            sector2Time = 26100,
            currentLapTime = 72400 // 01:12:400
        };

        // Act
        driver.ApplyCarLapData(raceFinishedData);

        // Assert
        Assert.AreEqual(ResultStatus.Finished, driver.ResultStatus);
    }

    [Test]
    public void ApplySessionHistory_AppliesSessionHistory_WhenDataIsValid()
    {
        // Arrange
        // TODO

        // Act

        // Assert
        Assert.Pass();
    }
}

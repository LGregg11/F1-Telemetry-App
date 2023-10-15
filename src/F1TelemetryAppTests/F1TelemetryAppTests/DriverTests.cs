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
    public void ApplySessionHistory_AppliesSessionHistory_WhenDataIsValid()
    {
        // Arrange
        // TODO

        // Act

        // Assert
        Assert.Pass();
    }
}

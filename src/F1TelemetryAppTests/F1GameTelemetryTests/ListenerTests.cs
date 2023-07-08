namespace F1TelemetryAppTests.F1GameTelemetryTests;

using Moq;
using NUnit.Framework;

using F1GameTelemetry.Listener;

internal class ListenerTests
{
    private Mock<IUdpClient> clientMock;
    private TelemetryListener cut;
    private readonly int portMock = 10101;

    [SetUp]
    public void Setup()
    {
        clientMock = new Mock<IUdpClient>();
        cut = new TelemetryListener(portMock, clientMock.Object);
    }

    [TearDown]
    public void Teardown()
    {
    }

    [Test]
    public void Start_ShouldStartTelemetryThread()
    {
        // Act & Arrange
        cut.Start();

        // Assert
        Assert.IsTrue(cut.IsListenerRunning);
    }

    [Test]
    public void Start_ShouldStartUdpClient()
    {
        // Act & Arrange
        cut.Start();

        // Assert
        // Assert.IsNotNull(cut.Client); TODO: Find another way to prove client starts up
    }
}

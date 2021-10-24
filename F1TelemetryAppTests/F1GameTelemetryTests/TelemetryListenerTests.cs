namespace F1TelemetryAppTests.F1GameTelemetryTests
{
    using NUnit.Framework;
    using System.Threading.Tasks;
    using F1GameTelemetry.Listener;

    public class TelemetryListenerTests
    {
        private TelemetryListener cut;
        private int portMock = 10101;

        [SetUp]
        public void Setup()
        {
            Task.Delay(500);
            cut = new TelemetryListener(portMock);
        }

        [TearDown]
        public void Teardown()
        {
            if (cut != null)
                cut.Stop();
        }

        [Test]
        public void Start_ShouldStartTelemetryThread()
        {
            // Act
            // Arrange
            cut.Start();

            // Assert
            Assert.IsTrue(cut.ListenerThread.IsAlive);
            Assert.AreEqual("Telemetry Listener Thread", cut.ListenerThread.Name);
        }

        [Test]
        public void Start_ShouldStartUdpClient()
        {
            // Act
            // Arrange
            cut.Start();
            Task.Delay(1000);

            // Assert
            Assert.IsNotNull(cut.Client);
        }
    }
}
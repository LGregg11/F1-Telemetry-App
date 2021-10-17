namespace F1TelemetryAppTests
{
    using F1_Telemetry_App.Model;
    using NUnit.Framework;
    using System.Linq;
    using UdpPackets;
    using TR = F1_Telemetry_App.Model.TelemetryReader;

    [TestFixture]
    public class TelemetryReaderTests
    {
        [Test]
        public void ByteArrayToUdpPacketHeader_ShouldReturnTelemetryHeaderObject_WhenHeaderIsValid()
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = new byte[] { 229, 7, 1, 12, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 66, 85, 84, 78, 32, 0, 0, 0, 3, 0, 0, 0 };

            // Act
            var result = TR.ByteArrayToUdpPacketHeader(input);

            // Assert
            Assert.AreEqual(typeof(UdpPacketHeader), result.GetType());
        }

        [Test]
        public void ByteArrayToUdpPacketHeader_ShouldReturn2021PacketFormat_WhenHeaderIsValidAndOf2021Format()
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = new byte[] { 229, 7, 1, 12, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 66, 85, 84, 78, 32, 0, 0, 0, 3, 0, 0, 0 };

            // Act
            var result = TR.ByteArrayToUdpPacketHeader(input);

            // Assert
            Assert.AreEqual(typeof(UdpPacketHeader), result.GetType());
            Assert.AreEqual(2021, result.packetFormat);
        }

        [Test]
        public void ByteArrayToUdpPacketHeader_ShouldReturnEventPacketIdType_WhenHeaderIsValidAndOfEventType()
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = new byte[] { 229, 7, 1, 12, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 66, 85, 84, 78, 32, 0, 0, 0, 3, 0, 0, 0 };

            // Act
            var result = TR.ByteArrayToUdpPacketHeader(input);

            // Assert
            Assert.AreEqual(typeof(UdpPacketHeader), result.GetType());
            Assert.AreEqual(TR.PacketIds.Event, (TR.PacketIds)result.packetId);
        }

        [Test]
        public void GetEventType_ShouldReturnCorrectEventType_WhenUdpPacketIsValidAndOfEventType()
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = new byte[] { 229, 7, 1, 12, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 66, 85, 84, 78, 32, 0, 0, 0, 3, 0, 0, 0 };
            input = input.Skip(TR.TELEMETRY_HEADER_SIZE).ToArray();

            // Act
            var result = TR.GetEventType(input);

            // Assert
            Assert.AreEqual(TR.EventType.BUTN, result);
        }
    }
}

namespace F1TelemetryAppTests
{
    using F1_Telemetry_App.Model;
    using NUnit.Framework;
    using System.Linq;
    using System.Text;
    using UdpPackets;
    using TR = F1_Telemetry_App.Model.TelemetryReader;

    [TestFixture]
    public class TelemetryReaderTests
    {
        #region ByteArrayToUdpPacketHeader
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

        #endregion

        #region GetEventType
        [TestCase("BUTN", TR.EventType.BUTN)]
        [TestCase("CHQF", TR.EventType.CHQF)]
        [TestCase("DRSD", TR.EventType.DRSD)]
        [TestCase("DRSE", TR.EventType.DRSE)]
        [TestCase("DTSV", TR.EventType.DTSV)]
        [TestCase("FLBK", TR.EventType.FLBK)]
        [TestCase("FTLP", TR.EventType.FTLP)]
        [TestCase("LGOT", TR.EventType.LGOT)]
        [TestCase("PENA", TR.EventType.PENA)]
        [TestCase("RCWN", TR.EventType.RCWN)]
        [TestCase("RTMT", TR.EventType.RTMT)]
        [TestCase("SEND", TR.EventType.SEND)]
        [TestCase("SGSV", TR.EventType.SGSV)]
        [TestCase("SPTP", TR.EventType.SPTP)]
        [TestCase("SSTA", TR.EventType.SSTA)]
        [TestCase("STLG", TR.EventType.STLG)]
        [TestCase("TMPT", TR.EventType.TMPT)]
        public void GetEventType_ShouldReturnCorrectEventType_WhenUdpPacketIsValidAndOfEventType(string inputString, TR.EventType expected)
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = Encoding.ASCII.GetBytes(inputString);

            // Act
            var result = TR.GetEventType(input);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestCase("AAAA", TR.EventType.UNKNOWN)]
        public void GetEventType_ShouldReturnUnknownEventType_WhenUdpPacketIsValidAndEventTypeIsUnknown(string inputString, TR.EventType expected)
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = Encoding.ASCII.GetBytes(inputString);

            // Act
            var result = TR.GetEventType(input);

            // Assert
            Assert.AreEqual(expected, result);
        }

        #endregion
    }
}

namespace F1TelemetryAppTests.F1GameTelemetryTests
{
    using F1GameTelemetry.Converters;
    using NUnit.Framework;
    using F1GameTelemetry.Packets.F12021;
    using F1GameTelemetry.Enums;

    [TestFixture]
    public class ConvertersTests
    {
        [TestCase(1.0f, 2.0f, 3.0f, 3.742d)]
        [TestCase(2.0f, 3.0f, 4.0f, 5.385d)]
        public void GetMagnitudeFromVectorData_ShouldReturnCorrectValue_WhenVectorDataIsValid(float x, float y, float z, double expected)
        {
            // Arrange
            var input = new float[] { x, y, z };

            // Act
            var result = Converter.GetMagnitudeFromVectorData(input);

            // Assert
            Assert.AreEqual(expected, result);

        }

        #region BytesToPacket

        [Test]
        public void BytesToPacket_ShouldReturnUdpPacketHeader_WhenPacketIsValid()
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = new byte[] { 229, 7, 1, 12, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 66, 85, 84, 78, 32, 0, 0, 0, 3, 0, 0, 0 };

            // Act
            var result = Converter.BytesToPacket<Header>(input);

            // Assert
            Assert.AreEqual(typeof(Header), result.GetType());
        }

        [Test]
        public void BytesToPacket_ShouldReturnHeaderPacket_WhenPacketIsValid()
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = new byte[] { 229, 7, 1, 12, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 66, 85, 84, 78, 32, 0, 0, 0, 3, 0, 0, 0 };

            // Act
            var result = Converter.BytesToPacket<Header>(input);

            // Assert
            Assert.AreEqual(typeof(Header), result.GetType());
            Assert.AreEqual(ReaderVersion.F12021, result.packetFormat);
        }

        [Test]
        public void BytesToPacket_ShouldReturnCorrectEvent_WhenPacketIsValid()
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = new byte[] { 229, 7, 1, 12, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 66, 85, 84, 78, 32, 0, 0, 0, 3, 0, 0, 0 };

            // Act
            var result = Converter.BytesToPacket<Header>(input);

            // Assert
            Assert.AreEqual(typeof(Header), result.GetType());
        }

        #endregion
    }
}

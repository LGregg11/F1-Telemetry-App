namespace F1TelemetryAppTests
{
    using F1GameTelemetry.Packets;

    using NUnit.Framework;
    using System;
    using System.Text;

    using TR = F1GameTelemetry.Reader.TelemetryReader;


    [TestFixture]
    public class TelemetryReaderTests
    {
        #region ByteArrayToUdpPacket

        [Test]
        public void ByteArrayToUdpPacketStruct_ShouldReturnUdpPacketHeaderStruct_WhenPacketIsValid()
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = new byte[] { 229, 7, 1, 12, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 66, 85, 84, 78, 32, 0, 0, 0, 3, 0, 0, 0 };

            // Act
            var result = TR.ByteArrayToUdpPacketStruct<Header>(input);

            // Assert
            Assert.AreEqual(typeof(Header), result.GetType());
        }

        [Test]
        public void ByteArrayToUdpPacketStruct_ShouldReturnUdpPacketHeaderFormat_WhenPacketIsValidUdpPacketHeader()
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = new byte[] { 229, 7, 1, 12, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 66, 85, 84, 78, 32, 0, 0, 0, 3, 0, 0, 0 };

            // Act
            var result = TR.ByteArrayToUdpPacketStruct<Header>(input);

            // Assert
            Assert.AreEqual(typeof(Header), result.GetType());
            Assert.AreEqual(2021, result.packetFormat);
        }

        [Test]
        public void ByteArrayToUdpPacketStruct_ShouldReturnCorrectEventStruct_WhenPacketIsValid()
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = new byte[] { 229, 7, 1, 12, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 66, 85, 84, 78, 32, 0, 0, 0, 3, 0, 0, 0 };

            // Act
            var result = TR.ByteArrayToUdpPacketStruct<Header>(input);

            // Assert
            Assert.AreEqual(typeof(Header), result.GetType());
        }

        #region EventStructs

        [Test]
        public void ByteArrayToUdpPacketStruct_ShouldReturnButtonsStruct_WhenPacketIsValid()
        {
            // Arrange - remaining packet of a button press
            byte[] input = new byte[] { 66, 85, 84, 78, 32, 0, 0, 0, 3, 0, 0, 0 };

            // Act
            var result = TR.GetEventStruct(input);

            // Assert
            Assert.AreEqual(typeof(Buttons), result.GetType());
            Assert.NotNull(((Buttons)result).buttonStatus);
        }

        #endregion

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

        #region GetEventStruct

        [TestCase(new byte[] { 66, 85, 84, 78, 32, 0, 0, 0, 3, 0, 0, 0 }, typeof(Buttons))]
        [TestCase(new byte[] { 83, 84, 76, 71, 1, 0, 0, 0, 0, 0, 0, 0 }, typeof(StartLights))]
        [TestCase(new byte[] { 70, 76, 66, 75, 77, 1, 0, 0, 192, 227, 103, 65 }, typeof(Flashback))]
        [TestCase(new byte[] { 70, 84, 76, 80, 12, 142, 87, 159, 66, 135, 6, 195 }, typeof(FastestLap))]
        [TestCase(new byte[] { 82, 67, 87, 78, 11, 26, 1, 0, 0, 0, 0, 0 }, typeof(RaceWinner))]
        [TestCase(new byte[] { 80, 69, 78, 65, 5, 4, 15, 19, 255, 1, 0, 0 }, typeof(Penalty))]
        [TestCase(new byte[] { 83, 80, 84, 80, 12, 210, 38, 147, 67, 1, 1, 0 }, typeof(SpeedTrap))]
        public void GetEventStruct_ShouldReturnCorrectStruct_WhenPacketIsValid(byte[] input, Type expectedType)
        {
            // Arrange
            // Act
            var result = TR.GetEventStruct(input);

            // Assert
            Assert.AreEqual(expectedType, result.GetType());

        }

        #endregion

        #region GetMotionStruct

        [Test]
        public void GetMotionType_ShouldReturnCorrectMotionStruct_WhenUdpPacketIsValidAndOfMotionType()
        {
            // Arrange - 20 cars in Imola (starting grid)
            byte[] motionInput = new byte[] { 215, 251, 158, 67, 43, 142, 30, 66, 48, 159, 176, 195, 222, 172, 224, 57, 120, 93, 129, 60, 184, 22, 195,
                187, 6, 128, 119, 0, 245, 253, 11, 2, 221, 255, 6, 128, 121, 217, 82, 188, 106, 158, 59, 60, 109, 56, 188, 61, 88, 27, 203, 191, 0, 138,
                112, 59, 126, 249, 143, 58, 178, 71, 146, 67, 91, 242, 30, 66, 160, 78, 173, 195, 0, 23, 189, 58, 177, 69, 155, 59, 20, 77, 243, 187, 2,
                128, 135, 0, 94, 255, 162, 0, 36, 0, 2, 128, 112, 216, 13, 189, 29, 41, 156, 187, 128, 217, 30, 61, 62, 178, 201, 191, 0, 30, 135, 59, 64,
                108, 146, 186, 252, 243, 141, 67, 40, 34, 31, 66, 204, 146, 176, 195, 118, 13, 8, 187, 98, 175, 95, 60, 36, 79, 182, 187, 3, 128, 143, 0,
                38, 1, 218, 254, 13, 0, 3, 128, 175, 209, 16, 189, 248, 228, 107, 59, 90, 81, 145, 61, 166, 233, 199, 191, 0, 185, 143, 59, 214, 123, 208,
                185, 53, 27, 201, 67, 142, 50, 29, 66, 214, 97, 176, 195, 44, 237, 146, 186, 244, 10, 92, 187, 238, 216, 145, 58, 2, 128, 92, 0, 184, 255,
                72, 0, 245, 255, 2, 128, 102, 199, 238, 58, 151, 161, 254, 58, 118, 125, 206, 188, 137, 88, 201, 191, 0, 88, 57, 59, 231, 93, 185, 57, 183,
                128, 133, 67, 106, 110, 31, 66, 60, 186, 176, 195, 171, 32, 122, 186, 114, 211, 166, 186, 32, 37, 33, 59, 2, 128, 115, 0, 199, 0, 57, 255,
                30, 0, 2, 128, 124, 182, 242, 59, 78, 28, 8, 60, 92, 26, 3, 60, 88, 72, 200, 191, 0, 124, 102, 59, 208, 225, 116, 186, 156, 179, 137, 67,
                186, 71, 31, 66, 240, 69, 173, 195, 136, 172, 233, 184, 163, 209, 130, 188, 127, 231, 203, 59, 3, 128, 125, 0, 6, 255, 250, 0, 0, 0, 2, 128,
                119, 189, 143, 60, 106, 131, 91, 188, 138, 217, 209, 189, 134, 10, 202, 191, 0, 40, 123, 59, 128, 235, 38, 183, 113, 20, 180, 67, 8, 229,
                29, 66, 201, 7, 173, 195, 58, 118, 187, 57, 180, 208, 112, 60, 128, 103, 183, 187, 2, 128, 146, 0, 191, 0, 65, 255, 213, 255, 2, 128, 244,
                164, 113, 188, 226, 228, 207, 59, 8, 17, 148, 61, 48, 80, 200, 191, 0, 237, 145, 59, 95, 152, 173, 58, 247, 136, 188, 67, 175, 143, 29, 66,
                187, 14, 173, 195, 40, 177, 99, 185, 1, 2, 91, 60, 124, 225, 157, 187, 3, 128, 121, 0, 23, 1, 233, 254, 21, 0, 3, 128, 91, 168, 166, 188,
                104, 226, 135, 186, 67, 84, 63, 61, 144, 248, 199, 191, 0, 196, 114, 59, 73, 2, 40, 186, 66, 219, 196, 67, 208, 83, 29, 66, 160, 17, 173,
                195, 137, 128, 171, 58, 192, 188, 19, 187, 96, 184, 38, 58, 7, 128, 108, 0, 157, 253, 99, 2, 7, 0, 7, 128, 190, 199, 232, 187, 246, 99, 36,
                188, 144, 122, 143, 60, 69, 115, 203, 191, 0, 158, 89, 59, 102, 188, 98, 185, 239, 199, 192, 67, 126, 119, 29, 66, 171, 93, 176, 195, 79, 1,
                25, 186, 132, 68, 20, 58, 160, 166, 170, 185, 2, 128, 121, 0, 87, 255, 169, 0, 13, 0, 2, 128, 38, 174, 46, 60, 64, 29, 35, 60, 176, 139, 220,
                188, 177, 185, 201, 191, 0, 12, 115, 59, 34, 165, 211, 185, 139, 84, 184, 67, 166, 193, 29, 66, 22, 132, 176, 195, 191, 20, 233, 186, 40, 37,
                199, 185, 156, 18, 248, 186, 3, 128, 112, 0, 253, 254, 3, 1, 248, 255, 3, 128, 67, 133, 175, 60, 122, 92, 27, 60, 91, 231, 166, 189, 140, 19,
                202, 191, 0, 68, 96, 59, 62, 203, 142, 57, 116, 27, 122, 67, 7, 193, 31, 66, 35, 178, 176, 195, 38, 170, 151, 58, 171, 121, 170, 59, 104, 175,
                157, 187, 2, 128, 140, 0, 112, 0, 144, 255, 102, 0, 2, 128, 144, 57, 6, 189, 122, 162, 149, 188, 111, 21, 3, 188, 106, 159, 200, 191, 0, 28,
                141, 59, 153, 228, 77, 187, 19, 147, 113, 67, 205, 209, 31, 66, 253, 85, 173, 195, 178, 166, 165, 58, 228, 60, 75, 188, 22, 111, 242, 59, 2,
                128, 144, 0, 125, 255, 132, 0, 97, 0, 2, 128, 193, 214, 231, 60, 141, 159, 83, 58, 135, 226, 164, 189, 72, 148, 201, 191, 0, 194, 143, 59, 20,
                232, 66, 187, 159, 187, 154, 67, 237, 194, 30, 66, 201, 39, 173, 195, 184, 148, 126, 186, 219, 253, 60, 59, 32, 11, 31, 57, 2, 128, 72, 0, 180,
                255, 76, 0, 247, 255, 2, 128, 104, 46, 112, 60, 123, 125, 157, 59, 3, 222, 10, 189, 239, 91, 201, 191, 0, 254, 16, 59, 157, 82, 146, 57, 211,
                64, 129, 67, 102, 135, 31, 66, 239, 57, 173, 195, 50, 55, 34, 59, 116, 64, 181, 58, 206, 242, 102, 59, 3, 128, 92, 0, 173, 254, 83, 1, 89, 0,
                3, 128, 24, 30, 152, 60, 78, 106, 76, 188, 149, 141, 75, 188, 215, 99, 202, 191, 0, 92, 55, 59, 191, 174, 50, 187, 23, 111, 167, 67, 33, 79,
                30, 66, 181, 121, 176, 195, 134, 40, 238, 58, 82, 185, 227, 59, 236, 207, 186, 58, 5, 128, 125, 0, 81, 254, 175, 1, 189, 255, 4, 128, 136, 196,
                139, 59, 98, 163, 227, 59, 209, 19, 84, 61, 97, 191, 202, 191, 0, 172, 124, 59, 203, 195, 7, 59, 73, 137, 150, 67, 35, 225, 30, 66, 166, 154,
                176, 195, 53, 172, 31, 187, 82, 48, 66, 187, 65, 216, 102, 187, 7, 128, 106, 0, 161, 253, 96, 2, 28, 0, 7, 128, 169, 68, 71, 188, 23, 121, 63,
                60, 204, 199, 134, 188, 241, 111, 203, 191, 0, 214, 84, 59, 249, 209, 96, 186, 93, 162, 171, 67, 246, 33, 30, 66, 179, 48, 173, 195, 245, 152,
                170, 58, 199, 240, 221, 59, 164, 247, 201, 186, 2, 128, 124, 0, 103, 0, 152, 255, 182, 255, 2, 128, 195, 160, 243, 187, 128, 177, 2, 60, 43, 48,
                88, 61, 168, 167, 200, 191, 0, 176, 120, 59, 252, 174, 21, 59, 85, 46, 163, 67, 166, 130, 30, 66, 75, 43, 173, 195, 72, 182, 107, 185, 232, 102,
                157, 186, 152, 161, 59, 58, 2, 128, 114, 0, 11, 0, 244, 255, 195, 255, 2, 128, 196, 99, 222, 60, 77, 121, 17, 60, 230, 65, 86, 189, 199, 3, 201,
                191, 0, 198, 100, 59, 185, 235, 245, 58, 113, 226, 175, 67, 242, 244, 29, 66, 150, 127, 176, 195, 221, 131, 29, 186, 112, 206, 130, 58, 204, 10,
                251, 186, 4, 128, 120, 0, 169, 254, 86, 1, 177, 255, 3, 128, 39, 254, 40, 186, 78, 112, 250, 59, 1, 208, 233, 186, 188, 102, 202, 191, 0, 30, 115,
                59, 71, 205, 30, 59, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 150, 62, 193, 10, 196, 58, 193, 114, 125, 29, 191, 20,
                188, 224, 190, 7, 163, 19, 193, 132, 162, 113, 64, 237, 133, 22, 193, 31, 25, 78, 64, 84, 160, 208, 195, 71, 186, 42, 67, 120, 180, 212, 195, 126,
                158, 17, 67, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 249, 226, 249, 186, 143, 28, 131, 58,
                52, 186, 35, 58, 42, 147, 224, 60, 97, 55, 68, 58, 92, 114, 87, 57, 99, 32, 34, 245, 52, 40, 41, 241, 113, 57, 24, 60, 0, 0, 0, 128 };

            // Act
            var result = TR.GetMotionStruct(motionInput);

            // Assert
            Assert.AreEqual(typeof(FullMotionData), result.GetType());
            Assert.AreEqual(typeof(CarMotionData[]), ((FullMotionData)result).CarMotionData.GetType());
            Assert.AreEqual(20, ((FullMotionData)result).CarMotionData.Length);
        
        }
        
        #endregion
    }
}

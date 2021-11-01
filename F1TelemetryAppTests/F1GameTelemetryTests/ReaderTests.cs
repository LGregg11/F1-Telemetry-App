﻿namespace F1TelemetryAppTests.F1GameTelemetryTests
{
    using F1GameTelemetry.Packets;
    using F1GameTelemetry.Packets.Enums;
    using NUnit.Framework;
    using System;
    using System.Text;

    using TC = F1GameTelemetry.Converters.Converter;
    using TR = F1GameTelemetry.Reader.TelemetryReader;


    [TestFixture]
    public class ReaderTests
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

        [TestCase("BUTN", EventType.BUTN)]
        [TestCase("CHQF", EventType.CHQF)]
        [TestCase("DRSD", EventType.DRSD)]
        [TestCase("DRSE", EventType.DRSE)]
        [TestCase("DTSV", EventType.DTSV)]
        [TestCase("FLBK", EventType.FLBK)]
        [TestCase("FTLP", EventType.FTLP)]
        [TestCase("LGOT", EventType.LGOT)]
        [TestCase("PENA", EventType.PENA)]
        [TestCase("RCWN", EventType.RCWN)]
        [TestCase("RTMT", EventType.RTMT)]
        [TestCase("SEND", EventType.SEND)]
        [TestCase("SGSV", EventType.SGSV)]
        [TestCase("SPTP", EventType.SPTP)]
        [TestCase("SSTA", EventType.SSTA)]
        [TestCase("STLG", EventType.STLG)]
        [TestCase("TMPT", EventType.TMPT)]
        public void GetEventType_ShouldReturnCorrectEventType_WhenUdpPacketIsValidAndOfEventType(string inputString, EventType expected)
        {
            // Arrange - Byte array was taken from a button event
            byte[] input = Encoding.ASCII.GetBytes(inputString);

            // Act
            var result = TR.GetEventType(input);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestCase("AAAA", EventType.UNKNOWN)]
        public void GetEventType_ShouldReturnUnknownEventType_WhenUdpPacketIsValidAndEventTypeIsUnknown(string inputString, EventType expected)
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
        public void GetMotionStruct_ShouldReturnCorrectMotionStruct_WhenUdpPacketIsValidAndOfMotionType()
        {
            // Arrange - 20 cars in Imola (starting grid)
            byte[] input = new byte[] { 215, 251, 158, 67, 43, 142, 30, 66, 48, 159, 176, 195, 222, 172, 224, 57, 120, 93, 129, 60, 184, 22, 195,
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
            var result = TR.GetMotionStruct(input);

            // Assert
            Assert.AreEqual(typeof(Motion), result.GetType());
            Assert.AreEqual(typeof(CarMotionData[]), result.carMotionData.GetType());
            Assert.AreEqual(TR.MAX_CARS_PER_RACE, result.carMotionData.Length);
            Assert.AreEqual(typeof(ExtraCarMotionData), result.extraCarMotionData.GetType());
            Assert.AreEqual(0.0, Math.Round(TC.GetMagnitudeFromVectorData(result.extraCarMotionData.localVelocity), 1));
        }

        #endregion

        #region GetCarTelemetryStruct

        [Test]
        public void GetCarTelemetryStruct_ShouldReturnCarTelemetryStruct_WhenPacketIsValid()
        {
            // Arrange
            byte[] input = new byte[] { 73, 0, 0, 0, 0, 0, 8, 232, 37, 191, 0, 0, 0, 0, 0, 1, 6, 36, 0, 0, 0, 0, 218, 3, 220, 3, 239, 3, 241, 3, 76, 79, 84,
                91, 93, 89, 94, 89, 89, 0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 0, 0, 1, 0, 163, 0, 119, 35, 1, 63,
                169, 214, 52, 190, 0, 0, 0, 0, 0, 4, 171, 39, 0, 0, 0, 0, 58, 3, 54, 3, 46, 3, 43, 3, 91, 88, 89, 82, 95, 88, 94, 86, 89, 0, 205, 204, 184,
                65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 0, 0, 0, 0, 152, 0, 0, 0, 0, 0, 150, 237, 129, 61, 119, 171, 20, 63, 0, 3, 222,
                43, 0, 38, 32, 0, 173, 3, 170, 3, 219, 3, 212, 3, 96, 82, 96, 83, 94, 88, 91, 84, 89, 0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177,
                65, 154, 153, 177, 65, 1, 0, 0, 0, 244, 0, 0, 0, 128, 63, 141, 168, 136, 59, 0, 0, 0, 0, 0, 6, 228, 43, 1, 26, 1, 0, 140, 2, 138, 2, 93, 2,
                89, 2, 87, 93, 80, 87, 96, 91, 95, 88, 89, 0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 0, 0, 0, 0, 201, 0,
                0, 0, 128, 63, 246, 109, 4, 189, 0, 0, 0, 0, 0, 5, 164, 41, 0, 0, 0, 0, 220, 2, 217, 2, 182, 2, 179, 2, 90, 102, 82, 95, 94, 88, 95, 87, 89,
                0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 0, 1, 0, 1, 1, 1, 0, 0, 128, 63, 89, 220, 172, 58, 0, 0, 0, 0,
                0, 6, 7, 46, 0, 88, 224, 127, 94, 2, 92, 2, 36, 2, 33, 2, 86, 89, 79, 83, 96, 90, 94, 87, 89, 0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153,
                177, 65, 154, 153, 177, 65, 0, 0, 0, 0, 73, 0, 0, 0, 0, 0, 76, 215, 77, 63, 77, 125, 31, 62, 0, 2, 135, 26, 0, 0, 0, 0, 232, 3, 228, 3, 254, 3,
                249, 3, 92, 80, 105, 86, 94, 88, 94, 86, 89, 0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 0, 0, 0, 0, 239, 0,
                0, 0, 0, 0, 221, 109, 217, 61, 0, 0, 128, 63, 0, 6, 172, 41, 0, 0, 0, 0, 212, 2, 209, 2, 220, 2, 217, 2, 103, 86, 99, 78, 92, 87, 91, 84, 89, 0,
                205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 0, 0, 0, 0, 163, 0, 245, 55, 129, 62, 208, 88, 76, 190, 0, 0, 0, 0,
                0, 4, 86, 39, 0, 0, 0, 0, 68, 3, 65, 3, 59, 3, 57, 3, 94, 87, 91, 79, 95, 88, 95, 86, 89, 0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177,
                65, 154, 153, 177, 65, 0, 0, 0, 0, 215, 0, 0, 0, 128, 63, 49, 239, 156, 188, 0, 0, 0, 0, 0, 5, 84, 44, 0, 38, 63, 0, 205, 2, 202, 2, 166, 2, 162,
                2, 90, 100, 80, 93, 95, 90, 94, 87, 89, 0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 1, 0, 0, 0, 214, 0, 170,
                183, 4, 63, 180, 64, 207, 59, 0, 0, 0, 0, 0, 5, 12, 44, 0, 31, 0, 0, 43, 3, 41, 3, 81, 3, 73, 3, 102, 85, 100, 82, 93, 88, 92, 86, 89, 0, 205,
                204, 184, 65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 0, 0, 0, 0, 102, 0, 60, 77, 186, 62, 177, 150, 127, 62, 0, 0, 0, 0, 0, 2,
                67, 39, 0, 0, 0, 0, 145, 3, 141, 3, 165, 3, 159, 3, 97, 81, 100, 80, 95, 88, 94, 86, 89, 0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177,
                65, 154, 153, 177, 65, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 81, 89, 106, 191, 0, 0, 0, 0, 0, 1, 64, 34, 0, 0, 0, 0, 204, 3, 206, 3, 225, 3, 229, 3, 76,
                82, 83, 92, 94, 91, 95, 89, 89, 0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 1, 0, 0, 0, 174, 0, 161, 82, 59,
                63, 143, 237, 89, 190, 0, 0, 0, 0, 0, 4, 67, 43, 0, 7, 0, 0, 25, 3, 22, 3, 7, 3, 3, 3, 92, 96, 88, 89, 95, 88, 95, 87, 89, 0, 205, 204, 184, 65,
                205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 0, 0, 0, 0, 190, 0, 223, 119, 43, 63, 139, 132, 1, 190, 0, 0, 0, 0, 0, 5, 98, 39, 0, 0,
                0, 0, 235, 2, 233, 2, 202, 2, 200, 2, 92, 103, 84, 96, 95, 88, 95, 87, 89, 0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153,
                177, 65, 0, 0, 0, 0, 77, 0, 0, 0, 0, 0, 86, 162, 46, 191, 0, 226, 111, 61, 0, 1, 227, 37, 0, 0, 3, 0, 255, 3, 1, 4, 22, 4, 24, 4, 78, 81, 86, 92,
                95, 91, 94, 88, 89, 0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 0, 0, 0, 0, 141, 0, 0, 0, 0, 0, 72, 52, 184,
                189, 0, 0, 128, 63, 0, 3, 158, 40, 0, 0, 0, 0, 220, 3, 221, 3, 3, 4, 3, 4, 80, 80, 86, 87, 94, 90, 94, 88, 89, 0, 205, 204, 184, 65, 205, 204, 184,
                65, 154, 153, 177, 65, 154, 153, 177, 65, 0, 0, 0, 0, 22, 1, 0, 0, 128, 63, 0, 0, 0, 128, 0, 0, 0, 0, 0, 7, 162, 43, 0, 20, 0, 0, 240, 1, 238, 1,
                68, 1, 165, 1, 84, 84, 79, 80, 94, 90, 93, 88, 89, 0, 205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 0, 0, 0, 0, 78,
                0, 0, 0, 0, 0, 100, 243, 48, 63, 51, 211, 25, 59, 0, 2, 96, 28, 0, 0, 0, 0, 204, 3, 200, 3, 234, 3, 230, 3, 90, 77, 97, 82, 94, 88, 93, 86, 89, 0,
                205, 204, 184, 65, 205, 204, 184, 65, 154, 153, 177, 65, 154, 153, 177, 65, 0, 0, 0, 0, 69, 0, 0, 0, 0, 0, 239, 30, 83, 191, 0, 0, 0, 0, 0, 2, 40,
                25, 0, 0, 31, 0, 13, 4, 14, 4, 26, 4, 28, 4, 79, 83, 82, 87, 99, 94, 98, 90, 113, 0, 137, 146, 188, 65, 34, 224, 185, 65, 17, 148, 180, 65, 146,
                182, 176, 65, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 255, 2 };

            // Act
            var result = TR.GetCarTelemetryStruct(input);

            // Assert
            Assert.AreEqual(typeof(CarTelemetry), result.GetType());
            Assert.AreEqual(typeof(CarTelemetryData[]), result.carTelemetryData.GetType());
            Assert.AreEqual(TR.MAX_CARS_PER_RACE, result.carTelemetryData.Length);
            Assert.AreEqual(MFDPanelIndexTypes.Damage, result.mfdPanelIndex);
        }

        #endregion

        #region GetCarStatusStruct

        [Test]
        public void GetCarStatusStruct_ShouldReturnCarStatusStruct_WhenPacketIsValid()
        {
            // Arrange
            byte[] input = new byte[] { 0, 1, 1, 60, 0, 133, 55, 74, 65, 0, 0, 220, 66, 224, 222, 148, 63, 200, 50, 204, 16, 9, 0, 0, 0,
                8, 8, 0, 0, 181, 62, 50, 74, 3, 246, 206, 63, 73, 130, 120, 151, 73, 151, 229, 60, 74, 0, 0, 1, 1, 60, 0, 237, 6, 76, 65,
                0, 0, 220, 66, 124, 84, 150, 63, 200, 50, 204, 16, 9, 0, 0, 0, 8, 8, 0, 0, 151, 99, 55, 74, 1, 59, 246, 50, 73, 148, 148,
                152, 73, 110, 6, 53, 74, 0, 0, 1, 1, 60, 0, 158, 8, 69, 65, 0, 0, 220, 66, 208, 41, 129, 63, 200, 50, 215, 14, 9, 0, 0, 0,
                8, 8, 0, 0, 67, 91, 50, 74, 3, 159, 149, 68, 73, 36, 194, 151, 73, 156, 199, 61, 74, 0, 0, 1, 1, 60, 0, 46, 210, 74, 65,
                0, 0, 220, 66, 36, 101, 145, 63, 200, 50, 204, 16, 9, 0, 0, 0, 8, 8, 0, 0, 162, 169, 60, 74, 1, 109, 73, 34, 73, 229, 175,
                154, 73, 242, 192, 44, 74, 0, 0, 1, 1, 60, 0, 24, 137, 81, 65, 0, 0, 220, 66, 196, 139, 172, 63, 200, 50, 171, 13, 9, 0,
                0, 0, 8, 8, 0, 0, 136, 228, 57, 74, 3, 26, 197, 81, 73, 32, 74, 149, 73, 84, 159, 56, 74, 0, 0, 1, 1, 60, 0, 66, 99, 80,
                65, 0, 0, 220, 66, 24, 182, 166, 63, 200, 50, 204, 16, 9, 0, 0, 0, 8, 8, 0, 0, 25, 209, 57, 74, 1, 230, 2, 37, 73, 242,
                123, 152, 73, 185, 252, 46, 74, 0, 0, 1, 1, 60, 0, 96, 18, 78, 65, 0, 0, 220, 66, 64, 191, 167, 63, 200, 50, 171, 13, 9,
                0, 0, 0, 8, 8, 0, 0, 175, 7, 25, 74, 1, 6, 241, 75, 73, 1, 144, 130, 73, 189, 192, 78, 74, 0, 0, 1, 1, 60, 0, 120, 92, 79,
                65, 0, 0, 220, 66, 116, 103, 164, 63, 200, 50, 171, 13, 9, 0, 0, 0, 8, 8, 0, 0, 136, 196, 71, 74, 3, 35, 204, 82, 73, 52,
                105, 156, 73, 162, 174, 46, 74, 0, 0, 1, 1, 60, 0, 106, 166, 72, 65, 0, 0, 220, 66, 204, 170, 138, 63, 200, 50, 204, 16,
                9, 0, 0, 0, 8, 8, 0, 0, 190, 47, 53, 74, 1, 21, 189, 30, 73, 165, 70, 151, 73, 107, 145, 49, 74, 0, 0, 1, 1, 60, 0, 113,
                130, 77, 65, 0, 0, 220, 66, 52, 123, 157, 63, 200, 50, 204, 16, 9, 0, 0, 0, 8, 8, 0, 0, 38, 37, 53, 74, 3, 252, 254, 40,
                73, 18, 243, 151, 73, 226, 143, 52, 74, 0, 0, 1, 1, 60, 0, 40, 53, 77, 65, 0, 0, 220, 66, 212, 126, 153, 63, 200, 50, 204,
                16, 9, 0, 0, 0, 8, 8, 0, 0, 31, 20, 64, 74, 1, 161, 19, 36, 73, 76, 248, 150, 73, 89, 173, 39, 74, 0, 0, 1, 1, 60, 0, 97,
                168, 68, 65, 0, 0, 220, 66, 208, 155, 119, 63, 200, 50, 204, 16, 9, 0, 0, 0, 8, 8, 0, 0, 184, 15, 67, 74, 1, 222, 186, 44,
                73, 200, 77, 151, 73, 137, 50, 39, 74, 0, 0, 1, 1, 60, 0, 232, 197, 69, 65, 0, 0, 220, 66, 204, 140, 137, 63, 200, 50, 204,
                16, 9, 0, 0, 0, 8, 8, 0, 0, 221, 61, 21, 74, 3, 180, 60, 44, 73, 224, 154, 134, 73, 84, 116, 76, 74, 0, 0, 1, 1, 60, 0,
                187, 222, 61, 65, 0, 0, 220, 66, 208, 184, 93, 63, 200, 50, 171, 13, 9, 0, 0, 0, 8, 8, 0, 0, 226, 187, 27, 74, 1, 245, 147,
                87, 73, 81, 146, 129, 73, 233, 148, 78, 74, 0, 0, 1, 1, 60, 0, 160, 31, 67, 65, 0, 0, 220, 66, 240, 207, 110, 63, 200, 50,
                204, 16, 9, 0, 0, 0, 8, 8, 0, 0, 205, 232, 61, 74, 1, 18, 89, 32, 73, 149, 207, 155, 73, 17, 154, 43, 74, 0, 0, 1, 1, 60,
                0, 63, 226, 68, 65, 0, 0, 220, 66, 112, 194, 124, 63, 200, 50, 204, 16, 9, 0, 0, 0, 8, 8, 0, 0, 102, 46, 57, 74, 1, 186,
                173, 32, 73, 154, 159, 151, 73, 207, 52, 46, 74, 0, 0, 1, 1, 60, 0, 151, 245, 66, 65, 0, 0, 220, 66, 48, 19, 117, 63, 200,
                50, 215, 14, 9, 0, 0, 0, 8, 8, 0, 0, 4, 15, 56, 74, 3, 62, 140, 57, 73, 215, 247, 154, 73, 101, 27, 55, 74, 0, 0, 1, 1, 60,
                0, 204, 177, 79, 65, 0, 0, 220, 66, 152, 108, 165, 63, 200, 50, 204, 16, 9, 0, 0, 0, 8, 8, 0, 0, 111, 79, 51, 74, 3, 122,
                49, 42, 73, 64, 173, 153, 73, 69, 162, 55, 74, 0, 0, 1, 1, 60, 0, 115, 119, 72, 65, 0, 0, 220, 66, 224, 108, 146, 63, 200,
                50, 204, 16, 9, 0, 0, 0, 8, 8, 0, 0, 29, 3, 19, 74, 3, 74, 70, 33, 73, 103, 188, 135, 73, 158, 165, 76, 74, 0, 1, 1, 1, 58,
                0, 7, 218, 73, 65, 0, 0, 220, 66, 248, 0, 148, 63, 200, 50, 204, 16, 9, 0, 0, 0, 8, 8, 0, 0, 235, 241, 91, 74, 1, 103, 53,
                29, 73, 229, 32, 111, 73, 84, 237, 243, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            // Act
            var result = TR.GetCarStatusStruct(input);

            // Assert
            Assert.AreEqual(typeof(CarStatus), result.GetType());
            Assert.AreEqual(TyreVisualTypes.Wet, result.carStatusData[0].visualTyreCompound);
        }

        #endregion

        #region GetCarStatusStruct

        [Test]
        public void GetFinalClassificationStruct_ShouldReturnFinalClassificationStruct_WhenPacketIsValid()
        {
            // Arrange
            byte[] input = new byte[] { 20, 20, 3, 19, 0, 0, 3, 16, 61, 1, 0, 0, 0, 0, 128, 79, 203, 111, 64, 0, 0, 1, 17, 0, 0, 0, 0, 0,
                0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 10, 3, 10, 1, 0, 3, 85, 54, 1, 0, 0, 0, 0, 128, 229, 195, 110, 64, 0, 0, 1, 17, 0, 0, 0, 0,
                0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 7, 3, 5, 6, 0, 3, 12, 52, 1, 0, 0, 0, 0, 224, 117, 100, 110, 64, 0, 0, 1, 17, 0, 0, 0, 0,
                0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 1, 3, 2, 26, 0, 3, 10, 46, 1, 0, 0, 0, 0, 128, 83, 121, 109, 64, 0, 0, 1, 17, 0, 0, 0, 0,
                0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 19, 3, 20, 0, 0, 3, 212, 60, 1, 0, 0, 0, 0, 224, 131, 184, 111, 64, 0, 0, 1, 17, 0, 0, 0,
                0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 16, 3, 13, 0, 0, 3, 225, 56, 1, 0, 0, 0, 0, 32, 198, 81, 111, 64, 0, 0, 1, 17, 0, 0, 0,
                0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 3, 3, 1, 15, 0, 3, 224, 46, 1, 0, 0, 0, 0, 224, 32, 184, 109, 64, 0, 0, 1, 17, 0, 0, 0,
                0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 17, 3, 15, 0, 0, 3, 152, 61, 1, 0, 0, 0, 0, 32, 111, 163, 111, 64, 0, 0, 1, 17, 0, 0,
                0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 2, 3, 3, 18, 0, 3, 252, 46, 1, 0, 0, 0, 0, 160, 137, 164, 109, 64, 0, 0, 1, 17, 0,
                0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 4, 3, 4, 12, 0, 3, 42, 47, 1, 0, 0, 0, 0, 160, 103, 211, 109, 64, 0, 0, 1, 17, 0,
                0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 12, 3, 12, 0, 0, 3, 109, 56, 1, 0, 0, 0, 0, 192, 204, 7, 111, 64, 0, 0, 1, 17, 0,
                0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 9, 3, 8, 2, 0, 3, 92, 52, 1, 0, 0, 0, 0, 160, 25, 148, 110, 64, 0, 0, 1, 17, 0,
                0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 11, 3, 18, 0, 0, 3, 74, 56, 1, 0, 0, 0, 0, 96, 177, 250, 110, 64, 0, 0, 1, 17, 0,
                0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 18, 3, 17, 0, 0, 3, 25, 61, 1, 0, 0, 0, 0, 64, 217, 176, 111, 64, 0, 0, 1, 17, 0,
                0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 14, 3, 14, 0, 0, 3, 20, 56, 1, 0, 0, 0, 0, 192, 55, 31, 111, 64, 0, 0, 1, 17, 0,
                0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 15, 3, 16, 0, 0, 3, 45, 57, 1, 0, 0, 0, 0, 160, 33, 68, 111, 64, 0, 0, 1, 17, 0,
                0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 13, 3, 11, 0, 0, 3, 51, 56, 1, 0, 0, 0, 0, 192, 178, 24, 111, 64, 0, 0, 1, 17, 0,
                0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 8, 3, 9, 4, 0, 3, 219, 51, 1, 0, 0, 0, 0, 64, 100, 118, 110, 64, 0, 0, 1, 17, 0,
                0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 6, 3, 7, 8, 0, 3, 230, 51, 1, 0, 0, 0, 0, 0, 95, 82, 110, 64, 0, 0, 1, 17, 0, 0,
                0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 5, 3, 6, 10, 0, 3, 84, 48, 1, 0, 0, 0, 0, 0, 149, 12, 110, 64, 0, 0, 1, 17, 0, 0, 0,
                0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0 };

            // Act
            var result = TR.GetFinalClassificationStruct(input);

            // Assert
            Assert.AreEqual(typeof(FinalClassification), result.GetType());
            Assert.AreEqual(20, (int)result.numberCars);
            Assert.AreEqual(TyreVisualTypes.Soft, result.finalClassificationData[0].tyreStintsVisual[0]);
        }

        #endregion

        #region GetCarStatusStruct

        [Test]
        public void GetLapDataStruct_ShouldReturnLapDataStruct_WhenPacketIsValid()
        {
            // Arrange
            byte[] input = new byte[] { 44, 49, 1, 0, 211, 201, 0, 0, 43, 107, 0, 0, 48, 213, 58, 69, 42, 134, 55, 70, 0, 0, 0,
                128, 1, 3, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 2, 0, 0, 0, 0, 0, 0, 253, 52, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 117, 14,
                54, 65, 117, 14, 54, 65, 0, 0, 0, 128, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 44, 49, 1, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 53, 124, 58, 69, 53, 124, 58, 69, 0, 0, 0, 128, 1, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 76, 173, 136, 69, 76, 173, 136, 69, 0, 0, 0, 128, 1, 1, 0,
                0, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 85, 105, 136, 69, 85, 105,
                136, 69, 0, 0, 0, 128, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 209, 44, 136, 69, 209, 44, 136, 69, 0, 0, 0, 128, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 48, 234, 135, 69, 48, 234, 135, 69, 0, 0, 0, 128, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0,
                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 43, 172, 135, 69, 43, 172, 135, 69, 0, 0, 0, 128,
                1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 106, 135, 69,
                20, 106, 135, 69, 0, 0, 0, 128, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 121, 42, 135, 69, 121, 42, 135, 69, 0, 0, 0, 128, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 123, 234, 134, 69, 123, 234, 134, 69, 0, 0, 0, 128, 1, 1, 0, 0, 2, 0,
                0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 235, 170, 134, 69, 235, 170, 134, 69,
                0, 0, 0, 128, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 245,
                105, 134, 69, 245, 105, 134, 69, 0, 0, 0, 128, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 82, 42, 134, 69, 82, 42, 134, 69, 0, 0, 0, 128, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            // Act
            var result = TR.GetLapDataStruct(input);

            // Assert
            Assert.AreEqual(typeof(LapData), result.GetType());
            Assert.AreEqual(TR.MAX_CARS_PER_RACE, result.carLapData.Length);
            Assert.AreEqual(Sector.Sector2, result.carLapData[0].sector);
        }

        #endregion
    }
}

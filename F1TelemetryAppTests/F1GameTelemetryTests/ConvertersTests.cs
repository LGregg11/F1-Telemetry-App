namespace F1TelemetryAppTests.F1GameTelemetryTests
{
    using F1GameTelemetry.Packets;
    using F1GameTelemetry.Converters;
    using NUnit.Framework;

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
    }
}

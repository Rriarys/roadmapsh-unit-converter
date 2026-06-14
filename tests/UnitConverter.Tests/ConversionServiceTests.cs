using UnitConverter.Api.Conversion;

namespace UnitConverter.Tests
{
    public class ConversionServiceTests
    {
        private readonly ConversionService _conversionService = new();

        #region Length Conversion Tests

        [Theory]
        [InlineData("millimeter", "meter", 1000, 1)]
        [InlineData("meter", "millimeter", 1, 1000)]
        [InlineData("centimeter", "meter", 100, 1)]
        [InlineData("kilometer", "meter", 1, 1000)]
        [InlineData("meter", "kilometer", 1000, 1)]
        [InlineData("inch", "foot", 12, 1)]
        [InlineData("foot", "yard", 3, 1)]
        [InlineData("yard", "mile", 1760, 1)]
        public void Convert_Length_WhenConvertingUnits_ReturnsExpectedValue(string fromUnit, string toUnit, double value, double expected)
        {
            var result = _conversionService.Convert("length", fromUnit, toUnit, value);
            Assert.NotNull(result.Item1);
            Assert.Equal(expected, result.Item1.Value, precision: 0);
            Assert.Equal(toUnit, result.Item2);
        }

        [Fact]
        public void Convert_Length_WhenConvertingMeterToInch_ReturnsApproximateValue()
        {
            var result = _conversionService.Convert("length", "meter", "inch", 1);
            Assert.NotNull(result.Item1);
            Assert.InRange(result.Item1.Value, 39.0, 40.0);
            Assert.Equal("inch", result.Item2);
        }

        [Fact]
        public void Convert_Length_WhenZeroValue_ReturnsZero()
        {
            var result = _conversionService.Convert("length", "meter", "kilometer", 0);
            Assert.NotNull(result.Item1);
            Assert.Equal(0.0, result.Item1.Value);
        }

        [Fact]
        public void Convert_Length_WhenNegativeValue_ReturnsNegativeResult()
        {
            var result = _conversionService.Convert("length", "meter", "kilometer", -1000);
            Assert.NotNull(result.Item1);
            Assert.Equal(-1.0, result.Item1.Value);
        }

        [Fact]
        public void Convert_Length_WhenSmallDecimalValue_PreservesDecimalPrecision()
        {
            var result = _conversionService.Convert("length", "meter", "millimeter", 0.123456789);
            Assert.NotNull(result.Item1);
            Assert.Equal(123.456789, result.Item1.Value);
        }

        [Fact]
        public void Convert_Length_WhenLargeValue_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "meter", "kilometer", 1000000);
            Assert.NotNull(result.Item1);
            Assert.Equal(1000.0, result.Item1.Value);
        }

        #endregion

        #region Mass Conversion Tests

        [Theory]
        [InlineData("milligram", "gram", 1000, 1)]
        [InlineData("gram", "kilogram", 1000, 1)]
        [InlineData("kilogram", "gram", 1, 1000)]
        [InlineData("ounce", "pound", 16, 1)]
        [InlineData("pound", "ounce", 1, 16)]
        public void Convert_Mass_WhenConvertingUnits_ReturnsExpectedValue(string fromUnit, string toUnit, double value, double expected)
        {
            var result = _conversionService.Convert("mass", fromUnit, toUnit, value);
            Assert.NotNull(result.Item1);
            Assert.Equal(expected, result.Item1.Value, precision: 0);
            Assert.Equal(toUnit, result.Item2);
        }

        [Fact]
        public void Convert_Mass_WhenConvertingGramToOunce_ReturnsApproximateValue()
        {
            var result = _conversionService.Convert("mass", "gram", "ounce", 28.3495);
            Assert.NotNull(result.Item1);
            Assert.InRange(result.Item1.Value, 0.999, 1.001);
            Assert.Equal("ounce", result.Item2);
        }

        [Fact]
        public void Convert_Mass_WhenZeroValue_ReturnsZero()
        {
            var result = _conversionService.Convert("mass", "gram", "kilogram", 0);
            Assert.NotNull(result.Item1);
            Assert.Equal(0.0, result.Item1.Value);
        }

        [Fact]
        public void Convert_Mass_WhenNegativeValue_ReturnsNegativeResult()
        {
            var result = _conversionService.Convert("mass", "gram", "kilogram", -1000);
            Assert.NotNull(result.Item1);
            Assert.Equal(-1.0, result.Item1.Value);
        }

        [Fact]
        public void Convert_Mass_WhenDecimalValue_RoundsTo4Decimals()
        {
            var result = _conversionService.Convert("mass", "gram", "gram", 1.23456789);
            Assert.NotNull(result.Item1);
            Assert.Equal(1.2346, result.Item1.Value);
        }

        #endregion

        #region Temperature Conversion Tests

        [Theory]
        [InlineData("celsius", "celsius", 25, 25)]
        [InlineData("celsius", "fahrenheit", 0, 32)]
        [InlineData("celsius", "fahrenheit", 100, 212)]
        [InlineData("fahrenheit", "celsius", 32, 0)]
        [InlineData("celsius", "kelvin", 0, 273.15)]
        [InlineData("kelvin", "celsius", 273.15, 0)]
        public void Convert_Temperature_WhenConvertingUnits_ReturnsExpectedValue(string fromUnit, string toUnit, double value, double expected)
        {
            var result = _conversionService.Convert("temperature", fromUnit, toUnit, value);
            Assert.NotNull(result.Item1);
            Assert.Equal(expected, result.Item1.Value, precision: 0);
            Assert.Equal(toUnit, result.Item2);
        }

        [Fact]
        public void Convert_Temperature_WhenNegativeCelsius_ReturnsCorrectFahrenheit()
        {
            var result = _conversionService.Convert("temperature", "celsius", "fahrenheit", -40);
            Assert.NotNull(result.Item1);
            Assert.Equal(-40.0, result.Item1.Value);
            Assert.Equal("fahrenheit", result.Item2);
        }

        [Fact]
        public void Convert_Temperature_WhenDecimalValue_RoundsTo1Decimal()
        {
            var result = _conversionService.Convert("temperature", "celsius", "celsius", 25.123456);
            Assert.NotNull(result.Item1);
            Assert.Equal(25.1, result.Item1.Value);
        }

        #endregion

        #region Null Value Tests

        [Theory]
        [InlineData("length", "meter", "kilometer")]
        [InlineData("mass", "gram", "kilogram")]
        [InlineData("temperature", "celsius", "fahrenheit")]
        public void Convert_WhenValueIsNull_ReturnsNullWithTargetUnit(string category, string fromUnit, string toUnit)
        {
            var result = _conversionService.Convert(category, fromUnit, toUnit, null);
            Assert.Null(result.Item1);
            Assert.Equal(toUnit, result.Item2);
        }

        #endregion

        #region Case Insensitive Tests

        [Theory]
        [InlineData("LENGTH", "Meter", "Kilometer", 1000, 1)]
        [InlineData("MaSs", "GrAm", "KiLoGrAm", 1000, 1)]
        [InlineData("TEMPERATURE", "CELSIUS", "FAHRENHEIT", 0, 32)]
        public void Convert_WhenCategoryIsDifferentCase_ReturnsCorrectValue(string category, string fromUnit, string toUnit, double value, double expected)
        {
            var result = _conversionService.Convert(category, fromUnit, toUnit, value);
            Assert.NotNull(result.Item1);
            Assert.Equal(expected, result.Item1.Value, precision: 0);
        }

        #endregion

        #region Exception Tests

        [Theory]
        [InlineData("invalid")]
        [InlineData("weight")]
        [InlineData("")]
        public void Convert_WhenCategoryIsInvalid_ThrowsArgumentException(string category)
        {
            Action action = () => _conversionService.Convert(category, "meter", "kilometer", 100);
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Contains("Unsupported category", exception.Message);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void Convert_WhenExtremellySmallValue_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "millimeter", "meter", 0.0001);
            Assert.NotNull(result.Item1);
            Assert.True(result.Item1.Value >= 0);
        }

        [Fact]
        public void Convert_WhenExtremelyLargeValue_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "kilometer", "meter", 999999999);
            Assert.NotNull(result.Item1);
            Assert.True(result.Item1.Value > 0);
        }

        [Fact]
        public void Convert_ReturnsTupleWithCorrectUnit()
        {
            var result = _conversionService.Convert("length", "meter", "kilometer", 1000);
            Assert.Equal("kilometer", result.Item2);
            Assert.Equal("kilometer", result.toUnit);
        }

        #endregion
    }
}

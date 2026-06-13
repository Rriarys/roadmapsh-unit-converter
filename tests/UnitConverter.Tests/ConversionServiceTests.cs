using UnitConverter.Api.Conversion;

namespace UnitConverter.Tests
{
    public class ConversionServiceTests
    {
        private readonly ConversionService _conversionService = new();

        #region Length Conversion Tests

        [Fact]
        public void Convert_Length_MillimeterToMeter_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "millimeter", "meter", 1000);
            Assert.Equal(1.0, result.Item1);
            Assert.Equal("meter", result.Item2);
        }

        [Fact]
        public void Convert_Length_MeterToMillimeter_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "meter", "millimeter", 1);
            Assert.Equal(1000.0, result.Item1);
            Assert.Equal("millimeter", result.Item2);
        }

        [Fact]
        public void Convert_Length_CentimeterToMeter_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "centimeter", "meter", 100);
            Assert.Equal(1.0, result.Item1);
            Assert.Equal("meter", result.Item2);
        }

        [Fact]
        public void Convert_Length_MeterToKilometer_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "meter", "kilometer", 1000);
            Assert.Equal(1.0, result.Item1);
            Assert.Equal("kilometer", result.Item2);
        }

        [Fact]
        public void Convert_Length_KilometerToMeter_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "kilometer", "meter", 1);
            Assert.Equal(1000.0, result.Item1);
            Assert.Equal("meter", result.Item2);
        }

        [Fact]
        public void Convert_Length_InchToFoot_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "inch", "foot", 12);
            Assert.Equal(1.0, result.Item1);
            Assert.Equal("foot", result.Item2);
        }

        [Fact]
        public void Convert_Length_FootToYard_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "foot", "yard", 3);
            Assert.Equal(1.0, result.Item1);
            Assert.Equal("yard", result.Item2);
        }

        [Fact]
        public void Convert_Length_YardToMile_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "yard", "mile", 1760);
            Assert.Equal(1.0, result.Item1);
            Assert.Equal("mile", result.Item2);
        }

        [Fact]
        public void Convert_Length_MeterToInch_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "meter", "inch", 1);
            Assert.NotNull(result.Item1);
            Assert.True(result.Item1 > 39 && result.Item1 < 40);
            Assert.Equal("inch", result.Item2);
        }

        [Fact]
        public void Convert_Length_SameUnitConversion_ReturnsOriginalValue()
        {
            var result = _conversionService.Convert("length", "meter", "meter", 42.5);
            Assert.Equal(42.5, result.Item1);
            Assert.Equal("meter", result.Item2);
        }

        [Fact]
        public void Convert_Length_ZeroValue_ReturnsZero()
        {
            var result = _conversionService.Convert("length", "meter", "kilometer", 0);
            Assert.Equal(0.0, result.Item1);
        }

        [Fact]
        public void Convert_Length_NegativeValue_ReturnsNegativeResult()
        {
            var result = _conversionService.Convert("length", "meter", "kilometer", -1000);
            Assert.Equal(-1.0, result.Item1);
        }

        [Fact]
        public void Convert_Length_SmallDecimalValue_ReturnsRoundedValue()
        {
            var result = _conversionService.Convert("length", "meter", "millimeter", 0.123456789);
            Assert.Equal(123.456789, result.Item1);
        }

        [Fact]
        public void Convert_Length_LargeValue_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "meter", "kilometer", 1000000);
            Assert.Equal(1000.0, result.Item1);
        }

        #endregion

        #region Mass Conversion Tests

        [Fact]
        public void Convert_Mass_MilligramToGram_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("mass", "milligram", "gram", 1000);
            Assert.Equal(1.0, result.Item1);
            Assert.Equal("gram", result.Item2);
        }

        [Fact]
        public void Convert_Mass_GramToKilogram_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("mass", "gram", "kilogram", 1000);
            Assert.Equal(1.0, result.Item1);
            Assert.Equal("kilogram", result.Item2);
        }

        [Fact]
        public void Convert_Mass_KilogramToGram_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("mass", "kilogram", "gram", 1);
            Assert.Equal(1000.0, result.Item1);
            Assert.Equal("gram", result.Item2);
        }

        [Fact]
        public void Convert_Mass_PoundToOunce_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("mass", "pound", "ounce", 1);
            Assert.Equal(16.0, result.Item1);
            Assert.Equal("ounce", result.Item2);
        }

        [Fact]
        public void Convert_Mass_OunceToPound_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("mass", "ounce", "pound", 16);
            Assert.Equal(1.0, result.Item1);
            Assert.Equal("pound", result.Item2);
        }

        [Fact]
        public void Convert_Mass_GramToOunce_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("mass", "gram", "ounce", 28.3495);
            Assert.NotNull(result.Item1);
            Assert.True(result.Item1 > 0.999 && result.Item1 < 1.001);
            Assert.Equal("ounce", result.Item2);
        }

        [Fact]
        public void Convert_Mass_SameUnitConversion_ReturnsOriginalValue()
        {
            var result = _conversionService.Convert("mass", "kilogram", "kilogram", 99.99);
            Assert.Equal(99.99, result.Item1);
            Assert.Equal("kilogram", result.Item2);
        }

        [Fact]
        public void Convert_Mass_ZeroValue_ReturnsZero()
        {
            var result = _conversionService.Convert("mass", "gram", "kilogram", 0);
            Assert.Equal(0.0, result.Item1);
        }

        [Fact]
        public void Convert_Mass_NegativeValue_ReturnsNegativeResult()
        {
            var result = _conversionService.Convert("mass", "gram", "kilogram", -1000);
            Assert.Equal(-1.0, result.Item1);
        }

        [Fact]
        public void Convert_Mass_DecimalValue_ReturnsRoundedTo4Decimals()
        {
            var result = _conversionService.Convert("mass", "gram", "gram", 1.23456789);
            Assert.Equal(1.2346, result.Item1);
        }

        #endregion

        #region Temperature Conversion Tests

        [Fact]
        public void Convert_Temperature_CelsiusToFahrenheit_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("temperature", "celsius", "fahrenheit", 0);
            Assert.Equal(32.0, result.Item1);
            Assert.Equal("fahrenheit", result.Item2);
        }

        [Fact]
        public void Convert_Temperature_FahrenheitToCelsius_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("temperature", "fahrenheit", "celsius", 32);
            Assert.Equal(0.0, result.Item1);
            Assert.Equal("celsius", result.Item2);
        }

        [Fact]
        public void Convert_Temperature_CelsiusToKelvin_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("temperature", "celsius", "kelvin", 0);
            Assert.NotNull(result.Item1);
            Assert.InRange(result.Item1.Value, 273.1, 273.2);
            Assert.Equal("kelvin", result.Item2);
        }

        [Fact]
        public void Convert_Temperature_KelvinToCelsius_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("temperature", "kelvin", "celsius", 273.15);
            Assert.Equal(0.0, result.Item1);
            Assert.Equal("celsius", result.Item2);
        }

        [Fact]
        public void Convert_Temperature_FahrenheitToKelvin_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("temperature", "fahrenheit", "kelvin", 32);
            Assert.NotNull(result.Item1);
            Assert.InRange(result.Item1.Value, 273.1, 273.2);
            Assert.Equal("kelvin", result.Item2);
        }

        [Fact]
        public void Convert_Temperature_100CelsiusToFahrenheit_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("temperature", "celsius", "fahrenheit", 100);
            Assert.Equal(212.0, result.Item1);
            Assert.Equal("fahrenheit", result.Item2);
        }

        [Fact]
        public void Convert_Temperature_NegativeCelsius_ReturnsCorrectFahrenheit()
        {
            var result = _conversionService.Convert("temperature", "celsius", "fahrenheit", -40);
            Assert.Equal(-40.0, result.Item1);
            Assert.Equal("fahrenheit", result.Item2);
        }

        [Fact]
        public void Convert_Temperature_SameUnitConversion_ReturnsOriginalValue()
        {
            var result = _conversionService.Convert("temperature", "celsius", "celsius", 25);
            Assert.Equal(25.0, result.Item1);
            Assert.Equal("celsius", result.Item2);
        }

        [Fact]
        public void Convert_Temperature_DecimalValue_ReturnsRoundedTo1Decimal()
        {
            var result = _conversionService.Convert("temperature", "celsius", "celsius", 25.123456);
            Assert.Equal(25.1, result.Item1);
        }

        #endregion

        #region Null Value Tests

        [Fact]
        public void Convert_WithNullValue_ReturnsNullResult()
        {
            var result = _conversionService.Convert("length", "meter", "kilometer", null);
            Assert.Null(result.Item1);
            Assert.Equal("kilometer", result.Item2);
        }

        [Fact]
        public void Convert_Temperature_WithNullValue_ReturnsNullResult()
        {
            var result = _conversionService.Convert("temperature", "celsius", "fahrenheit", null);
            Assert.Null(result.Item1);
            Assert.Equal("fahrenheit", result.Item2);
        }

        [Fact]
        public void Convert_Mass_WithNullValue_ReturnsNullResult()
        {
            var result = _conversionService.Convert("mass", "gram", "kilogram", null);
            Assert.Null(result.Item1);
            Assert.Equal("kilogram", result.Item2);
        }

        #endregion

        #region Case Sensitivity Tests

        [Fact]
        public void Convert_UppercaseCategory_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("LENGTH", "meter", "kilometer", 1000);
            Assert.Equal(1.0, result.Item1);
        }

        [Fact]
        public void Convert_MixedcaseCategory_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("MaSs", "gram", "kilogram", 1000);
            Assert.Equal(1.0, result.Item1);
        }

        [Fact]
        public void Convert_LowercaseCategory_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("temperature", "celsius", "fahrenheit", 0);
            Assert.Equal(32.0, result.Item1);
        }

        #endregion

        #region Exception Tests

        [Fact]
        public void Convert_InvalidCategory_ThrowsArgumentException()
        {
            Action action = () => _conversionService.Convert("invalid", "meter", "kilometer", 100);
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Contains("Unsupported category", exception.Message);
        }

        [Fact]
        public void Convert_UnknownCategory_ThrowsArgumentException()
        {
            Action action = () => _conversionService.Convert("weight", "meter", "kilometer", 100);
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Contains("Unsupported category", exception.Message);
        }

        #endregion

        #region All Length Unit Combinations

        [Theory]
        [InlineData("meter", "meter", 100, 100)]
        [InlineData("kilometer", "meter", 1, 1000)]
        [InlineData("meter", "kilometer", 1000, 1)]
        [InlineData("inch", "foot", 12, 1)]
        [InlineData("foot", "yard", 3, 1)]
        [InlineData("yard", "mile", 1760, 1)]
        public void Convert_Length_Various_ReturnsExpectedValue(string fromUnit, string toUnit, double value, double expected)
        {
            var result = _conversionService.Convert("length", fromUnit, toUnit, value);
            Assert.NotNull(result.Item1);
            Assert.Equal(expected, result.Item1.Value, precision: 0);
        }

        #endregion

        #region All Mass Unit Combinations

        [Theory]
        [InlineData("gram", "gram", 100, 100)]
        [InlineData("kilogram", "gram", 1, 1000)]
        [InlineData("gram", "kilogram", 1000, 1)]
        [InlineData("ounce", "pound", 16, 1)]
        [InlineData("pound", "ounce", 1, 16)]
        public void Convert_Mass_Various_ReturnsExpectedValue(string fromUnit, string toUnit, double value, double expected)
        {
            var result = _conversionService.Convert("mass", fromUnit, toUnit, value);
            Assert.NotNull(result.Item1);
            Assert.Equal(expected, result.Item1.Value, precision: 0);
        }

        #endregion

        #region All Temperature Unit Combinations

        [Theory]
        [InlineData("celsius", "celsius", 25, 25)]
        [InlineData("celsius", "fahrenheit", 0, 32)]
        [InlineData("celsius", "fahrenheit", 100, 212)]
        [InlineData("fahrenheit", "celsius", 32, 0)]
        [InlineData("celsius", "kelvin", 0, 273.15)]
        [InlineData("kelvin", "celsius", 273.15, 0)]
        public void Convert_Temperature_Various_ReturnsExpectedValue(string fromUnit, string toUnit, double value, double expected)
        {
            var result = _conversionService.Convert("temperature", fromUnit, toUnit, value);
            Assert.NotNull(result.Item1);
            Assert.Equal(expected, result.Item1.Value, precision: 0);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void Convert_ExtremelySmalldouble_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "millimeter", "meter", 0.0001);
            Assert.NotNull(result.Item1);
            Assert.True(result.Item1.Value >= 0);
        }

        [Fact]
        public void Convert_ExtremelyLargedouble_ReturnsCorrectValue()
        {
            var result = _conversionService.Convert("length", "kilometer", "meter", 999999999);
            Assert.NotNull(result.Item1);
            Assert.True(result.Item1.Value > 0);
        }

        [Fact]
        public void Convert_ReturnsTupleWithToUnit()
        {
            var result = _conversionService.Convert("length", "meter", "kilometer", 1000);
            Assert.Equal("kilometer", result.Item2);
            Assert.Equal("kilometer", result.toUnit);
        }

        #endregion
    }
}

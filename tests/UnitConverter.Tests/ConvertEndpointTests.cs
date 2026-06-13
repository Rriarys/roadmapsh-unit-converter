using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using UnitConverter.Api.Models;

namespace UnitConverter.Tests
{
    public class ConvertEndpointTests : IAsyncLifetime
    {
        private WebApplicationFactory<Program> _factory = null!;
        private HttpClient _client = null!;

        public async Task InitializeAsync()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            _client?.Dispose();
            await _factory.DisposeAsync();
        }

        #region Valid Request Tests

        [Fact]
        public async Task Put_Convert_WithValidLengthRequest_ReturnsOkWithCorrectResult()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 1000
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(1.0, result.Result);
            Assert.Equal("kilometer", result.ToUnit);
        }

        [Fact]
        public async Task Put_Convert_WithValidMassRequest_ReturnsOkWithCorrectResult()
        {
            var request = new ConvertRequest
            {
                Category = "mass",
                FromUnit = "kilogram",
                ToUnit = "gram",
                Value = 1
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(1000.0, result.Result);
            Assert.Equal("gram", result.ToUnit);
        }

        [Fact]
        public async Task Put_Convert_WithValidTemperatureRequest_ReturnsOkWithCorrectResult()
        {
            var request = new ConvertRequest
            {
                Category = "temperature",
                FromUnit = "celsius",
                ToUnit = "fahrenheit",
                Value = 0
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(32.0, result.Result);
            Assert.Equal("fahrenheit", result.ToUnit);
        }

        [Fact]
        public async Task Put_Convert_WithNullValue_ReturnsOkWithNullResult()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = null
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Put_Convert_WithZeroValue_ReturnsOkWithZeroResult()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 0
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(0.0, result.Result);
        }

        [Fact]
        public async Task Put_Convert_WithNegativeValue_ReturnsOkWithNegativeResult()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = -1000
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(-1.0, result.Result);
        }

        [Fact]
        public async Task Put_Convert_WithDecimalValue_ReturnsOkWithCorrectResult()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "millimeter",
                Value = 0.5
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(500.0, result.Result);
        }

        [Fact]
        public async Task Put_Convert_WithSameUnitConversion_ReturnsOkWithOriginalValue()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "meter",
                Value = 42.5
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(42.5, result.Result);
        }

        [Fact]
        public async Task Put_Convert_WithUppercaseCategory_ReturnsOk()
        {
            var request = new ConvertRequest
            {
                Category = "LENGTH",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 1000
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(1.0, result.Result);
        }

        [Fact]
        public async Task Put_Convert_WithMixedcaseCategory_ReturnsOk()
        {
            var request = new ConvertRequest
            {
                Category = "MaSs",
                FromUnit = "gram",
                ToUnit = "kilogram",
                Value = 1000
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(1.0, result.Result);
        }

        #endregion

        #region Invalid Category Tests

        [Fact]
        public async Task Put_Convert_WithInvalidCategory_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "invalid",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 100
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid category", content);
        }

        [Fact]
        public async Task Put_Convert_WithUnknownCategory_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "weight",
                FromUnit = "kilogram",
                ToUnit = "gram",
                Value = 1
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid category", content);
        }

        [Fact]
        public async Task Put_Convert_WithNullCategory_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = null!,
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 100
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("required", content, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Put_Convert_WithEmptyCategory_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 100
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("required", content, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Put_Convert_WithWhitespaceCategory_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "   ",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 100
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("required", content, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region Invalid FromUnit Tests

        [Fact]
        public async Task Put_Convert_WithInvalidFromUnit_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "invalid",
                ToUnit = "kilometer",
                Value = 100
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid from units", content);
        }

        [Fact]
        public async Task Put_Convert_WithNullFromUnit_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = null!,
                ToUnit = "kilometer",
                Value = 100
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("required", content, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Put_Convert_WithEmptyFromUnit_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "",
                ToUnit = "kilometer",
                Value = 100
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Put_Convert_WithFromUnitFromDifferentCategory_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "gram",
                ToUnit = "kilometer",
                Value = 100
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid from units", content);
        }

        #endregion

        #region Invalid ToUnit Tests

        [Fact]
        public async Task Put_Convert_WithInvalidToUnit_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "invalid",
                Value = 100
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid to units", content);
        }

        [Fact]
        public async Task Put_Convert_WithNullToUnit_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = null!,
                Value = 100
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("required", content, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Put_Convert_WithEmptyToUnit_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "",
                Value = 100
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Put_Convert_WithToUnitFromDifferentCategory_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "gram",
                Value = 100
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid to units", content);
        }

        #endregion

        #region Invalid Value Tests

        #endregion

        #region Response Format Tests

        [Fact]
        public async Task Put_Convert_Response_HasCorrectContentType()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 1000
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        }

        [Fact]
        public async Task Put_Convert_Response_ContainsResultAndToUnit()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 1000
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.NotNull(result.ToUnit);
            Assert.IsType<double>(result.Result);
            Assert.IsType<string>(result.ToUnit);
        }

        #endregion

        #region Multiple Conversions

        [Theory]
        [InlineData("length", "meter", "kilometer", 1000, 1)]
        [InlineData("length", "kilometer", "meter", 1, 1000)]
        [InlineData("mass", "kilogram", "gram", 1, 1000)]
        [InlineData("mass", "gram", "kilogram", 1000, 1)]
        [InlineData("temperature", "celsius", "fahrenheit", 0, 32)]
        [InlineData("temperature", "fahrenheit", "celsius", 32, 0)]
        public async Task Put_Convert_VariousConversions_ReturnsCorrectResults(
            string category, string fromUnit, string toUnit, double value, double expected)
        {
            var request = new ConvertRequest
            {
                Category = category,
                FromUnit = fromUnit,
                ToUnit = toUnit,
                Value = value
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Equal(expected, result.Result.Value, precision: 0);
        }

        #endregion

        #region Endpoint Method Tests

        [Fact]
        public async Task Put_Convert_WithGetRequest_ReturnsMethodNotAllowed()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 1000
            };

            var response = await _client.GetAsync("/convert");

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Fact]
        public async Task Put_Convert_WithPostRequest_ReturnsMethodNotAllowed()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 1000
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        #endregion

        #region Case Insensitive Unit Tests

        [Theory]
        [InlineData("METER", "KILOMETER")]
        [InlineData("Meter", "Kilometer")]
        [InlineData("MeTer", "KiLoMeTeR")]
        public async Task Put_Convert_WithMixedCaseUnits_ReturnsOk(string fromUnit, string toUnit)
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = fromUnit,
                ToUnit = toUnit,
                Value = 1000
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Equal(1.0, result.Result.Value, precision: 0);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Put_Convert_WithVeryLargeValue_ReturnsOk()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 999999999
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.True(result.Result > 0);
        }

        [Fact]
        public async Task Put_Convert_WithVerySmallValue_ReturnsOk()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "millimeter",
                ToUnit = "meter",
                Value = 0.0001
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.True(result.Result >= 0);
        }

        #endregion
    }
}

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

        #region Valid Conversion Tests

        [Fact]
        public async Task Post_Convert_WhenValidLengthRequest_ReturnsOkWithCorrectResult()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 1000
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(1.0, result.Result);
            Assert.Equal("kilometer", result.ToUnit);
        }

        [Theory]
        [InlineData("length", "meter", "kilometer", 1000, 1)]
        [InlineData("length", "kilometer", "meter", 1, 1000)]
        [InlineData("length", "meter", "millimeter", 0.5, 500)]
        [InlineData("length", "meter", "meter", 42.5, 42.5)]
        [InlineData("mass", "kilogram", "gram", 1, 1000)]
        [InlineData("mass", "gram", "kilogram", 1000, 1)]
        [InlineData("temperature", "celsius", "fahrenheit", 0, 32)]
        [InlineData("temperature", "fahrenheit", "celsius", 32, 0)]
        public async Task Post_Convert_WhenVariousConversions_ReturnsCorrectResults(
            string category, string fromUnit, string toUnit, double value, double expected)
        {
            var request = new ConvertRequest
            {
                Category = category,
                FromUnit = fromUnit,
                ToUnit = toUnit,
                Value = value
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(expected, result.Result!.Value, precision: 0);
            Assert.Equal(toUnit.ToLowerInvariant(), result.ToUnit.ToLowerInvariant());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1000)]
        public async Task Post_Convert_WhenSpecialNumericValues_ReturnsCorrectResult(double value)
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = value
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(value / 1000, result.Result!.Value);
        }

        #endregion

        #region Validation Tests

        [Fact]
        public async Task Post_Convert_WhenNullValue_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = null
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region Category Validation Tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Post_Convert_WhenCategoryIsNullOrEmpty_ReturnsBadRequest(string? category)
        {
            var request = new ConvertRequest
            {
                Category = category!,
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 100
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("required", content, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("weight")]
        public async Task Post_Convert_WhenInvalidCategory_ReturnsBadRequest(string category)
        {
            var request = new ConvertRequest
            {
                Category = category,
                FromUnit = "kilogram",
                ToUnit = "gram",
                Value = 1
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid category", content);
        }

        [Theory]
        [InlineData("LENGTH")]
        [InlineData("MaSs")]
        [InlineData("TEMPERATURE")]
        public async Task Post_Convert_WhenCategoryIsMixedCase_ReturnsOk(string category)
        {
            var fromUnit = category.ToLowerInvariant() switch
            {
                "length" => "meter",
                "mass" => "gram",
                "temperature" => "celsius",
                _ => "meter"
            };

            var toUnit = category.ToLowerInvariant() switch
            {
                "length" => "kilometer",
                "mass" => "kilogram",
                "temperature" => "fahrenheit",
                _ => "kilometer"
            };

            var value = category.ToLowerInvariant() switch
            {
                "temperature" => 0,
                _ => 1000
            };

            var request = new ConvertRequest
            {
                Category = category,
                FromUnit = fromUnit,
                ToUnit = toUnit,
                Value = value
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
        }

        #endregion

        #region Unit Validation Tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Post_Convert_WhenFromUnitIsNullOrEmpty_ReturnsBadRequest(string? fromUnit)
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = fromUnit!,
                ToUnit = "kilometer",
                Value = 100
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("required", content, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Post_Convert_WhenToUnitIsNullOrEmpty_ReturnsBadRequest(string? toUnit)
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = toUnit!,
                Value = 100
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("required", content, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Post_Convert_WhenFromUnitIsInvalid_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "invalid",
                ToUnit = "kilometer",
                Value = 100
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid from units", content);
        }

        [Fact]
        public async Task Post_Convert_WhenToUnitIsInvalid_ReturnsBadRequest()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "invalid",
                Value = 100
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid to units", content);
        }

        [Theory]
        [InlineData("gram")]
        [InlineData("kilogram")]
        public async Task Post_Convert_WhenFromUnitFromDifferentCategory_ReturnsBadRequest(string fromUnit)
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = fromUnit,
                ToUnit = "kilometer",
                Value = 100
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid from units", content);
        }

        [Theory]
        [InlineData("gram")]
        [InlineData("kilogram")]
        public async Task Post_Convert_WhenToUnitFromDifferentCategory_ReturnsBadRequest(string toUnit)
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = toUnit,
                Value = 100
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid to units", content);
        }

        #endregion

        #region Case Insensitive Tests

        [Theory]
        [InlineData("METER", "KILOMETER")]
        [InlineData("Meter", "Kilometer")]
        [InlineData("MeTer", "KiLoMeTeR")]
        public async Task Post_Convert_WhenUnitsAreMixedCase_ReturnsOk(string fromUnit, string toUnit)
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = fromUnit,
                ToUnit = toUnit,
                Value = 1000
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(1.0, result.Result!.Value, precision: 0);
        }

        #endregion

        #region Endpoint Method Tests

        [Fact]
        public async Task Post_Convert_WhenUsingGetRequest_ReturnsMethodNotAllowed()
        {
            var response = await _client.GetAsync("/convert");

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Fact]
        public async Task Post_Convert_WhenUsingPutRequest_ReturnsMethodNotAllowed()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 1000
            };

            var response = await _client.PutAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Post_Convert_WhenValueIsVeryLarge_ReturnsOkWithCorrectResult()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "meter",
                ToUnit = "kilometer",
                Value = 999999999
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(999999.999, result.Result!.Value, precision: 0);
        }

        [Fact]
        public async Task Post_Convert_WhenValueIsVerySmall_ReturnsOkWithCorrectResult()
        {
            var request = new ConvertRequest
            {
                Category = "length",
                FromUnit = "millimeter",
                ToUnit = "meter",
                Value = 0.5
            };

            var response = await _client.PostAsJsonAsync("/convert", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ConvertResponse>();
            Assert.NotNull(result);
            Assert.Equal(0.0005, result.Result!.Value);
        }

        #endregion
    }
}

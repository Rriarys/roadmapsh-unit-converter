using UnitConverter.Api.Conversion;
using UnitConverter.Api.Extensions;
using UnitConverter.Api.Models;
using UnitConverter.Api.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton<ConversionService>();

var app = builder.Build();

// Custom exception handling middleware
app.UseCustomExceptionHandler();

// POST
app.MapPost("/convert", (ConvertRequest request, ConversionService conversionService) =>
{
    var (isValid, error) = ConvertRequestValidator.Validate(request);
    if (!isValid)
    {
        return Results.BadRequest(error);
    }

    var (result, toUnit) = conversionService.Convert(request.Category, request.FromUnit, request.ToUnit, request.Value);


    var response = new ConvertResponse
    {
        Result = result,
        ToUnit = toUnit
    };

    return Results.Ok(response);
});

app.Run();
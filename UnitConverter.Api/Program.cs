using UnitConverter.Api.Extensions;
using UnitConverter.Api.Models;
using UnitConverter.Api.Validation;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Custom exception handling middleware
app.UseCustomExceptionHandler();

// PUT mock
app.MapPut("/convert", (ConvertRequest request) =>
{
    var (isValid, error) = ConvertRequestValidator.Validate(request);
    if (!isValid)
    {
        return Results.BadRequest(error);
    }

    return Results.Ok(request);
});

app.Run();
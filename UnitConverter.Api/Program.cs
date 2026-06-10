var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

// PUT mock
app.MapPut("/convert", () =>
{
    return Results.Ok("PUT mocked");
});

app.Run();
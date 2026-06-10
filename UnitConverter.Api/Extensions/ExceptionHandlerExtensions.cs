using Microsoft.AspNetCore.Diagnostics;

namespace UnitConverter.Api.Extensions;

// Custom exception handler that logs clean messages and returns minimal JSON without RFC 9110
public static class ExceptionHandlerExtensions
{
    // Configures a clean custom exception handler without RFC bloat
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            // Suppresses the default massive stack trace log for bad requests
            SuppressDiagnosticsCallback = context => context.Exception is BadHttpRequestException,

            ExceptionHandler = async context =>
            {
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionFeature?.Error is null)
                {
                    return;
                }

                var exception = exceptionFeature.Error;
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

                context.Response.ContentType = "application/json";

                // Handles bad request exceptions with a clean message and minimal JSON response into console logs and client response
                if (exception is BadHttpRequestException badRequestException)
                {
                    // Extracts the base message
                    var rawMessage = badRequestException.InnerException?.Message ?? badRequestException.Message;

                    // Cuts off the technical JSON path details if they exist
                    var pathSeparatorIndex = rawMessage.IndexOf(" Path: ");
                    var cleanMessage = pathSeparatorIndex > 0
                        ? rawMessage.Substring(0, pathSeparatorIndex)
                        : rawMessage;

                    logger.LogWarning("Bad request. Reason: {Message}", cleanMessage);

                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    // Returns a clean, minimal JSON response to the client
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Invalid request",
                        message = "Malformed JSON or invalid request body"
                    });
                }
                else
                {
                    logger.LogError(exception, "Unexpected server error occurred");

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Server error",
                        message = "An unexpected error occurred"
                    });
                }
            }
        });

        return app;
    }
}
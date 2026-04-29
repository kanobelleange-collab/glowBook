using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access detected during request execution.");
            await HandleUnauthorizedAsync(context, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception during request execution.");
            await HandleUnexpectedAsync(context);
        }
    }

    private static Task HandleUnauthorizedAsync(HttpContext context, string message)
    {
        context.Response.Clear();
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var payload = new
        {
            status = StatusCodes.Status401Unauthorized,
            title = "Unauthorized",
            detail = message
        };

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        return context.Response.WriteAsJsonAsync(payload, options);
    }

    private static Task HandleUnexpectedAsync(HttpContext context)
    {
        context.Response.Clear();
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var payload = new
        {
            status = StatusCodes.Status500InternalServerError,
            title = "Internal Server Error",
            detail = "Une erreur inattendue est survenue."
        };

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        return context.Response.WriteAsJsonAsync(payload, options);
    }
}

using DPBack.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Middleware;

public sealed class GlobalExceptionHandler

{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
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
        catch (OperationCanceledException)
        {
            context.Response.StatusCode = 499;
        }
        catch (Exception error)
        {
            _logger.LogWarning(error, "Unhandled exception");
            await HandleExceptionAsync(context, error);

        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception error)
    {
        var (statusCode, title) = error switch
        {
            ArgumentException => (StatusCodes.Status400BadRequest, "Bad request"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not found"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            StatusChangeNotAllowedException => (StatusCodes.Status409Conflict, "Status change not allowed"),
            _ => (StatusCodes.Status500InternalServerError, "Internal server error")
        };
        context.Response.StatusCode = statusCode;
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = statusCode,
            Type = error.GetType().Name,
            Detail = statusCode == 500 ? null : error.Message
        };
        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}


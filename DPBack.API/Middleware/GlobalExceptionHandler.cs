using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Middleware;

public sealed class GlobalExceptionHandler(RequestDelegate next)

{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception error)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Type = error.GetType().Name,
                    Title = "En error has occured",
                    Detail = error.Message
                });
            
        }
    }
}


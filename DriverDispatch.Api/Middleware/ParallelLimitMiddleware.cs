using DriverDispatch.Api.Options;
using Microsoft.Extensions.Options;

namespace DriverDispatch.Api.Middleware;

public class ParallelLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly int _limit;
    private static int _currentRequests;

    public ParallelLimitMiddleware(RequestDelegate next, IOptions<ServiceSettings> settings)
    {
        _next = next;
        _limit = settings.Value.ParallelLimit;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Не ограничиваем Swagger
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }

        int current = Interlocked.Increment(ref _currentRequests);

        try
        {
            if (current > _limit)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Service Unavailable");
                return;
            }

            await _next(context);
        }
        finally
        {
            Interlocked.Decrement(ref _currentRequests);
        }
    }
}
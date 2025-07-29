using Serilog.Context;

namespace Web.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ICorrelationIdContext _correlationIdContext;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public CorrelationIdMiddleware(ICorrelationIdContext correlationIdContext)
    {
        _correlationIdContext = correlationIdContext;
    }
    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                            ?? Guid.NewGuid().ToString();

        correlationId = _correlationIdContext?.CorrelationId ?? correlationId;


        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Infrastructure.Logging
{
    public class LoggingFilter : ILoggingFilter
    {
        private readonly ILoggerManager _logger;
        private readonly ICorrelationIdAccessor _correlation;

        public LoggingFilter(ILoggerManager logger, ICorrelationIdAccessor correlation)
        {
            _logger = logger;
            _correlation = correlation;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var controller = context.Controller.GetType().Name;
            var action = context.ActionDescriptor.DisplayName;
            var route = httpContext.Request.Path;

            var correlationId = Guid.NewGuid().ToString();
            correlationId = _correlation.GetCorrelationId() ?? correlationId;

            var user = httpContext.User.Identity?.Name ?? "Anonymous";

            // Capture headers
            var headers = httpContext.Request.Headers
                .Select(h => $"{h.Key}: {h.Value}")
                .ToList();

            // Start timing
            var stopwatch = Stopwatch.StartNew();

            _logger.Info("➡️ Request started | {Controller}.{Action} | Route: {Route} | User: {User} | CorrelationId: {CorrelationId} | Headers: {@Headers}",
                         controller, action, route, user, correlationId, headers);

            var resultContext = await next();

            stopwatch.Stop();

            _logger.Info("✅ Request completed | {Controller}.{Action} | StatusCode: {StatusCode} | Duration: {ElapsedMs}ms | CorrelationId: {CorrelationId}",
                         controller, action, resultContext.HttpContext.Response.StatusCode, stopwatch.ElapsedMilliseconds, correlationId);
        }
    }

}

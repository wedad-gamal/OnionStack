using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text;

namespace MVC.Presentation.Middlewares;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerManager _loggerManager;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerManager loggerManager)
    {
        _next = next;
        _loggerManager = loggerManager;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var request = context.Request;

        request.EnableBuffering();

        string body = "";
        if (request.Method == HttpMethods.Post)
        {
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        var queryParams = request.QueryString.HasValue ? request.QueryString.Value : "";
        _loggerManager.Info("➡️ HTTP Request: {Method} {Path}{Query} | CorrelationId: {CorrelationId}",
            request.Method,
            request.Path,
            queryParams,
            context.Items["X-Correlation-ID"]);


        try
        {
            await _next(context);
            stopwatch.Stop();


            var response = context.Response;

            // Log Response Info
            _loggerManager.Info("⬅️ HTTP Response: {StatusCode} | Duration: {Elapsed} ms",
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);

        }
        catch (Exception ex)
        {

            stopwatch.Stop();

            _loggerManager.Error("🔥 Exception during HTTP Request {Method} {Path}. | CorrelationId: {CorrelationId} | Took {Elapsed} ms {ex}",
                request.Method,
                request.Path,
                context.Items["X-Correlation-ID"],
                stopwatch.ElapsedMilliseconds,
                ex);

            throw; // Re-throw to preserve pipeline behavior
        }
    }
}

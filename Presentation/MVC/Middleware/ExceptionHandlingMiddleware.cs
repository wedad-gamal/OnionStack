namespace MVC.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IErrorLogService errorLogService, ILoggerManager loggerManager)
    {
        try
        {
            await _next(context); // continue pipeline
        }
        catch (Exception ex)
        {
            loggerManager.Error("An unhandled exception occurred: {Message}", ex.Message);

            // save to DB (via service from Application layer)
            await errorLogService.LogAsync(ex, context);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;


            context.Response.Redirect("/Home/Error");

        }
    }
}

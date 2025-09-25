using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Logging;
public class ErrorLogService : IErrorLogService
{
    private readonly ApplicationDbContext _db;
    private readonly ICorrelationContextAccessor _correlationContextAccessor;
    private readonly ILoggerManager _loggerManager;

    public ErrorLogService(ApplicationDbContext db, ICorrelationContextAccessor correlationContextAccessor, ILoggerManager loggerManager)
    {
        _db = db;
        _correlationContextAccessor = correlationContextAccessor;
        _loggerManager = loggerManager;
    }

    public async Task LogAsync(Exception ex, HttpContext context, string correlationId = "")
    {
        try
        {
            var error = new ErrorLog
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Path = context != null ? context.Request.Path : ex.StackTrace,
                InnerException = $"{ex.InnerException.Message} {ex.InnerException.InnerException}",
                CorrelationId = _correlationContextAccessor?.CorrelationContext.CorrelationId ?? correlationId
            };

            _db.Set<ErrorLog>().Add(error);
            await _db.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            _loggerManager.Error("An error has occured. {exception}", $"{ex.InnerException.Message} {ex.InnerException.InnerException}");

            throw;
        }

    }
}


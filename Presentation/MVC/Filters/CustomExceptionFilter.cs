using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVC.Filters;

public class CustomExceptionFilter : IExceptionFilter
{
    private readonly ILoggerManager _loggerManager;
    private readonly IErrorLogService _errorLogService;
    private readonly ICorrelationContextAccessor _correlation;

    public CustomExceptionFilter(ILoggerManager loggerManager, IErrorLogService errorLogService, ICorrelationContextAccessor correlation)
    {
        _loggerManager = loggerManager;
        _errorLogService = errorLogService;
        _correlation = correlation;
    }

    public void OnException(ExceptionContext context)
    {
        _loggerManager.Error("An Error Has Occured. {Message}", $"{context.Exception.Message} {context.Exception.InnerException}");
        _errorLogService.LogAsync(context.Exception, null, _correlation?.CorrelationContext.CorrelationId);
        context.Result = new ViewResult
        {
            ViewName = "Error"
        };
        context.ExceptionHandled = true;
    }
}
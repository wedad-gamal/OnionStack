using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Abstractions.Logging
{
    public interface ILoggingFilter : IFilterMetadata
    {
        Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next);
    }
}

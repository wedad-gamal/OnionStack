using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Common.Interfaces.Logging
{
    public interface ILoggingFilter : IFilterMetadata
    {
        Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next);
    }
}

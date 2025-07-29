
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Logging
{
    public interface ILoggingFilter : IFilterMetadata
    {
        Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next);
    }
}

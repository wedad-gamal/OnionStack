using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVC.Apis.Attributes;

public class ValidateCsrfTokenAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var antiforgery = context.HttpContext.RequestServices.GetService<IAntiforgery>();
        if (antiforgery == null)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return;
        }

        var isValid = await antiforgery.IsRequestValidAsync(context.HttpContext);
        if (!isValid)
        {
            context.Result = new BadRequestObjectResult("Invalid or missing CSRF token.");
        }
    }
}

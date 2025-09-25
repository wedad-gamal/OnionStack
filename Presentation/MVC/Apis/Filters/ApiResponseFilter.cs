using Microsoft.AspNetCore.Mvc.Filters;
using MVC.Apis.Common;

namespace MVC.Apis.Filters;


public class ApiResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Do nothing before action executes
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
        {
            // handle unhandled exceptions
            var error = ApiResponse<string>.Fail(context.Exception.Message);
            context.Result = new ObjectResult(error)
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
            return;
        }

        if (context.Result is ObjectResult objectResult)
        {
            // If already an ApiResponse, leave it
            if (objectResult.Value is IApiResponseMarker) return;

            // Otherwise wrap it
            var wrapped = ApiResponse<object>.Ok(objectResult.Value, "Success");
            context.Result = new ObjectResult(wrapped)
            {
                StatusCode = objectResult.StatusCode
            };
        }
        else if (context.Result is EmptyResult)
        {
            context.Result = new ObjectResult(ApiResponse<string>.Ok(null, "No content"))
            {
                StatusCode = 204
            };
        }
    }
}
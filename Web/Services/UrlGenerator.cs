

namespace Web.Services;

public class UrlGenerator : IUrlGenerator
{
    private readonly IUrlHelper _urlHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UrlGenerator(IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _urlHelper = urlHelperFactory.GetUrlHelper(new ActionContext(
            httpContextAccessor.HttpContext,
            httpContextAccessor.HttpContext.GetRouteData(),
            new ActionDescriptor()));
    }

    public string GenerateUrl(string email, string token, string action = "ResetPassword", string controller = "Account")
    {
        return _urlHelper.Action(action, controller, new { email, token }, _httpContextAccessor.HttpContext.Request.Scheme);
    }
}

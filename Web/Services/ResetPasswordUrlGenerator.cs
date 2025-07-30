

namespace Web.Services;

public class ResetPasswordUrlGenerator : IResetPasswordUrlGenerator
{
    private readonly IUrlHelper _urlHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ResetPasswordUrlGenerator(IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _urlHelper = urlHelperFactory.GetUrlHelper(new ActionContext(
            httpContextAccessor.HttpContext,
            httpContextAccessor.HttpContext.GetRouteData(),
            new ActionDescriptor()));
    }

    public string GenerateUrl(string email, string token)
    {
        return _urlHelper.Action("ResetPassword", "Account", new { email, token }, _httpContextAccessor.HttpContext.Request.Scheme);
    }
}

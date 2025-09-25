using Microsoft.AspNetCore.Antiforgery;

namespace MVC.Apis.Common;

[ApiController]
[Route("api/[controller]")]
public class SecurityController : ControllerBase
{
    private readonly IAntiforgery _antiforgery;

    public SecurityController(IAntiforgery antiforgery)
    {
        _antiforgery = antiforgery;
    }

    [HttpGet("antiforgery-token")]
    public IActionResult GetAntiforgeryToken()
    {
        var tokens = _antiforgery.GetAndStoreTokens(HttpContext);

        // Optional: return token in body for debugging or manual use
        return Ok(new { token = tokens.RequestToken });
    }
}
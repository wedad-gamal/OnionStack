using System.Security.Claims;

namespace Application.DTOs.Identity
{
    public class ExternalLoginInfoDto
    {
        public ClaimsPrincipal? Principal { get; set; }
        public string? LoginProvider { get; set; }
        public string? ProviderKey { get; set; }
        public string? ProviderDisplayName { get; set; }
        public string? DisplayName { get; set; }
    }
}

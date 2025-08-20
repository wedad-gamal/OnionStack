namespace Application.DTOs.Identity
{
    public class ExternalLoginInfoDto
    {
        public string LoginProvider { get; set; } = string.Empty;
        public string ProviderKey { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? ProviderDisplayName { get; set; }
    }
}

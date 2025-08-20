namespace Application.DTOs.Identity
{
    public class AuthenticationPropertiesDto
    {
        public IDictionary<string, string?> Items { get; set; } = new Dictionary<string, string?>();
        public bool AllowRefresh { get; set; }
        public DateTimeOffset? ExpiresUtc { get; set; }
        public DateTimeOffset? IssuedUtc { get; set; }
        public bool IsPersistent { get; set; }
        public string? RedirectUri { get; set; }
    }
}

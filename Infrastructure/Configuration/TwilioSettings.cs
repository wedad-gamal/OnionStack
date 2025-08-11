namespace Infrastructure.Configuration
{
    public class TwilioSettings
    {
        public string AccountSid { get; set; } = default!;
        public string AuthToken { get; set; } = default!;
        public string FromNumber { get; set; } = default!;
        public string FromWhatsAppNumber { get; set; } = default!;
    }
}

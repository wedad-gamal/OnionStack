namespace Application.DTOs.Common
{
    public class WhatsAppMessageDto
    {
        public string ToPhoneNumber { get; set; } = default!;
        public string Message { get; set; } = default!;
    }
}

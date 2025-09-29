

namespace Infrastructure.Services
{
    public class WhatsAppService : ISmsService
    {
        private const string WhatsAppPrefix = "whatsapp:";

        private readonly TwilioSettings _twilioSettings;
        public WhatsAppService(IOptions<TwilioSettings> twilioSettings)
        {
            _twilioSettings = twilioSettings.Value;
            TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
        }
        public async Task SendMessageAsync(MessageDto messageDto)
        {
            var fromWhatsAppNumber = $"{WhatsAppPrefix}{_twilioSettings.FromWhatsAppNumber}";
            var toWhatsAppNumber = $"{WhatsAppPrefix}{messageDto.ToPhoneNumber}";
            var result = await MessageResource.CreateAsync(
                body: messageDto.Message,
                from: new Twilio.Types.PhoneNumber(fromWhatsAppNumber),
                to: new Twilio.Types.PhoneNumber(toWhatsAppNumber)
            );
        }
    }

}

using Application.Common.Interfaces.Logging;
using Application.Common.Interfaces.Services;

namespace Infrastructure.Services
{
    public class SmsService : ISmsService
    {
        private readonly TwilioSettings _options;
        private readonly ILoggerManager _loggerManager;

        public SmsService(IOptions<TwilioSettings> options, ILoggerManager loggerManager)
        {
            _options = options.Value;
            _loggerManager = loggerManager;
        }
        public async Task SendMessageAsync(MessageDto messageDto)
        {
            try
            {
                TwilioClient.Init(_options.AccountSid, _options.AuthToken);
                var result = MessageResource.Create(
                    body: messageDto.Message,
                    to: messageDto.ToPhoneNumber,
                    from: new Twilio.Types.PhoneNumber(_options.FromNumber)
                    );

            }
            catch (Exception ex)
            {
                _loggerManager.Error("Error sending SMS: {Message}", ex.Message);
                throw;
            }
        }
    }
}

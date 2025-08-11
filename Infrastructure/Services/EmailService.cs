

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILoggerManager _loggerManager;

        public EmailService(IOptions<EmailSettings> settings, ILoggerManager loggerManager)
        {
            _settings = settings.Value;
            _loggerManager = loggerManager;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(to) || !MailboxAddress.TryParse(to, out _))
                    throw new ArgumentException("Invalid recipient email address", nameof(to));

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;

                var builder = new BodyBuilder();
                if (isHtml)
                    builder.HtmlBody = body;
                else
                    builder.TextBody = body;
                message.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _loggerManager.Error("An Exception has occurred while sending email: {ex}", ex);
                throw;
            }
        }
    }
}

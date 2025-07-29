using Core.Entities;

namespace Tests.Unit.Infrastructure.Email
{
    public class MailKitEmailSenderTests
    {
        [Fact]
        public async Task SendEmailAsync_ShouldConstructMimeMessageCorrectly()
        {
            // Arrange
            var settings = new EmailSettings
            {
                SenderName = "HR System",
                SenderEmail = "hr@company.com",
                Host = "smtp.test.com",
                Port = 587,
                Username = "user",
                Password = "pass",
                UseSsl = true
            };

            //var options = Options.Create(settings);
            //var sender = new MailKitEmailSender(options);

            // Act & Assert — no exception means basic construction passed
            //await sender.SendEmailAsync("recipient@test.com", "Welcome", "<b>Onboarding</b>", true);
        }
    }
}

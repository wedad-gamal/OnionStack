

namespace Infrastructure.IntegrationTests.Background
{
    public class EmailJobTests
    {
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ILoggerManager> _loggerManagerMock;
        private readonly EmailJob _emailJob;

        public EmailJobTests()
        {
            _emailServiceMock = new Mock<IEmailService>();
            _loggerManagerMock = new Mock<ILoggerManager>();
            _emailJob = new EmailJob(_emailServiceMock.Object, _loggerManagerMock.Object);
        }

        [Fact]
        public async Task SendRoleChangedEmailAsync_ShouldCallEmailServiceWithCorrectParams()
        {
            // Arrange
            var email = "test@gmail.com";
            var roleName = "Admin";
            var isAssigned = true;
            // Act
            await _emailJob.SendRoleChangedEmailAsync(email, roleName, isAssigned);

            // Assert
            _emailServiceMock.Verify(
                service => service.SendEmailAsync(
                    It.Is<string>(to => to == email),
                    It.Is<string>(subject => subject.Contains("Role Changed")),
                    It.Is<string>(body => body.Contains(roleName)), true
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task SendWelcomeEmailAsync_ShouldCallEmailServiceWithCorrectParams()
        {
            // Arrange
            var email = "test@gmail.com";

            // Act
            await _emailJob.SendWelcomeEmailAsync(email);

            // Assert
            _emailServiceMock.Verify(
                service => service.SendEmailAsync(
                    It.Is<string>(to => to == email),
                    It.Is<string>(subject => subject.Contains("Welcome")),
                    It.IsAny<string>(), true
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task SendPasswordResetEmailAsync_ShouldCallEmailServiceWithCorrectParams()
        {
            // Arrange
            var email = "test@example.com";
            var token = "reset-token";

            // Act
            await _emailJob.SendPasswordResetEmailAsync(email, token);

            // Assert
            _emailServiceMock.Verify(
                service => service.SendEmailAsync(
                    It.Is<string>(to => to == email),
                    It.Is<string>(subject => subject.Contains("Password Reset")),
                    It.Is<string>(body => body.Contains(token)), true
                ),
                Times.Once
            );
        }
        [Fact]
        public async Task SendRoleChangedEmailAsync_CallsEmailServiceWithUserEmail()
        {
            // Arrange
            var email = "test@gmail.com";
            var roleName = "Admin";
            var isAssigned = true;

            // Act
            await _emailJob.SendRoleChangedEmailAsync(email, roleName, isAssigned);

            // Assert
            _emailServiceMock.Verify(s =>
                s.SendEmailAsync("u@example.com",
                It.Is<string>(sub => sub.Contains("Role Changed")),
                It.Is<string>(b => b.Contains(roleName)), true),
                Times.Once);
        }

        [Fact]
        public async Task SendWelcomeEmailAsync_CallsEmailServiceWithUserEmail()
        {
            var email = "test@gmail.com";

            await _emailJob.SendWelcomeEmailAsync(email);

            _emailServiceMock.Verify(s =>
                s.SendEmailAsync(email, It.Is<string>(sub => sub.Contains("Welcome")), It.IsAny<string>(), true),
                Times.Once);
        }

        [Fact]
        public async Task SendPasswordResetEmailAsync_CallsEmailServiceWithToken()
        {
            var email = "t@example.com";
            var token = "tok";
            await _emailJob.SendPasswordResetEmailAsync(email, token);
            _emailServiceMock.Verify(s =>
                s.SendEmailAsync(email,
                It.Is<string>(sub => sub.Contains("Password Reset")),
                It.Is<string>(b => b.Contains(token)), true),
                Times.Once);
        }
    }
}

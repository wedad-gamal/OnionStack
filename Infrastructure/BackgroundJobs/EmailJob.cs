namespace Infrastructure.BackgroundJobs
{

    public class EmailJob : IEmailJob
    {
        private readonly IEmailService _emailService;
        private readonly ILoggerManager _loggerManager;

        public EmailJob(IEmailService emailService, ILoggerManager loggerManager)
        {
            _emailService = emailService;
            _loggerManager = loggerManager;
        }

        public async Task SendRoleChangedEmailAsync(string email, string newRole, bool isAssigned)
        {
            _loggerManager.Info("Sending role change email to {UserId}...", email);
            var message = !isAssigned ? $"Your role {newRole} has removed" : $"Your role is {newRole}";
            await _emailService.SendEmailAsync(email, "Role Changed", message);
        }
        public Task SendWelcomeEmailAsync(string email)
        {
            return _emailService.SendEmailAsync(email, "Welcome", "Thanks for joining our platform!");
        }

        public Task SendPasswordResetEmailAsync(string email, string token)
        {
            return _emailService.SendEmailAsync(email, "Password Reset", $"Use this token to reset your password: {token}");
        }
    }
}

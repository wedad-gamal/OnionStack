namespace Infrastructure.BackgroundJobs
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly IHangfireClient _hangfireClient;

        public BackgroundJobService(IHangfireClient hangfireClient)
        {
            _hangfireClient = hangfireClient ?? throw new ArgumentNullException(nameof(hangfireClient));
        }

        public void EnqueueSendRoleChangedEmail(string email, string roleName, bool isAssigned)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("email cannot be null or empty.", nameof(email));

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("RoleName cannot be null or empty.", nameof(roleName));

            _hangfireClient.Enqueue<IEmailJob>(job => job.SendRoleChangedEmailAsync(email, roleName, isAssigned));
        }

        public void EnqueueSendWelcomeEmail(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));

            _hangfireClient.Enqueue<IEmailJob>(job => job.SendWelcomeEmailAsync(userId));
        }

        public void EnqueueSendPasswordResetEmail(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));

            _hangfireClient.Enqueue<IEmailJob>(job => job.SendPasswordResetEmailAsync(email, token));
        }
    }
}

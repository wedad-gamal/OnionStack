namespace Application.Common.Interfaces.Background
{
    public interface IBackgroundJobService
    {
        void EnqueueSendRoleChangedEmail(string userId, string roleName, bool isAssigned);
        void EnqueueSendWelcomeEmail(string userId);
        void EnqueueSendPasswordResetEmail(string email, string token);
    }
}

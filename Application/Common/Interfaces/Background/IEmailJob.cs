namespace Application.Common.Interfaces.Background
{
    public interface IEmailJob
    {
        Task SendPasswordResetEmailAsync(string email, string token);
        Task SendRoleChangedEmailAsync(string email, string newRole, bool isAssigned);
        Task SendWelcomeEmailAsync(string email);
    }
}

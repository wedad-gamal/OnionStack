namespace Application.Common.Interfaces.Services
{
    public interface INotificationService
    {
        Task NotifyUserAsync(string userId, string message);
    }

}

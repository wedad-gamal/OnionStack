namespace Abstraction.Abstraction.Interfaces.Services
{
    public interface INotificationService
    {
        Task NotifyUserAsync(string userId, string message);
    }

}

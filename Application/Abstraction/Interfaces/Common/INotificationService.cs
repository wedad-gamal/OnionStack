namespace Application.Abstraction.Interfaces.Common
{
    public interface INotificationService
    {
        Task NotifyUserAsync(string userId, string message);
    }

}

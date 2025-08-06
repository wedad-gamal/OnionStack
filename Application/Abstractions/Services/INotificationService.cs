namespace Application.Abstractions.Services
{
    public interface INotificationService
    {
        Task NotifyUserAsync(string userId, string message);
    }

}

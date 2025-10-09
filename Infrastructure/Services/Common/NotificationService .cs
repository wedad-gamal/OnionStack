using Application.Abstraction.Interfaces.Common;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services.Common
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyUserAsync(string userId, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("Notify", message);
        }
    }

}

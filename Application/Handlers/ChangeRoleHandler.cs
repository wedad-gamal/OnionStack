

using Application.Abstractions.Services;

namespace Application.Handlers
{
    public class ChangeRoleHandler : IRequestHandler<ChangeRoleCommand, bool>
    {
        private readonly IRoleService _roleService;
        private readonly INotificationService _notificationService;

        public ChangeRoleHandler(IRoleService roleService, INotificationService notificationService)
        {
            _roleService = roleService;
            _notificationService = notificationService;
        }

        public async Task<bool> Handle(ChangeRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _roleService.AddUsersToRoleAsync(request.RoleName, request.Users);
            foreach (var user in result)
            {
                if (user.Succeed.HasValue && user.Succeed.Value)
                {
                    foreach (var userId in request.Users)
                    {
                        await _notificationService.NotifyUserAsync(user.UserId, $"Your role has been changed to {request.RoleName}");
                    }
                }
            }

            return true;
        }
    }

}

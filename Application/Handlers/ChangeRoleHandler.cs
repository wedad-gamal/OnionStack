using Application.Common.Interfaces.Background;
using Application.Common.Interfaces.Services;

namespace Application.Handlers
{
    public class ChangeRoleHandler : IRequestHandler<ChangeRoleCommand, bool>
    {
        private readonly IRoleService _roleService;
        private readonly INotificationService _notificationService;
        private readonly IBackgroundJobService _backgroundJobService;

        public ChangeRoleHandler(IRoleService roleService, INotificationService notificationService,
            IBackgroundJobService backgroundJobService)
        {
            _roleService = roleService;
            _notificationService = notificationService;
            _backgroundJobService = backgroundJobService;
        }

        public async Task<bool> Handle(ChangeRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _roleService.AddUsersToRoleAsync(request.RoleName, request.Users);
            foreach (var user in result)
            {
                if (user.Succeed.HasValue && user.Succeed.Value)
                {

                    await _notificationService.NotifyUserAsync(user.Email, $"Your role has been changed to {request.RoleName}");

                    // Schedule email job
                    _backgroundJobService.EnqueueSendRoleChangedEmail(
                        user.Email,
                        request.RoleName,
                        user.IsAssigned
                    );
                }
            }

            return true;
        }
    }

}
